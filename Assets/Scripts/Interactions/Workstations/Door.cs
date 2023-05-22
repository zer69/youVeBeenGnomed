using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Transform forgeryPosition;
    [SerializeField] private Transform magicPosition;

    [SerializeField] private Transform player;

    [Header("Sound Events")]
    public AK.Wwise.Event DoorOpenSoundEvent;
    private bool inForgery = false;

    public string InteractionPrompt => _prompt;


    public bool Interact(Interactor interactor)
    {
        
        if (inForgery)
        {
            forgeryPosition.position = player.position;
            player.position = magicPosition.position;
            inForgery = false;
            DoorOpenSoundEvent.Post(gameObject);
        }
        else
        {
            magicPosition.position = player.position;
            player.position = forgeryPosition.position;
            inForgery = true;
            DoorOpenSoundEvent.Post(gameObject);
        }


        return true;
    }
}
