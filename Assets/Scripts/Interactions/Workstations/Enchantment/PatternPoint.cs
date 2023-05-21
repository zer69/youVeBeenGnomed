using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPoint : MonoBehaviour
{
    [Header("Materials")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] Material materialOn;
    //[SerializeField] float duration = 1.0f;
    Renderer rend;
    [SerializeField] private Logic logic;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private int pointId;

    public int PointId { get => pointId; set { pointId = value; } }

    public bool isUsed = false;
    public bool IsUsed
    {
        get => isUsed;
        set
        {
            if(value != isUsed)
            {
                isUsed = value;
                // ping-pong between the materials over the duration
                //float lerp = Mathf.PingPong(Time.time, duration) / duration;
                
                if (value)
                {
                    rend.enabled = true;
                    //int id = gameObject.GetComponent<PatternPoint>().PointId;
                    //logic.activatePoint(pointId) ;
                }
                else
                {
                    rend.enabled = false;
                }
            }
        }
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materialOn;
        // At start, use the first material
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnOn()
    {
        IsUsed = true;
        Debug.Log("point: " + pointId + " - on");        

    }

    public void turnOff()
    {
       // Debug.Log("point: " + pointId + " - off");
        IsUsed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter, other.gameObject.tag: " + other.gameObject.tag);
        //when magic stone touches points it switch point status (is used or not)
        if (other.gameObject.tag == "MagicStone") {

            if (!isUsed)
            {
                turnOn();
                
            }

            logic.activatePoint(pointId);
            
        }
    }

}
