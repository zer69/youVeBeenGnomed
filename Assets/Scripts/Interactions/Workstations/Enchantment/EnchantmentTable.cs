using Assets.Scripts.Interactions.Workstations.Enchantment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnchantmentTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private s_GameEvent hint;

    [Header("Sound Events")]
    public AK.Wwise.Event DoneSoundEvent;
    public AK.Wwise.Event ResetSoundEvent;
    public AK.Wwise.Event ErrorSoundEvent;
    public AK.Wwise.Event MagicStoneOnSoundEvent;
    public AK.Wwise.Event MagicStoneOffSoundEvent;
    public AK.Wwise.Event PutWeaponSoundEvent;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform enchantmentStartingPosition;
    [SerializeField] private EnchantmentPattern enchantmentPattern;
    [SerializeField] private Transform stoneDefaultPosition;
    private Transform weapon;

    
    [SerializeField] private MagicStone magicStone;
    [SerializeField] private EnergyReceiver EnergyReceiver;
    private int enchantmentQuality = 1;

    private Vector2 moveStoneCommand = Vector2.zero;

    private bool canEnchante = false;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("stone returning speed")]
    [SerializeField] private float speedHoriz;


    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    void Update()
    {
        if (canEnchante)
        {
            if (moveStoneCommand != Vector2.zero)
            {
                if (enchantmentPattern.checkEnchantment())
                {
                    makeEnchantment(enchantmentPattern.getRuneId());

                    enchantmentPattern.resetRunes();
                    magicStone.setBlockMove();
                    magicStone.startAutoMove(stoneDefaultPosition.position, speedHoriz);

                    moveStoneCommand = Vector2.zero;

                    DoneSoundEvent.Post(gameObject);
                }
                else
                {
                    magicStone.MoveStone(moveStoneCommand);
                    //Debug.Log("Update - move stone");
                }
                
            }
        }
        
    }

    private void makeEnchantment(int enchantmentId)
    {
        Ingot weaponObj = weapon.GetComponent<Ingot>();

        switch (weaponObj.getEnchantment())
        {
            case Ingot.Enchantment.None:
                Debug.Log("set enchantment id: " + enchantmentId);
                weapon.GetComponent<Ingot>().setEnchantment(enchantmentId, enchantmentQuality);
                EnergyReceiver.destroyBattery();
                break;

            case Ingot.Enchantment.Water:
                //6 is dark
                //dark + water = poison
                if (enchantmentId == 6)
                {
                    Debug.Log("set enchantment Poison");
                    weapon.GetComponent<Ingot>().setEnchantment(Ingot.Enchantment.Poison, enchantmentQuality);
                }
                else
                {
                    Debug.Log("enchantment failed");
                    resetPattern();
                    ErrorSoundEvent.Post(gameObject);
                    EnergyReceiver.destroyBattery();
                }
                EnergyReceiver.destroyBattery();
                break;

            case Ingot.Enchantment.Dark:
                //2 is water
                //dark + water = poison
                if (enchantmentId == 2)
                {
                    Debug.Log("set enchantment Poison");
                    weapon.GetComponent<Ingot>().setEnchantment(Ingot.Enchantment.Poison, enchantmentQuality);
                }
                else
                {
                    Debug.Log("enchantment failed");
                    resetPattern();
                    ErrorSoundEvent.Post(gameObject);
                    EnergyReceiver.destroyBattery();
                }
                EnergyReceiver.destroyBattery();
                break;

            case Ingot.Enchantment.Fire:
                //5 is light
                //light + fire = lightning
                if (enchantmentId == 5)
                {
                    Debug.Log("set enchantment Lightning");
                    weapon.GetComponent<Ingot>().setEnchantment(Ingot.Enchantment.Lightning, enchantmentQuality);
                }
                else
                {
                    Debug.Log("enchantment failed");
                    resetPattern();
                    ErrorSoundEvent.Post(gameObject);
                    EnergyReceiver.destroyBattery();
                }
                EnergyReceiver.destroyBattery();
                break;

            case Ingot.Enchantment.Light:
                //1 is fire
                //light + fire = lightning
                if (enchantmentId == 1)
                {
                    Debug.Log("set enchantment Lightning");
                    weapon.GetComponent<Ingot>().setEnchantment(Ingot.Enchantment.Lightning, enchantmentQuality);
                }
                else
                {
                    Debug.Log("enchantment failed");
                    resetPattern();
                    ErrorSoundEvent.Post(gameObject);
                    EnergyReceiver.destroyBattery();
                }
                EnergyReceiver.destroyBattery();
                break;

            default:
                Debug.Log("enchantment failed");
                EnergyReceiver.destroyBattery();
                break;
        }
    }

    public bool Interact(Interactor interactor)
    {
        Inventory inventory = playerTransform.GetComponentInParent<Inventory>();
        //add chek if it's weapon not just ingot
        if (EnergyReceiver.hasBattery)
        {
            if (inventory.hasIngot)
            {
                PutWeaponSoundEvent.Post(gameObject);
                Rigidbody weaponRB = playerTransform.GetComponentInChildren<Rigidbody>();

                weaponRB.transform.rotation = Quaternion.identity;
                weaponRB.transform.position = enchantmentStartingPosition.position;
                weaponRB.transform.SetParent(enchantmentStartingPosition);

                weaponRB.transform.Rotate(-90, 0, 0);

                weapon = weaponRB.transform;

                cam.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);

                enchantmentQuality = EnergyReceiver.enchantmentQuality;

                canEnchante = true;
                Debug.Log("Enchantment Table is used");
                             

                playerInput.actions.FindAction("DropItems").Disable();
                playerInput.actions.FindAction("Use").Disable();
                playerInput.actions.FindAction("Build").Disable();
                return true;
            }
            hint.Raise("Hey, bring the weapon you want to enchant");
        }
        hint.Raise("No energy");
        return false;
    }


    private void resetPattern()
    {
        //Debug.Log("reset: canMove: " + magicStone.CanMove);
        if (magicStone.CanMove) { 
            magicStone.setBlockMove();
        }
        
        magicStone.startAutoMove(stoneDefaultPosition.position, speedHoriz);

        enchantmentPattern.turnOffPoints();
        enchantmentPattern.turnOffLines();
        enchantmentPattern.resetRunes();
        enchantmentPattern.resetLogic();

        enchantmentQuality = 0;

    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        if (canEnchante)
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

                    canEnchante = false;


                    resetPattern();

                    playerInput.actions.FindAction("DropItems").Enable();
                    playerInput.actions.FindAction("Use").Enable();
                    playerInput.actions.FindAction("Build").Enable();

                    break;

                case "DrawMagicRune":
                    //Debug.Log("try move stone");

                    if (!magicStone.IsAutoMoving && EnergyReceiver.hasBattery)
                    {
                        magicStone.CanMove = !magicStone.CanMove;
                        if (magicStone.CanMove)
                        {
                            magicStone.setCanMove();
                            magicStone.stoneDown();
                            MagicStoneOnSoundEvent.Post(gameObject);
                            //break;
                        }
                        else
                        {
                            magicStone.setBlockMove();
                            magicStone.stoneUp();
                            MagicStoneOffSoundEvent.Post(gameObject);
                        }
                    }else if (!EnergyReceiver.hasBattery)
                    {
                        hint.Raise("No energy");
                    }
                    break;

                case "MoveMagicStone":

                    if (magicStone.CanMove && !magicStone.IsAutoMoving)
                    {
                        //Debug.Log("moving stone");
                        moveStoneCommand = context.action.ReadValue<Vector2>();
                    }
                    break;


                case "ResetDrawingRune":

                    //Debug.Log("resetPattern");
                    resetPattern();
                    ResetSoundEvent.Post(gameObject);
                    break;
            }
        }
    }



}
