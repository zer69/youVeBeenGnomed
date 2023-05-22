using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform menu;
    public VolumeProfile globalVolume;
    private DepthOfField dof;

    private void Start()
    {
        globalVolume.TryGet(out dof);
        dof.active = true;

        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.DeactivateInput();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        globalVolume.TryGet(out dof);
        dof.active = false;
        menu.gameObject.SetActive(false);
        playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Continue()
    {
        globalVolume.TryGet(out dof);
        dof.active = false;
        menu.gameObject.SetActive(false);
        playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
