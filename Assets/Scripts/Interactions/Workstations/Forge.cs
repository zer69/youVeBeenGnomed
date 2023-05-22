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
    [SerializeField] private s_GameEvent hint;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Inflation parameters")]

    [SerializeField] private float temperatureIncrement;
    [SerializeField] private float forgeCooldown = 1f;

    [BackgroundColor()]
    [Header("Sound Events")]
    public AK.Wwise.Event BellowsSoundEvent; 

    private bool canInflateForge = true;

    public string InteractionPrompt => _prompt;

    void Start()
    {

    }

    public bool Interact(Interactor interactor)
    {
        useForge();
        BellowsSoundEvent.Post(gameObject);
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
        
        Debug.Log("Forge inflated");
        if(furnace.fireIsKindled)
        {
            furnace.furnaceTemperature += temperatureIncrement;
            hint.Raise("The fire burns stronger");
        }
        yield return new WaitForSeconds(forgeCooldown);

        canInflateForge = true;
    }
}
