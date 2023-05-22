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
    [SerializeField] private go_GameEvent pickObject;

    [SerializeField] private Transform placeForIngot;

    [SerializeField] private TextMeshPro TemperatureText;

    private bool fuelIsFilled = false;
    public bool fireIsKindled = false;

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
    [Header("Sound Events")]
    public AK.Wwise.Event ThrowCoalPikedSoundEvent;
    public enum FurnaceState
    {
        NoFuel,
        HasFuel,
        Kindled
    }

    public enum IngotState
    {
        NoIngot,
        HasIngot
    }

    public FurnaceState state;
    public IngotState ingotState;
    
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
    }

    IEnumerator Burning()
    {
        fireIsKindled = true;
        state = FurnaceState.Kindled;
        furnaceTemperature = furnaceInitialTemperature;
        Debug.Log("Fire Is Kindled");
        hint.Raise("Fire is kindled. Now you can put ingot in the furnace");

        while (furnaceTemperature > minFireTemperature)
        {
            yield return new WaitForSeconds(1);
            furnaceTemperature -= 1;

            TemperatureText.text = furnaceTemperature.ToString() + "*C";
            Debug.Log("Current temperature of furnace is " + furnaceTemperature + "*C");
        }

        fireIsKindled = false;
        fuelIsFilled = false;
        state = FurnaceState.NoFuel;
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
                Refuel();
                break;

            case FurnaceState.HasFuel:
                StartCoroutine(Burning());
                break;

            case FurnaceState.Kindled:
                InteractWithIngotInFurnace();
                break;
        }
    }

    void Refuel()
    {
        if (ingotState == IngotState.HasIngot)
        {
            InteractWithIngotInFurnace();
        }
        else if (inventory.hasCoal)
        {
            fuelIsFilled = inventory.hasCoal;
            Destroy(inventory.coal);
            inventory.CoalIsPicked(false);
            Debug.Log("Fuel Is Filled");
            hint.Raise("Fuel is filled. Now you can start a fire in the furnace");
            state = FurnaceState.HasFuel;
            ThrowCoalPikedSoundEvent.Post(gameObject);
        }
        else
        {
            Debug.Log("No fuel for furnace");
            hint.Raise("To start a fire, you need to put coal in the furnace");
        }
    }

    void InteractWithIngotInFurnace()
    {
        switch (ingotState)
        {
            case IngotState.NoIngot:
                if (inventory.hasIngot)
                {
                    ingot = inventory.ingot;
                    inventory.IngotIsPicked(false);
                }
                else if (inventory.hasIngotInThongs)
                {
                    ingot = inventory.ingotInThongs;
                    inventory.IngotIsPicked(false);
                }

                ingot.transform.position = placeForIngot.transform.position;
                ingot.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
                ingot.transform.parent = null;

                Debug.Log("Ingot placed in the furnace");
                ingotState = IngotState.HasIngot;
                break;

            case IngotState.HasIngot:
                if (inventory.hasThongs && !inventory.hasIngotInThongs)
                {
                    pickObject.Raise(ingot);
                    ingot = null;
                    Debug.Log("Ingot taken");
                    ingotState = IngotState.NoIngot;
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
