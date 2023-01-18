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
    private bool canClick = true;
    private bool interacting = false;
    private bool leftHand = true;
    private bool rightHand = true;

    private int defaultLayer;
    private int pickableLayer;
    


    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        pickableMask = LayerMask.GetMask("Pickable");
        //interactiveMask = LayerMask.GetMask("Interactive");

        playerInput.onActionTriggered += OnPlayerInputActionTriggered;

        defaultLayer = LayerMask.NameToLayer("Ignore Raycast");
        pickableLayer = LayerMask.NameToLayer("Pickable");
    }

        // Update is called once per frame
    void Update()
    {
        CheckForTargets();
        

        if (interacting)
            InteractWithBox();

        Debug.Log(targeted);

    }

    private void InteractWithBox()
    {
        if (targeted)
        {
            
            pickableObject.SetParent(playerTransform);
            
            pickableObject.transform.localRotation = Quaternion.identity;
            
            interacting = false;
            rightHand = false;
            leftHand = false;

            pickableObject.gameObject.layer = defaultLayer;

            pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ResizeCrossHair();


        }
        else
        {

            playerTransform.DetachChildren();
            //pickableObject.localPosition = pickableObject.position;
            interacting = false;
            rightHand = true;
            leftHand = true;

            pickableObject.gameObject.layer = pickableLayer;

            pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
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
            case "Interact":
                interacting = true;
                
                break;
        }
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
