using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternLine : MonoBehaviour
{

    [Header("Sound Events")]
    public AK.Wwise.Event PointOnSoundEvent;

    [Header("Materials")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] Material materialOn;
    Renderer rend;
    public bool isDrawn = false;

    private bool IsDrawn
    {
        get => isDrawn;
        set
        {
            if (value != isDrawn)
            {
                isDrawn = value;

                if (value)
                {
                    rend.enabled = true;
                    PointOnSoundEvent.Post(gameObject);
                }
                else
                {
                    rend.enabled = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        // At start, use the first material
        rend.enabled = false;
        rend.material = materialOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnOn()
    {
        Debug.Log("line on");
        IsDrawn = true;

    }

    public void turnOff()
    {
       // Debug.Log("line off");
        IsDrawn = false;
    }
}
