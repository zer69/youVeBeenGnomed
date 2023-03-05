using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfIngots : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private GameObject ingot;

    [SerializeField] private int ingotsInPile = 5;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform rightHand;

    private GameObject thongs;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        thongs = GameObject.Find("Thongs");
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory.hasThongs && !inventory.hasIngot)
        {
            GameObject newIngot = Instantiate(ingot);
            newIngot.transform.position = thongs.transform.Find("ThongsPosition").position;
            newIngot.transform.SetParent(thongs.transform);
            newIngot.GetComponent<BoxCollider>().enabled = false;
            newIngot.GetComponent<Rigidbody>().isKinematic = true;
            inventory.IngotIsPicked(true);
            ChangePileSize(ingotsInPile);
            Debug.Log("Ingot taken");
        }

        else if (!inventory.hasThongs && !inventory.hasIngot && !inventory.coal)
        {
            GameObject newIngot = Instantiate(ingot);
            newIngot.transform.position = rightHand.position;
            newIngot.transform.rotation = rightHand.rotation;
            newIngot.transform.SetParent(playerTransform);
            newIngot.GetComponent<BoxCollider>().enabled = false;
            newIngot.GetComponent<Rigidbody>().isKinematic = true;
            inventory.IngotIsPicked(true);
            ChangePileSize(ingotsInPile);
            Debug.Log("Ingot taken");
        }

        return true;
    }

    void ChangePileSize(int pileSize)
    {
        if(pileSize > 0)
        {
            pileSize -= 1;
            ingotsInPile = pileSize;

            if(pileSize == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
