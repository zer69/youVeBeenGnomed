using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private RectTransform crosshair;

    private Camera cam;
    private Camera currentCam;

    private bool isWorkstationTargeted = false;
    private bool usingStation = false;

    private IInteractable interactable;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        currentCam = cam;
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWorkstation();
        UseWorkstation();
        ResizeCrosshair();
    }

    private void CheckForWorkstation()
    {
        RaycastHit hit;

        if(Physics.Raycast(currentCam.transform.position, currentCam.transform.forward, out hit, 3, interactableLayerMask))
        {
            isWorkstationTargeted = true;
            interactable = hit.collider.GetComponent<IInteractable>();
            //currentCam = hit.collider.gameObject.GetComponentInChildren<Camera>(true);
        }
        else
        {
            isWorkstationTargeted = false;
        }
    }

    private void ResizeCrosshair()
    {
        if (isWorkstationTargeted)
        {
            crosshair.sizeDelta = new Vector2(16, 16);
        }
        else
        {
            crosshair.sizeDelta = new Vector2(4, 4);
        }
    }

    private void UseWorkstation()
    {
        if (isWorkstationTargeted == true && usingStation == true)
        {
            interactable.Interact(this);
            usingStation = false;
        }
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Use":
                if (isWorkstationTargeted)
                {
                    if (context.phase == InputActionPhase.Started)
                    {
                        usingStation = true;
                    }
                    else if (context.phase == InputActionPhase.Canceled)
                    {
                        usingStation = false;
                    }
                }

                break;

            case "Abort":
                usingStation = false;

                break;
        }
    }
}
