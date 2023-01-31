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
        Debug.Log(pickableObject == null);

        CheckForTargets();
        //if (!interacting)
            //pickableObject = null;

        InteractWithBox(interacting);


    }

    private void InteractWithBox(bool state)
    {
        if (pickableObject != null)
        {
            if (state)
            {
                inHands = true;
                pickableObject.SetParent(playerTransform);

                pickableObject.transform.localRotation = Quaternion.identity;

                //interacting = false;

                pickableObject.gameObject.layer = defaultLayer;

                pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                ResizeCrossHair();


            }
            else
            {
                inHands = false;
                playerTransform.DetachChildren();
                //pickableObject.localPosition = pickableObject.position;
                //interacting = false;

                pickableObject.gameObject.layer = pickableLayer;

                pickableObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
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
            case "GrabBox":
                if (targeted || inHands)
                interacting = !interacting;
                
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
