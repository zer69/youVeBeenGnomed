using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnchantmentTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponStartingPosition;
    [SerializeField] private EnchantmentPattern enchantmentPattern;
    [SerializeField] private Transform stoneDefaultPosition;
    private Transform weapon;

    
    [SerializeField] private MagicStone magicStone;

    private float enchantmentQuality = 0.5f;

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
                    makeEnchantment(enchantmentPattern.getRune());

                    enchantmentPattern.resetRunes();
                    magicStone.setBlockMove();
                    magicStone.startAutoMove(stoneDefaultPosition.position, speedHoriz);

                    moveStoneCommand = Vector2.zero;
                }
                else
                {
                    magicStone.MoveStone(moveStoneCommand);
                    //Debug.Log("move stone");
                }
                
            }
        }
        
    }

    private void makeEnchantment(Rune rune)
    {
        
        weapon.GetComponent<Ingot>().setEnchantment(rune.enchantmentId, enchantmentQuality);
    }

    public bool Interact(Interactor interactor)
    {
        Rigidbody weaponRB = playerTransform.GetComponentInChildren<Rigidbody>();

        weaponRB.transform.rotation = Quaternion.identity;
        weaponRB.transform.position = weaponStartingPosition.position;
        weaponRB.transform.SetParent(weaponStartingPosition);

        weaponRB.transform.Rotate(180, 0, 0);

        weapon = weaponRB.transform;

        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);

        canEnchante = true;
        Debug.Log("Enchantment Table is used");


        return true;
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

        enchantmentQuality = 1f;

    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":

                Rigidbody weaponRB = weapon.GetComponent<Rigidbody>();
                weaponRB.transform.rotation = Quaternion.identity;
                weaponRB.transform.position = cam.transform.Find("Right Hand").position;
                weaponRB.transform.SetParent(playerTransform);

                cam.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);
                              
                canEnchante = false;


                resetPattern();

                break;

            case "DrawMagicRune":

                if (canEnchante && !magicStone.IsAutoMoving)
                {
                    magicStone.CanMove = !magicStone.CanMove;
                    if (magicStone.CanMove)
                    {
                        magicStone.setCanMove();
                        magicStone.stoneDown();
                        break;
                    }
                    else
                    {
                        magicStone.setBlockMove();
                        magicStone.stoneUp();
                        
                    }
                }
                break;

            case "MoveMagicStone":

                if (magicStone.CanMove && !magicStone.IsAutoMoving)
                {
                    moveStoneCommand = context.action.ReadValue<Vector2>();
                }
                break;


            case "ResetDrawingRune":

                resetPattern();

                break;
        }
    }



}
