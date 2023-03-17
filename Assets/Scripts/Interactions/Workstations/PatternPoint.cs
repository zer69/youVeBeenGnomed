using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPoint : MonoBehaviour
{
    [Header("Materials")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] Material materialOn;
    [SerializeField] Material materialOff;
    //[SerializeField] float duration = 1.0f;
    Renderer rend;

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
                    turnOn();
                }
                else
                {
                    turnOff();
                }
            }
        }
    }

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

    public void turnOn()
    {
        Debug.Log("on");
        rend.material = materialOn;
        //rend.material.Lerp(materialOff, materialOn, lerp);
    }

    public void turnOff()
    {
        Debug.Log("off");
        //rend.material.Lerp(materialOn, materialOff, lerp);
        rend.material = materialOff;
    }
    private void OnTriggerEnter(Collider other)
    {
        //when magic stone touches points it switch point status (is used or not)
        if (other.gameObject.tag == "MagicStone") {

            IsUsed = !IsUsed;

        }
    }

}
