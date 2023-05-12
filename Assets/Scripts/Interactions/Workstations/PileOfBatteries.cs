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

    [SerializeField] private s_GameEvent hint;

    [SerializeField] private go_GameEvent pickObject;

    private GameObject newBattery;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory.rightHandFree == true)
        {
            GiveBattery();
        }
        else
        {
            hint.Raise("Your right hand is busy");
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
        newBattery = Instantiate(battery);
        pickObject.Raise(newBattery);
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
