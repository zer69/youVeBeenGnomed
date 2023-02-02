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
    [SerializeField] Transform thongs;

    private Transform pickableObject;
    

    private LayerMask pickableMask;
    private LayerMask interactiveMask;

    private bool targeted;
    private bool inHands;
    private bool canClick = true;
    private bool interacting = false;

    private int defaultLayer;
    private int pickableLayer;
    private int toolLayer;

    private bool leftHand = true;
    private bool rightHand = true;
    private bool leftWithIngot = false;

    [SerializeField] private Transform lefttHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform thongsPosition;
    [SerializeField] private float dropPower;



    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        defaultLayer = LayerMask.NameToLayer("Ignore Raycast");
        pickableLayer = LayerMask.NameToLayer("Pickable");
        toolLayer = LayerMask.NameToLayer("Tool");
        pickableMask = LayerMask.GetMask("Pickable");
        pickableMask |= (1 << toolLayer);
        //interactiveMask = LayerMask.GetMask("Interactive");

        playerInput.onActionTriggered += OnPlayerInputActionTriggered;

        
        
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
        //Debug.Log(pickableObject.gameObject.tag);
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
    
    private void PickUpTool(bool hand) //if false then check for left, if hammer then check for right
    {
        if (hand)
        {
            if (rightHand)
            {
                pickableObject.transform.position = rightHandPosition.position;
                pickableObject.transform.rotation = rightHandPosition.rotation;
                pickableObject.SetParent(playerTransform);
                rightHand = false;
            }
            else
                return;
        }
        else
        {
            pickableObject.transform.position = lefttHandPosition.position;
            pickableObject.transform.rotation = lefttHandPosition.rotation;
            pickableObject.SetParent(playerTransform);
            leftHand = false;
        }
        Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

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
                pickableObject.transform.rotation = rightHandPosition.rotation;
                pickableObject.SetParent(playerTransform);
                rightHand = false;
            }
                
            else
                return;
        }
        else
        {
            if (leftWithIngot)
            {
                //showhint
                return;
            }
            pickableObject.transform.position = thongsPosition.position;
            pickableObject.transform.rotation = thongsPosition.rotation;
            pickableObject.SetParent(thongs);
            leftHand = false;
            pickableObject.GetComponent<Rigidbody>().isKinematic = true;
            pickableObject.GetComponent<BoxCollider>().enabled = false;
            leftWithIngot = true;
        }

        Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
        
        
        playerInput.GetComponent<Inventory>().IngotIsPicked(true);
        


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
                if (interacting)
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
                playerInput.GetComponent<Inventory>().IngotIsPicked(false);
            }
            
        }
        playerTransform.DetachChildren();
        rightHand = true;
        leftHand = true;
        pickableObject = null;
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
