using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IngotMenu : MonoBehaviour
{

    private Transform ingotToSet;
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform ingotMenu;
    [SerializeField] private Transform Buttons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIngot(Transform ingot)
    {
        ingotToSet = ingot;
        LockMovement();
    }

    private void LockMovement()
    {
        playerInput.DeactivateInput();
        Cursor.lockState = CursorLockMode.None;
    }

    private void UnlockMovement()
    {
        Debug.Log(ingotToSet.GetComponent<Ingot>().weaponType);
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
        
        ingotMenu.gameObject.SetActive(false);
    } 

    public void SetAxe()
    {
        ingotToSet.GetComponent<Ingot>().weaponType = Ingot.WeaponType.Axe;
        UnlockMovement();
    }

    public void SetSword()
    {
        ingotToSet.GetComponent<Ingot>().weaponType = Ingot.WeaponType.Sword;
        UnlockMovement();
    }

    public void SetSpear()
    {
        ingotToSet.GetComponent<Ingot>().weaponType = Ingot.WeaponType.Spear;
        UnlockMovement();
    }

    public void SetDagger()
    {
        ingotToSet.GetComponent<Ingot>().weaponType = Ingot.WeaponType.Dagger;
        UnlockMovement();
    }
}
