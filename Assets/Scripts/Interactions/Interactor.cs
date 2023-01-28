using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask interactableLayerMask;

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
    }

    private void CheckForWorkstation()
    {
        RaycastHit hit;

        if(Physics.Raycast(currentCam.transform.position, currentCam.transform.forward, out hit, 2, interactableLayerMask))
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

    private void UseWorkstation()
    {
        if (isWorkstationTargeted == true && usingStation == true)
        {
            interactable.Interact(this);
            usingStation = false;
            //playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //playerInput.transform.localRotation = Quaternion.identity;
        }
        else
        {
            currentCam = cam;
            //playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
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
