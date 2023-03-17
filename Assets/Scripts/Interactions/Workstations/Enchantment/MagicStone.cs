using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicStone : MonoBehaviour
{
    [Header("Materials")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] Material materialOn;
    [SerializeField] Material materialOff;
    Renderer rend;

    [Header("How hight stone must be")]
    [SerializeField] private float yRange = 0.3f;

    [Header("Borders")]
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] private float xMax = 8.2f;
    [SerializeField] private float xMin = 5.5f;
    [SerializeField] private float zMax = 5f;
    [SerializeField] private float zMin = 2.6f;
    //if canMove - stone moves down and player can draw
    //else stone flying under the table

    public bool isActivatePoint = false;

    public bool CanMove { get; set; }

    [SerializeField] private Transform magicStoneTransform;
    //public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        // At start, use the first material
        rend.material = materialOff;
    }

    // Update is called once per frame
    void Update()
    {
                   

    }

    public void MoveStone(Vector2 moveStoneCommand)
    {
        float zMouse = moveStoneCommand.y * Time.deltaTime;
        float xMouse = moveStoneCommand.x * Time.deltaTime;

        Vector3 stoneVector = new Vector3(xMouse, 0, zMouse);

        float xResult = magicStoneTransform.position.x + xMouse;
        float zResult = magicStoneTransform.position.z + zMouse;

        if ((xResult > xMin) && (xResult < xMax) && (zResult > zMin) && (zResult < zMax))
        {            
            magicStoneTransform.position += new Vector3(xMouse, 0, zMouse);
        }



        //if (zResult > -zRange && zResult < zRange)
        //{
        //    magicStoneTransform.position += new Vector3(0, 0, zMouse);
        //}


        //if (magicStoneTransform.position.x <= xMin)
        //{
        //    Debug.Log("Min");
        //    Debug.Log(transform.position);
        //    Debug.Log(transform.position.x);
        //    transform.position = new Vector3(xMin, magicStoneTransform.position.y, magicStoneTransform.position.z);
        //    Debug.Log(transform.position);
        //}
        //if (magicStoneTransform.position.x >= xMax)
        //{
        //    Debug.Log("Max");
        //    Debug.Log(transform.position);
        //    Debug.Log(transform.position.x);
        //    transform.position = new Vector3(xMax, magicStoneTransform.position.y, magicStoneTransform.position.z);
        //    Debug.Log(transform.position);
        //}

        //if (magicStoneTransform.position.z < -zRange)
        //{
        //    transform.position = new Vector3(magicStoneTransform.position.x, magicStoneTransform.position.y, -zRange);
        //}
        //if (magicStoneTransform.position.z > zRange)
        //{
        //    transform.position = new Vector3(magicStoneTransform.position.x, magicStoneTransform.position.y, zRange);
        //}
    }

    public void setBlockMove()
    {
        magicStoneTransform.position += new Vector3(0, yRange, 0);
        CanMove = false;

        rend.material = materialOff;

        //Debug.Log("stone is blocked");
    }

    public void setCanMove()
    {
        magicStoneTransform.position -= new Vector3(0, yRange, 0);
        CanMove = true;

        rend.material = materialOn;

        //Debug.Log("can move stone");
    }

    public void moveToDefoultPosision(Transform stoneDefaultPosition)
    {
        magicStoneTransform.position = stoneDefaultPosition.position;
    }

}
