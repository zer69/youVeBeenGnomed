using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private List<Transform> shopItems;

    public void SwitchInfo(int state)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (i == state)
                shopItems[i].gameObject.SetActive(true);
            else
                shopItems[i].gameObject.SetActive(false);

        }
    }

    public void AddItem()
    {
        foreach (Transform item in this.transform)
        {
            if (item.gameObject.tag == "ShopItem" && item.gameObject.active)
            {
                item.GetComponent<ShopItem>().SendItemInfo();
                break;
            }
                
        }
    }
}
