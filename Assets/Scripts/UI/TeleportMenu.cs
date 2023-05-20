using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TeleportMenu : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]

    [SerializeField] private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitTeleport()
    {
        LockMovement();
    }

    private void LockMovement()
    {
        playerInput.DeactivateInput();
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnlockMovement()
    {
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
    }



    


}
