using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Furnance : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private s_GameEvent hint;

    [SerializeField] private Transform placeForIngot;

    [SerializeField] private TextMeshPro TemperatureText;

    private bool fuelIsFilled = false;
    public bool fireIsKindled = false;
    private bool ingotInFurnace = false;

    public float furnaceTemperature = 0f;
    
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Furnace burning parameters")]

    [SerializeField] private float furnaceInitialTemperature;
    [SerializeField] private float smeltingSpeed;
    [SerializeField] private float minFireTemperature = 0f;

    [BackgroundColor()]

    [SerializeField] private GameObject ingot;
    private GameObject thongs;
    private Inventory inventory;

    public enum FurnaceState
    {
        NoFuel,
        HasFuel,
        Kindled,
        NoIngot,
        HasIngot
    }

    public FurnaceState state;

    
    public string InteractionPrompt => _prompt;

    void Start()
    {
        thongs = GameObject.Find("Thongs");
        inventory = GameObject.Find("PLAYER").GetComponent<Inventory>();
    }

    public bool Interact(Interactor interactor)
    {
        InteractWithFurnace();
        return true;
        //var inventory = interactor.GetComponent<Inventory>();

        //if (ingotInFurnace && inventory.hasThongs && !inventory.hasIngotInThongs)
        //{
        //    ingot.transform.position = thongs.transform.Find("ThongsPosition").position;
        //    ingot.transform.rotation = thongs.transform.Find("ThongsPosition").rotation;
        //    ingot.transform.SetParent(thongs.transform.Find("ThongsPosition"));
        //    ingot.GetComponent<BoxCollider>().enabled = false;
        //    ingot.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //    //ingot.GetComponent<Rigidbody>().isKinematic = true;
        //    inventory.IngotIsPicked(true);
        //    ingotInFurnace = false;
        //    ingot = null;
        //    Debug.Log("Ingot taken");
        //    return true;
        //}

        //if (ingotInFurnace)
        //{            
        //    Debug.Log("You need empty thongs to get an ingot from the furnace");
        //    hint.Raise("You need empty thongs to get an ingot from the furnace");
        //    return true;
        //}

        //else if (fireIsKindled && !inventory.hasIngot && !inventory.hasIngotInThongs)
        //{            
        //    Debug.Log("No ingot");
        //    hint.Raise("You need ingot to put it in the furnace");
        //    return true;
        //}

        //else if(fireIsKindled && !ingotInFurnace)
        //{
        //    if (inventory.hasIngot)
        //    {
        //        ingot = inventory.ingot;
        //        inventory.IngotIsPicked(false);
        //    }
        //    else
        //    {
        //        ingot = inventory.ingotInThongs;
        //        inventory.IngotIsPicked(false);
        //    }

        //    ingot.transform.position = placeForIngot.transform.position;
        //    ingot.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
        //    //ingot.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //    //ingot.GetComponent<Rigidbody>().isKinematic = false;
        //    //ingot.GetComponent<BoxCollider>().enabled = true;
        //    ingot.transform.parent = null;

        //    ingotInFurnace = true;

        //    Debug.Log("Ingot placed in the furnace");
        //    return true;
        //}

        //else
        //{
        //    if (fuelIsFilled)
        //    {
        //        StartCoroutine(Burning());
        //        return true;
        //    }

        //    else
        //    {
        //        if (inventory.hasCoal)
        //        {
        //            fuelIsFilled = inventory.hasCoal;
        //            Destroy(inventory.coal);
        //            inventory.CoalIsPicked(false);
        //            Debug.Log("Fuel Is Filled");
        //            hint.Raise("Fuel is filled. Now you can start a fire in the furnace");
        //            return true;
        //        }

        //        else
        //        {
        //            Debug.Log("No fuel for furnace");
        //            hint.Raise("To start a fire, you need to put coal in the furnace");
        //            return true;
        //        }
        //    }
        //}
    }

    IEnumerator Burning()
    {
        fireIsKindled = true;
        furnaceTemperature = furnaceInitialTemperature;
        Debug.Log("Fire Is Kindled");
        hint.Raise("Fire is kindled. Now you can put ingot in the furnace");

        while (furnaceTemperature > minFireTemperature)
        {
            yield return new WaitForSeconds(1);
            furnaceTemperature -= 1;

            TemperatureText.text = furnaceTemperature.ToString() + "*C";
            Debug.Log("Current temperature of furnace is " + furnaceTemperature + "*C");
            if (ingotInFurnace)
            {
                Debug.Log("Current temperature of ingot is " + ingot.gameObject.GetComponent<Ingot>().currentTemperature + "*C");
            }
        }

        fireIsKindled = false;
        fuelIsFilled = false;
        Debug.Log("Fire went out");
        hint.Raise("Fire went out");
    }

    void smeltingIngot(Collision ingotCollision)
    {
        Ingot ingot = ingotCollision.gameObject.GetComponent<Ingot>();

        if(ingot.currentTemperature < furnaceTemperature)
        {
            ingot.Heating(furnaceTemperature, smeltingSpeed);
            ingot.Melting();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ingot")
        {
            //we may have problems here when furnace is colder than ingot
            if (furnaceTemperature > collision.gameObject.GetComponent<Ingot>().currentTemperature)
            {
                collision.gameObject.GetComponent<Ingot>().setZeroCoolingRate();
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ingot")
        {
            collision.gameObject.GetComponent<Ingot>().setNormalCoolingRate();
        }
    }

    private void InteractWithFurnace()
    {
        switch (state)
        {
            case FurnaceState.NoFuel:
                if (inventory.hasCoal)
                {
                    fuelIsFilled = inventory.hasCoal;
                    Destroy(inventory.coal);
                    inventory.CoalIsPicked(false);
                    Debug.Log("Fuel Is Filled");
                    hint.Raise("Fuel is filled. Now you can start a fire in the furnace");
                    state = FurnaceState.HasFuel;
                }
                else
                {
                    Debug.Log("No fuel for furnace");
                    hint.Raise("To start a fire, you need to put coal in the furnace");
                }
                break;

            case FurnaceState.HasFuel:
                StartCoroutine(Burning());
                state = FurnaceState.Kindled;
                break;

            case FurnaceState.Kindled:
                if (inventory.hasIngot)
                {
                    ingot = inventory.ingot;
                    inventory.IngotIsPicked(false);
                }
                else
                {
                    ingot = inventory.ingotInThongs;
                    inventory.IngotIsPicked(false);
                }

                ingot.transform.position = placeForIngot.transform.position;
                ingot.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
                ingot.transform.parent = null;

                ingotInFurnace = true;

                Debug.Log("Ingot placed in the furnace");
                state = FurnaceState.HasIngot;
                break;

            case FurnaceState.NoIngot:
                Debug.Log("No ingot");
                hint.Raise("You need ingot to put it in the furnace");
                break;

            case FurnaceState.HasIngot:
                if (inventory.hasThongs && !inventory.hasIngotInThongs)
                {
                    ingot.transform.position = thongs.transform.Find("ThongsPosition").position;
                    ingot.transform.rotation = thongs.transform.Find("ThongsPosition").rotation;
                    ingot.transform.SetParent(thongs.transform.Find("ThongsPosition"));
                    ingot.GetComponent<BoxCollider>().enabled = false;
                    ingot.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    //ingot.GetComponent<Rigidbody>().isKinematic = true;
                    inventory.IngotIsPicked(true);
                    ingotInFurnace = false;
                    ingot = null;
                    Debug.Log("Ingot taken");
                    state = FurnaceState.Kindled;
                }
                else
                {
                    Debug.Log("You need empty thongs to get an ingot from the furnace");
                    hint.Raise("You need empty thongs to get an ingot from the furnace");
                }
                break;
        }
    }
}
