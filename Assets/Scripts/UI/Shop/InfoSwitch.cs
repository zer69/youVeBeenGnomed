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
    

    public void RefreshStockOnOrder()
    {
        foreach (Transform itemInfo in transform)
        {
            itemInfo.GetComponent<ItemPrice>().dayQuantity =
            itemInfo.GetComponent<ItemPrice>().currentQuantity;
        }
    }

    public void RefreshStock()
    {
        foreach (Transform itemInfo in transform)
        {
            
            itemInfo.GetComponent<ItemPrice>().currentQuantity =
                itemInfo.GetComponent<ItemPrice>().dayQuantity =
                itemInfo.GetComponent<ItemPrice>().maxQuantity;
        }
    }
}
