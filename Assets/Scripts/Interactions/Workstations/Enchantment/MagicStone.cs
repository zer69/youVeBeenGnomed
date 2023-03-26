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

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("speed")]
    [SerializeField] private float speedVertic;
    //actual speed
    private float speed;

    private float startTime;
    private float journeyLength;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;
    public bool IsAutoMoving { get; set; }


    //if canMove - stone moves down and player can draw
    //else stone flying under the table      
    public bool CanMove { get; set; }

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("Transform")]
    [SerializeField] private Transform magicStoneTransform;

    void Start()
    {
        IsAutoMoving = false;
        rend = GetComponent<Renderer>();

        // At start, use the first material
        rend.material = materialOff;
    }


    void Update()
    {
        if (IsAutoMoving)
        {            
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            if (journeyLength != 0)
            {
                //Debug.Log("AutoMoving from: " + startPosition + " to: " + endPosition);
                magicStoneTransform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            }

            if (magicStoneTransform.position == endPosition) {
                IsAutoMoving = false;
                //Debug.Log("AutoMoving: " + IsAutoMoving);
            }
        }
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

    }

    public void setBlockMove()
    {      
        
        CanMove = false;
        rend.material = materialOff;

        //Debug.Log("stone is blocked");
    }

    public void stoneUp()
    {        
        startAutoMove(new Vector3(magicStoneTransform.position.x, (magicStoneTransform.position.y + yRange), magicStoneTransform.position.z), speedVertic);
        //magicStoneTransform.position += new Vector3(0, yRange, 0);
    }

    public void stoneDown()
    {
        startAutoMove(new Vector3(magicStoneTransform.position.x, (magicStoneTransform.position.y - yRange), magicStoneTransform.position.z), speedVertic);
        //magicStoneTransform.position -= new Vector3(0, yRange, 0);
    }
    public void setCanMove()
    {        
        CanMove = true;
        rend.material = materialOn;

        //Debug.Log("can move stone");
    }

    public void startAutoMove(Vector3 endPosition, float speed)
    {
        IsAutoMoving = true;
        this.speed = speed;

        startTime = Time.time;


        startPosition = new Vector3(magicStoneTransform.position.x, magicStoneTransform.position.y, magicStoneTransform.position.z);
        this.endPosition = endPosition;

        journeyLength = Vector3.Distance(startPosition, endPosition);

        

    }

}
