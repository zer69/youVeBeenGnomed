using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraClicker : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Transform playerTransform;

    private Transform pickableObject;
    

    private LayerMask pickableMask;
    private LayerMask interactiveMask;

    private bool targeted;
    private bool inHands;
    private bool canClick = true;
    private bool interacting = false;

    private int defaultLayer;
    private int pickableLayer;

    private bool leftHand = true;
    private bool rightHand = true;

    [SerializeField] private Transform lefttHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform thongsPosition;
    [SerializeField] private float dropPower;



    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        pickableMask = LayerMask.GetMask("Pickable");
        //interactiveMask = LayerMask.GetMask("Interactive");

        playerInput.onActionTriggered += OnPlayerInputActionTriggered;

        defaultLayer = LayerMask.NameToLayer("Ignore Raycast");
        pickableLayer = LayerMask.NameToLayer("Pickable");
        interacting = false;
        inHands = false;
    }

        // Update is called once per frame
    void Update()
    {
       

        CheckForTargets();
        //if (!interacting)
            //pickableObject = null;
        if (interacting && (pickableObject != null))
        {
            PickInteraction();
        }
        interacting = false;
        //Debug.Log(leftHand);
       // Debug.LogWarning(rightHand);

    }

    private void PickInteraction()
    {
        switch (pickableObject.gameObject.tag)
        {
            case "Ingot":
                InteractWithIngot();
                break;
            case "Tool":
                InteractWithTool();
                break;
            case "Box":
                if (leftHand && rightHand)
                    InteractWithBox();
                break;
        }
    }

    private void InteractWithTool()
    {
        switch (pickableObject.GetComponent<Instrument>().type)
        {
            case Instrument.Type.Thongs:
                PickUpTool(false);
                break;
            case Instrument.Type.Hammer:
                PickUpTool(true);
                break;
        }
    }
    
    private void PickUpTool(bool hand) //if false then check for left, if hammer then check for left
    {

    }

    private void InteractWithIngot()
    {
        
        bool targetHand;
        if (leftHand)// hands are true when free, false when full
        {
            targetHand = true; // targets right without thongs
        }
        else
        {
            targetHand = false; // targets left with thongs
        }

        if (targetHand)
        {
            if (rightHand)
            {
                pickableObject.transform.position = rightHandPosition.position;
                rightHand = false;
            }
                
            else
                return;
        }
        else
        {
            if (leftHand)
            {
                pickableObject.transform.position = thongsPosition.position;
                leftHand = false;
            }
                
            else
                return;
        }

        Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        pickableObject.transform.localRotation = Quaternion.identity;
        pickableObject.SetParent(playerTransform);
        
        
        


    }

    private void InteractWithBox()
    {
    
        leftHand = false;
        rightHand = false;
        pickableObject.SetParent(playerTransform);
        pickableObject.transform.localRotation = Quaternion.identity;
        pickableObject.gameObject.layer = defaultLayer;
        pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ResizeCrossHair();
    }

    private void DropBox()
    {
        
        leftHand = true;
        rightHand = true;
        playerTransform.DetachChildren();
        //pickableObject.localPosition = pickableObject.position;
        //interacting = false;

        pickableObject.gameObject.layer = pickableLayer;

        pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
    IEnumerator MoveBoxToHands()
    {
        

        yield return null;
    }

    private void CheckForTargets()
    {
        if (canClick)
        {
            RaycastHit mousehit;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out mousehit, 3.0f, pickableMask))
            {
                pickableObject = mousehit.transform.gameObject.transform;
                targeted = true;
            }
            else
            {
                targeted = false;
            }
            ResizeCrossHair();
        }
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Use":
                interacting = true;
                break;
            case "DropItems":
                DropHands();
                break;
        }
    }

    private void DropHands()
    {
        
        foreach (Transform child in playerTransform)
        {
            if (child.gameObject.tag == "Box")
            {
                DropBox();
                break;
            }
            else
            {
                child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                child.GetComponent<Rigidbody>().AddForce(transform.forward * dropPower);
            }
            
        }
        playerTransform.DetachChildren();
        rightHand = true;
        leftHand = true;

    }

    private void ResizeCrossHair()
    {
        if (targeted)
        {
            crosshair.sizeDelta = new Vector2(16, 16);
        }
        else
        {
            crosshair.sizeDelta = new Vector2(4, 4);
        }
    }
}
