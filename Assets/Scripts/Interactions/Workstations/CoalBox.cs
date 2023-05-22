using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Box parameters")]

    [SerializeField] private GameObject coal;

    [SerializeField] public int coalInPile = 5;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private s_GameEvent hint;

    [SerializeField] private go_GameEvent pickObject;

    [SerializeField] private GameObject coalPile;

    [SerializeField] private s_GameEvent hotkey;
    private bool picked = false;

    [Header("Sound Events")]
    public AK.Wwise.Event GaveCoalSoundEvent;

    private GameObject newCoal;

    public string InteractionPrompt => _prompt;


    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if(inventory.rightHandFree == true)
        {
            GiveCoal();
            GaveCoalSoundEvent.Post(gameObject);
            picked = false;
            hotkey.Raise("inHands");
        }
        else
        {
            if (picked)
            hint.Raise("Your right hand is busy");
            picked = true;
        }

        return true;
    }

    void GiveCoal()
    {
        newCoal = Instantiate(coal);
        pickObject.Raise(newCoal);
        ChangePileSize(-1);
    }

    public void ChangePileSize(int pileSize)
    {
        coalInPile += pileSize;

        if (coalInPile == 0)
        {
            coalPile.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            coalPile.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
