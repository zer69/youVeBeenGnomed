using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicStone : MonoBehaviour
{
    //[SerializeField] private string _prompt;

    [SerializeField] private Transform magicStoneTransform;
    [SerializeField] private PlayerInput playerInput;

    private Vector2 moveStoneCommand = Vector2.zero;

    [SerializeField] private float yRange = 0.3f;

    [SerializeField] private float xRange = 8.2f;
    [SerializeField] private float zRange = 5.1f;
    //if canMove - stone moves down and player can draw
    //else stone flying under the table
    private bool canMove = false;

    //public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    // Update is called once per frame
    void Update()
    {

            
        if (moveStoneCommand != Vector2.zero)
        {
            MoveStone();
            //Debug.Log("move stone");
        }
    }

    void MoveStone()
    {
        float zMouse = moveStoneCommand.y * Time.deltaTime;
        float xMouse = moveStoneCommand.x * Time.deltaTime;

        Vector3 stoneVector = new Vector3(xMouse, 0, zMouse);
        
        magicStoneTransform.position += stoneVector;

    }

    public void setBlockMove()
    {
        Vector3 stoneVector = new Vector3(0, yRange, 0);
      
        magicStoneTransform.position += stoneVector;
        Debug.Log("stone is blocked");
        canMove = false;      

    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "DrawMagicRune":

                canMove = !canMove;

                Vector3 stoneVector = new Vector3(0, yRange, 0);

                if (canMove)
                {                    
                    magicStoneTransform.position -= stoneVector;
                }
                else
                {
                    magicStoneTransform.position += stoneVector;
                }
                break;

            case "MoveMagicStone":

                if (canMove)
                {
                    moveStoneCommand = context.action.ReadValue<Vector2>();
                }
                break;

            case "Abort":

                if (canMove)
                {
                    setBlockMove();
                }
                break;
        }
    }
}
