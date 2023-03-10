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

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform rightHand;

    public string InteractionPrompt => _prompt;


    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if(!inventory.hasCoal && (!inventory.hasIngot || inventory.hasThongs))
        {
            GiveCoal();
            inventory.CoalIsPicked(true);
        }

        return true;
    }

    void GiveCoal()
    {
        GameObject newCoal = Instantiate(coal);
        newCoal.transform.position = rightHand.position;
        newCoal.transform.rotation = rightHand.rotation;
        newCoal.transform.SetParent(playerTransform);
        newCoal.GetComponent<BoxCollider>().enabled = false;
        newCoal.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("You picked up  some coal");
        ChangePileSize(coalInPile);
    }

    void ChangePileSize(int pileSize)
    {
        if (pileSize > 0)
        {
            pileSize -= 1;
            coalInPile = pileSize;

            if (pileSize == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
