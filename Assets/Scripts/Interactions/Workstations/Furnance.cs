using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Furnance : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private PlayerInput playerInput;    

    private bool fuelIsFilled = false;
    private bool fireIsKindled = false;
    private bool ingotInFurnace = false;

    public float furnaceTemperature = 0f;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Furnace burning parameters")]

    [SerializeField] private float furnaceInitialTemperature;
    [SerializeField] private float smeltingSpeed;
    [SerializeField] private float minFireTemperature = 0f;

    [BackgroundColor()]

    private GameObject ingot;
    private GameObject thongs;

    
    public string InteractionPrompt => _prompt;

    void Start()
    {
        thongs = GameObject.Find("Thongs");
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (ingotInFurnace && inventory.hasThongs)
        {
            ingot.transform.position = thongs.transform.Find("ThongsPosition").position;
            ingot.transform.SetParent(thongs.transform);
            ingot.GetComponent<BoxCollider>().enabled = false;
            ingot.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //ingot.GetComponent<Rigidbody>().isKinematic = true;
            inventory.IngotIsPicked(true);
            ingotInFurnace = false;
            Debug.Log("Ingot taken");
            return true;
        }

        if (ingotInFurnace)
        {            
            Debug.Log("You need thongs to get an ingot from the furnace");
            return true;
        }

        else if (fireIsKindled && !inventory.hasIngot)
        {            
            Debug.Log("No ingot");
            return true;
        }

        else if(fireIsKindled && inventory.hasIngot && !ingotInFurnace)
        {
            ingot = inventory.ingot;
            inventory.IngotIsPicked(false);
            ingot.transform.position = new Vector3(-2.5f, 3.6f, -2);
            ingot.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
            ingot.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //ingot.GetComponent<Rigidbody>().isKinematic = false;
            ingot.GetComponent<BoxCollider>().enabled = true;
            ingot.transform.parent = null;

            ingotInFurnace = true;

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
                    Destroy(inventory.coal);
                    inventory.CoalIsPicked(false);
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
            if (ingotInFurnace)
            {
                Debug.Log("Current temperature of ingot is " + ingot.gameObject.GetComponent<Ingot>().currentTemperature + "*C");
            }
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
