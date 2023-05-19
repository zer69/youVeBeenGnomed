using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private s_GameEvent hint;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform workbenchStartingPosition1;
    [SerializeField] private Transform workbenchStartingPosition2;
    [SerializeField] private Transform workbenchStartingPosition3;

    [SerializeField] private int swordGuard;
    [SerializeField] private int swordHilt;
    [SerializeField] private int spearHandle;
    [SerializeField] private int axeHandle;


    private Transform weapon;
    private Ingot ingot;
    private bool workbenchIsUsed;

    public string InteractionPrompt => _prompt;
    // Start is called before the first frame update
    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Interact(Interactor interactor)
    {
        Inventory inventory = playerTransform.GetComponentInParent<Inventory>();

        //add chek if it's weapon of just ingot

        if (inventory.hasIngot)
        {
            ingot = inventory.ingot.GetComponent<Ingot>();

            if (ingot.status == Ingot.CompletionStatus.Sharpened)
            {
                Rigidbody weaponRB = playerTransform.GetComponentInChildren<Rigidbody>();

                weaponRB.transform.rotation = Quaternion.identity;
                switch (ingot.weaponType)
                {
                    case Ingot.WeaponType.Axe:
                    case Ingot.WeaponType.Spear:
                        weaponRB.transform.position = workbenchStartingPosition1.position;
                        weaponRB.transform.rotation = workbenchStartingPosition1.rotation;
                        weaponRB.transform.SetParent(workbenchStartingPosition1);
                        break;
                    case Ingot.WeaponType.Sword:
                        weaponRB.transform.position = workbenchStartingPosition3.position;
                        weaponRB.transform.rotation = workbenchStartingPosition1.rotation;
                        weaponRB.transform.SetParent(workbenchStartingPosition3);
                        break;
                    case Ingot.WeaponType.Dagger:
                        weaponRB.transform.position = workbenchStartingPosition2.position;
                        weaponRB.transform.rotation = workbenchStartingPosition1.rotation;
                        weaponRB.transform.SetParent(workbenchStartingPosition2);
                        break;
                }


                weaponRB.transform.Rotate(-90, 0, -90);

                weapon = weaponRB.transform;

                cam.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);

                workbenchIsUsed = true;
                Debug.Log("woekbanch is used");

                playerInput.actions.FindAction("DropItems").Disable();

                return true;
            }
            hint.Raise("Hey, blade not ready");
        }

        hint.Raise("Hey, bring the part of weapon you made");
        return false;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        if (workbenchIsUsed)
        {
            switch (context.action.name)
            {
                case "Abort":

                    Rigidbody weaponRB = weapon.GetComponent<Rigidbody>();
                    weaponRB.transform.rotation = Quaternion.identity;
                    weaponRB.transform.position = cam.transform.Find("Right Hand").position;
                    weaponRB.transform.rotation = cam.transform.Find("Right Hand").rotation;
                    weaponRB.transform.SetParent(playerTransform);

                    cam.gameObject.SetActive(true);
                    cam2.gameObject.SetActive(false);

                    playerInput.actions.FindAction("DropItems").Enable();

                    break;

                case "Build":
                    if (ingot.status != Ingot.CompletionStatus.Completed) {
                        ingot.setComponentsActive(true);
                        ingot.status = Ingot.CompletionStatus.Completed;
                        hint.Raise("Done!");
                    }
                    break;
            }
        }
    }

    public void ChangeAmounOfSwordGuard(int amount)
    {
        swordGuard += amount;
    }

    public void ChangeAmounOfSwordHilt(int amount)
    {
        swordHilt += amount;
    }

    public void ChangeAmounOfSpearHandle(int amount)
    {
        spearHandle += amount;
    }

    public void ChangeAmounOfAxeHandle(int amount)
    {
        axeHandle += amount;
    }
}
