using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string InteractionPrompt { get; }

    bool Interact(Interactor interactor);

}
