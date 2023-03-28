using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfBatteries : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Box parameters")]

    [SerializeField] private GameObject battery;

    [SerializeField] private int batteriesInPile = 5;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform rightHand;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (!inventory.hasBattery && (!inventory.hasIngot || inventory.hasThongs))
        {
            GiveBattery();
            inventory.BatteryIsPicked(true);
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GiveBattery()
    {
        GameObject newBattery = Instantiate(battery);
        newBattery.transform.position = rightHand.position;
        newBattery.transform.rotation = rightHand.rotation;
        newBattery.transform.SetParent(playerTransform);
        newBattery.GetComponent<BoxCollider>().enabled = false;
        newBattery.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //newCoal.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("You picked up battery");
        ChangePileSize(batteriesInPile);
    }

    void ChangePileSize(int pileSize)
    {
        if (pileSize > 0)
        {
            pileSize -= 1;
            batteriesInPile = pileSize;

            if (pileSize == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
