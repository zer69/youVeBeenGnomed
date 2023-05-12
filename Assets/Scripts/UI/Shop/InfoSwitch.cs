using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSwitch : MonoBehaviour
{
    

    public void SwitchPanels(int state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(state).gameObject.SetActive(true);
    }
    
}
