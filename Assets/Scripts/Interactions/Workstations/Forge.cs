using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forge : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Furnance furnace;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Inflation parameters")]

    [SerializeField] private float temperatureIncrement;
    [SerializeField] private float forgeCooldown = 1f;

    [BackgroundColor()]

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
        yield return new WaitForSeconds(forgeCooldown);

        canInflateForge = true;
    }
}
