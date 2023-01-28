using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forge : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Furnance furnace;
    [SerializeField] private float temperatureIncrement;

    private bool canInflateForge = true;

    public string InteractionPrompt => _prompt;

    void Start()
    {

    }

    public bool Interact(Interactor interactor)
    {
        useForge();
        return true;
    }

    void useForge()
    {
        if (canInflateForge)
        {
            StartCoroutine(InflateForge());
        }
    }

    IEnumerator InflateForge()
    {
        canInflateForge = false;

        furnace.furnaceTemperature += temperatureIncrement;
        Debug.Log("Forge inflated");
        yield return new WaitForSeconds(1f);

        canInflateForge = true;
    }
}
