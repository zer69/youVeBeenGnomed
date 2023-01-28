using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Furnance : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    private bool fuelIsFilled = false;
    private bool fireIsKindled = false;

    public float furnaceTemperature = 0f;
    private float minFireTemperature = 0f;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        //if(inventory == null) return false;

        if (fireIsKindled)
        {
            cam.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
            playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerInput.transform.localRotation = Quaternion.identity;
            Debug.Log("Furnance is used");
            return true;
        }

        else
        {
            if (fuelIsFilled)
            {
                StartCoroutine(Burning());
                //fireIsKindled = true;
                //Debug.Log("Fire Is Kindled");
                return true;
            }

            else
            {
                if (inventory.hasCoal)
                {
                    fuelIsFilled = inventory.hasCoal;
                    inventory.hasCoal = false;
                    Debug.Log("Fuel Is Filled");
                    return true;
                }

                else
                {
                    Debug.Log("No fuel for furnace");
                    return true;
                }
                fuelIsFilled = inventory.hasCoal;
                Debug.Log("Fuel Is Filled");
                return true;
            }
        }

        //Debug.Log("No fuel for furnace");
        //return false;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
                cam.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);

                playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                break;

            case "Kindle":
                cam.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);

                playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                break;
        }
    }

    IEnumerator Burning()
    {
        fireIsKindled = true;
        furnaceTemperature = 10f;
        Debug.Log("Fire Is Kindled");

        while(furnaceTemperature > minFireTemperature)
        {
            yield return new WaitForSeconds(1);
            furnaceTemperature -= 1;
            Debug.Log("Current temperature of furnace is " + furnaceTemperature + "*C");
        }

        fireIsKindled = false;
        fuelIsFilled = false;
        Debug.Log("Fire went out");
    }
}
