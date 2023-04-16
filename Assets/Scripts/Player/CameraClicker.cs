using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraClicker : MonoBehaviour
{
    [Header("Camera Stats")]
    [BackgroundColor (0f, 1.5f, 0f, 1f)]
    [SerializeField] private float dropPower;
    [SerializeField] private float interactRange = 3.0f;

    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Transform lefttHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform thongsPosition;
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform thongs;
    public bool leftHand = true;
    public bool rightHand = true;
    public bool leftWithIngot = false;

    [Header("Events")]
    [BackgroundColor(.75f, 0f, 1.5f, 1f)]
    [SerializeField] private t_GameEvent typeChoice;
    [SerializeField] private col_GameEvent weaponPicked;
    [SerializeField] private b_GameEvent crosshairResized;

    private Transform pickableObject;
    private LayerMask pickableMask;
    private LayerMask interactiveMask;

    private bool targeted;
    private bool canClick = true;
    private bool interacting = false;

    private int defaultLayer;
    private int pickableLayer;
    private int toolLayer;




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
        //inHands = false;
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
                pickableObject = null;
                break;
            case "Tool":
                InteractWithTool();
                break;
            case "Coal":
                InteractWithCoal();
                break;
            case "Battery":
                InteractWithBattery();
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

                playerInput.GetComponent<Inventory>().HammerIsPicked(true);
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

            playerInput.GetComponent<Inventory>().ThongsIsPicked(true);
        }
        Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        pickableObject = null;
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
            pickableObject.SetParent(thongsPosition);
            leftHand = false;
            pickableObject.GetComponent<BoxCollider>().enabled = false;
            leftWithIngot = true;
        }

        Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        if (pickableObject.GetComponent<Ingot>().weaponType == Ingot.WeaponType.None)
        {
            typeChoice.Raise(pickableObject);
        }

        playerInput.GetComponent<Inventory>().IngotIsPicked(true);
        weaponPicked.Raise(pickableObject.GetComponent<BoxCollider>());

        //pickableObject = null;
    }

    private void InteractWithCoal()
    {
        if (rightHand)
        {
            pickableObject.transform.position = rightHandPosition.position;
            pickableObject.transform.rotation = rightHandPosition.rotation;
            pickableObject.SetParent(playerTransform);

            rightHand = false;

            Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;

            playerInput.GetComponent<Inventory>().CoalIsPicked(true);

            pickableObject = null;
        }

        else
        {
            Debug.Log("Your right hand is busy");
        }
    }
    private void InteractWithBattery()
    {
        if (rightHand)
        {
            pickableObject.transform.position = rightHandPosition.position;
            pickableObject.transform.rotation = rightHandPosition.rotation;
            pickableObject.SetParent(playerTransform);

            rightHand = false;

            Rigidbody rb = pickableObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;

            playerInput.GetComponent<Inventory>().BatteryIsPicked(true);

            pickableObject = null;
        }

        else
        {
            Debug.Log("Your right hand is busy");
        }
    }

    private void CheckForTargets()
    {
        if (canClick)
        {
            RaycastHit mousehit;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out mousehit, interactRange, pickableMask))
            {
                if (interacting)
                    pickableObject = mousehit.transform.gameObject.transform;
                targeted = true;
                crosshairResized.Raise(true);
            }
            else
            {
                targeted = false;
            }
            //ResizeCrossHair();
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
        Quaternion cameraRotation = this.transform.rotation;
        
        foreach (Transform child in playerTransform)
        {
                
                child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                child.GetComponent<Rigidbody>().AddForce(cameraRotation* Vector3.forward * dropPower);
                playerInput.GetComponent<Inventory>().ThongsIsPicked(false);
                playerInput.GetComponent<Inventory>().IngotIsPicked(false);
                playerInput.GetComponent<Inventory>().CoalIsPicked(false);
                playerInput.GetComponent<Inventory>().BatteryIsPicked(false);
                playerInput.GetComponent<Inventory>().HammerIsPicked(false);
        }
        playerTransform.DetachChildren();
        rightHand = true;
        leftHand = true;
        leftWithIngot = false;
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
