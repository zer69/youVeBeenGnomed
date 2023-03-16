using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnchantmentTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponStartingPosition;
    [SerializeField] private Transform stoneDefaultPosition;
    [SerializeField] private EnchantmentPattern enchantmentPattern;

    private Transform weapon;

    
    [SerializeField] private MagicStone magicStone;


    private Vector2 moveStoneCommand = Vector2.zero;

    private bool canEnchante = false;

    

    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }
    
    void Update()
    {
        if (moveStoneCommand != Vector2.zero)
        {
            magicStone.MoveStone(moveStoneCommand);
            //Debug.Log("move stone");
        }
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

                enchantmentPattern.turnOffPoints();

                //first, block movement 
                magicStone.setBlockMove();
                //second, move stone to default position 
                magicStone.moveToDefoultPosision(stoneDefaultPosition);
                

                break;

            case "DrawMagicRune":

                if (canEnchante)
                {
                    magicStone.CanMove = !magicStone.CanMove;                                       

                    if (magicStone.CanMove)
                    {
                        magicStone.setCanMove();
                    }
                    else
                    {
                        magicStone.setBlockMove();
                    }
                }
                break;

            case "MoveMagicStone":

                if (magicStone.CanMove)
                {
                    moveStoneCommand = context.action.ReadValue<Vector2>();
                }
                break;

        }
    }



}
