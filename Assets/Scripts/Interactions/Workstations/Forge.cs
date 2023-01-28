using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forge : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Furnance furnace;

    public string InteractionPrompt => _prompt;

    void Start()
    {

    }

    public bool Interact(Interactor interactor)
    {
        furnace.furnaceTemperature += 5f;
        Debug.Log("lol");
        return true;
    }
}
