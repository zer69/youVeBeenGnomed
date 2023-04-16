using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
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
            if (item.gameObject.tag == "ShopItem" && item.gameObject.activeSelf)
            {
                item.GetComponent<ShopItem>().SendItemInfo(true);
                break;
            }
                
        }
    }

    public void SubtractItem()
    {
        foreach (Transform item in this.transform)
        {
            if (item.gameObject.tag == "ShopItem" && item.gameObject.activeSelf)
            {
                item.GetComponent<ShopItem>().SendItemInfo(false);
                break;
            }

        }
    }
}
