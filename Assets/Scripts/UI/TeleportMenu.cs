using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TeleportMenu : MonoBehaviour
{
    [SerializeField] private Transform orderMenu;
    [SerializeField] private Transform shopMenu;
    [SerializeField] private Transform recyclingMenu;
    [SerializeField] private Transform blackMarketMenu;
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

    private void UnlockMovement()
    {
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
    }



    public void SwitchMenu(int state)
    {
        orderMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(false);
        recyclingMenu.gameObject.SetActive(false); 
        blackMarketMenu.gameObject.SetActive(false);


        switch (state)
        {
            case 0:
                orderMenu.gameObject.SetActive(true);
                break;
            case 1:
                shopMenu.gameObject.SetActive(true);
                break;
            case 2:
                recyclingMenu.gameObject.SetActive(true);
                break;
            case 3:
                blackMarketMenu.gameObject.SetActive(true);
                break;
        }
    }


}
