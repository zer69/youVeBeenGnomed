using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Furnance : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private PlayerInput playerInput;    

    private bool fuelIsFilled = false;
    private bool fireIsKindled = false;

    [BackgroundColor(4f, 0f, 0f, 1f)]
    public float furnaceTemperature = 0f;

    [BackgroundColor(0f, 4f, 0f, 1f)]
    [SerializeField] private float furnaceInitialTemperature;
    [SerializeField] private float smeltingSpeed;
    [BackgroundColor()]
    private float minFireTemperature = 0f;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (fireIsKindled && !inventory.hasIngot)
        {            
            Debug.Log("No ingot");
            return true;
        }

        else if(fireIsKindled && inventory.hasIngot)
        {
            Debug.Log("Ingot placed in the furnace");
            return true;
        }

        else
        {
            if (fuelIsFilled)
            {
                StartCoroutine(Burning());
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
            }
        }
    }

    IEnumerator Burning()
    {
        fireIsKindled = true;
        furnaceTemperature = furnaceInitialTemperature;
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

    void smeltingIngot(Collision ingot)
    {
        float ingotTemperature = ingot.gameObject.GetComponent<Ingot>().currentTemperature;

        if(ingotTemperature < furnaceTemperature)
        {
            ingot.gameObject.GetComponent<Ingot>().currentTemperature += smeltingSpeed * Time.deltaTime;
            //Debug.Log("Current temperature of ingot is " + ingotTemperature + "*C");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ingot")
        {
            smeltingIngot(collision);
        }
    }
}
