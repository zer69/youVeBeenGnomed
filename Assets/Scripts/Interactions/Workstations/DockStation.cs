using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockStation : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => throw new System.NotImplementedException();
    
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private i_GameEvent increasedEnergy;
    [SerializeField] private b_GameEvent wearGlasses;
    [SerializeField] private s_GameEvent hint;
    [SerializeField] private GameObject glasses;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("DockStation parameters")]

    [SerializeField] private int maxEnergy = 20;
    [SerializeField] private int energyIncrement;
    [SerializeField] private int chargingSpeed;

    private int currentEnergy;
    private int glassesMaxEnergy;
    [SerializeField] private bool isGlassesCharging = false;
    [SerializeField] private bool glassesPutOn = false;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory.hasBattery)
        {
            currentEnergy = maxEnergy;
            Destroy(inventory.battery);
            inventory.BatteryIsPicked(false);
            Debug.Log("DockStation is fully charged again");
            hint.Raise("Dock station is fully charged again");
            return true;
        }

        else
        {
            wearGlasses.Raise(glassesPutOn);
            glassesPutOn = !glassesPutOn;
            //isGlassesCharging = !isGlassesCharging;
            glasses.gameObject.SetActive(glassesPutOn);
            chargeGlasses(glassesPutOn);
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void chargeGlasses(bool charging)
    {
        if(charging == true)
        {
            StartCoroutine(ChargingGlasses());
        }
    }

    IEnumerator ChargingGlasses()
    {
        isGlassesCharging = true;// !isGlassesCharging;

        increasedEnergy.Raise(0);

        while (currentEnergy > 0 && isGlassesCharging == true)
        {
            yield return new WaitForSeconds(chargingSpeed);
            if (isGlassesCharging)
            {
                increasedEnergy.Raise(energyIncrement);
                currentEnergy -= 1;
                Debug.Log("Current charge of DockStation is " + currentEnergy);
            }
        }

        isGlassesCharging = false;
        Debug.Log("Charging is over");
        hint.Raise("Charging is over");

        if(currentEnergy == 0)
        {
            hint.Raise("Dock station discharged. Insert a mana-battery to recharge it");
        }
    }

    public void chargingStatus(bool status)
    {
        if(status != true)
        {
            isGlassesCharging = false;
        }
    }
}
