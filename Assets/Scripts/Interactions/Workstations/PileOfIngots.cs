using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfIngots : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Pile parameters")]

    [SerializeField] private GameObject ingot;

    [SerializeField] public int ingotsInPile = 5;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform rightHand;

    [SerializeField] private GameObject ingots;

    [SerializeField] private bool glassesActivated = false;

    [SerializeField] private t_GameEvent typeChoice;
    [SerializeField] private s_GameEvent hint;
    [SerializeField] private go_GameEvent pickObject;

    private GameObject thongs;
    private GameObject newIngot;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        thongs = GameObject.Find("Thongs");
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if ((inventory.hasIngot == false && inventory.rightHandFree) || (inventory.hasIngotInThongs == false && inventory.hasThongs == true))
        {
            GiveIngot();
        }
        else
        {
            hint.Raise("Your hands are busy");
        }   

        return true;
    }

    void GiveIngot()
    {
        newIngot = Instantiate(ingot);
        pickObject.Raise(newIngot);
        newIngot.transform.Find("Ingot temperature (TMP)").gameObject.SetActive(glassesActivated);
        ChangePileSize(-1);
    }

    public void ChangePileSize(int pileSize)
    {
        ingotsInPile += pileSize;

        if(ingotsInPile == 0)
        {
            ingots.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            ingots.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void GlassesStatus(bool active)
    {
        if (active)
        {
            glassesActivated = true;
        }

        else
        {
            glassesActivated = false;
        }
    }
}
