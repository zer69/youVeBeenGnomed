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
    [SerializeField] private GameObject glasses;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("DockStation parameters")]

    [SerializeField] private int maxEnergy = 20;
    [SerializeField] private int energyIncrement;
    [SerializeField] private int chargingSpeed;

    private int currentEnergy;
    private int glassesMaxEnergy;
    private bool isGlassesCharging = false;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory.hasBattery)
        {
            currentEnergy = maxEnergy;
            Destroy(inventory.battery);
            inventory.BatteryIsPicked(false);
            Debug.Log("DockStation is fully charged again");
            return true;
        }

        else
        {
            wearGlasses.Raise(isGlassesCharging);
            isGlassesCharging = !isGlassesCharging;
            glasses.gameObject.SetActive(isGlassesCharging);
            chargeGlasses(isGlassesCharging);
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
        while (currentEnergy > 0 && isGlassesCharging == true)
        {
            yield return new WaitForSeconds(chargingSpeed);
            if (isGlassesCharging)
            {
                currentEnergy -= 1;
                increasedEnergy.Raise(energyIncrement);
                Debug.Log("Current charge of DockStation is " + currentEnergy);
            }
        }
        Debug.Log("Charging is over");
    }

    public void chargingStatus(bool status)
    {
        if(status != true)
        {
            isGlassesCharging = false;
        }
    }
}
