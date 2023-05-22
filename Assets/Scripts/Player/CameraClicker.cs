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
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Transform playerTransform;

    [Header("Events")]
    [BackgroundColor(.75f, 0f, 1.5f, 1f)]
    [SerializeField] private t_GameEvent typeChoice;
    [SerializeField] private col_GameEvent weaponPicked;
    [SerializeField] private b_GameEvent crosshairResized;
    [SerializeField] private go_GameEvent pickObject;
    [SerializeField] private s_GameEvent hotkey;

    [SerializeField] private Transform pickableObject;
    
    private LayerMask pickableMask;
    private LayerMask interactiveMask;

    private bool targeted;
    private bool canClick = true;
    private bool interacting = false;

    private int defaultLayer;
    private int pickableLayer;
    private int toolLayer;

    [Header("Sound Events")]
    public AK.Wwise.Event ToolDropedSoundEvent;


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
        pickObject.Raise(pickableObject.gameObject);
        hotkey.Raise("inHands");
        weaponPicked.Raise(pickableObject.GetComponent<Collider>());
        pickableObject = null;
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
            child.GetComponent<Rigidbody>().AddForce(cameraRotation * Vector3.forward * dropPower);
            playerInput.GetComponent<Inventory>().ThongsIsPicked(false);
            playerInput.GetComponent<Inventory>().IngotIsPicked(false);
            if (playerInput.GetComponent<Inventory>().hasCoal)
            {
                ToolDropedSoundEvent.Post(gameObject);
            }
            playerInput.GetComponent<Inventory>().CoalIsPicked(false);
            playerInput.GetComponent<Inventory>().BatteryIsPicked(false);
            playerInput.GetComponent<Inventory>().HammerIsPicked(false);
        }
        playerTransform.DetachChildren();
        hotkey.Raise("main");
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
