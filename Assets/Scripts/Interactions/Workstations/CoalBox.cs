using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Box parameters")]

    [SerializeField] private GameObject coal;

    [SerializeField] private int coalInPile = 5;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private s_GameEvent hint;

    [SerializeField] private go_GameEvent pickObject;

    private GameObject newCoal;

    public string InteractionPrompt => _prompt;


    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if(inventory.rightHandFree == true)
        {
            GiveCoal();
        }
        else
        {
            hint.Raise("Your right hand is busy");
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
            gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
