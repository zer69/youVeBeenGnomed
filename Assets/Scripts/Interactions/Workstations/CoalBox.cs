using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    

    public string InteractionPrompt => _prompt;


    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();
        inventory.hasCoal = true;
        Debug.Log("You picked up  some coal");
        return true;
    }
}
