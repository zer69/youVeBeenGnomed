using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPoint : MonoBehaviour
{

    public bool isUsed = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //when magic stone touches points it switch point status (is used or not)
        if (other.gameObject.tag == "MagicStone") { 
           
            isUsed = !isUsed;

            if (isUsed)
            {
                Debug.Log("on");
            }
            else Debug.Log("off");
        }
    }

}
