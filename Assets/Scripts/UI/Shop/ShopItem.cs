using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public enum ShopItemType
    {
        Copper,
        Iron,
        Tin,
        Coal
    };

    public ShopItemType shopItemType;
    public int price;
    public int amount;
    public int currentAmount;

    private TextMeshProUGUI TMPAmount;
    private TextMeshProUGUI TMPPrice;

    public i_GameEvent itemAddedToTotal;
    public si_GameEvent itemAddedToShoppingCart;

    public si_GameEvent itemRemovedFromShoppingList;

    void RefreshStock()
    {

    }

    private void Start()
    {
        TMPAmount = this.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        TMPPrice = this.transform.Find("Price").GetComponent<TextMeshProUGUI>();
        InitItem();
    }

    void InitItem()
    {
        //string tmp = amount.ToString() + "/" + currentAmount.ToString();
        TMPAmount.text = currentAmount.ToString() + "/" + amount.ToString();
        TMPPrice.text = price.ToString();
    }

    void RefreshItem()
    {
        TMPAmount.text = currentAmount.ToString() + "/" + amount.ToString();
    }

    public void SendItemInfo(bool state)
    {
        if (state)
        {
            if (currentAmount == 0)
                return;
            currentAmount--;
            itemAddedToShoppingCart.Raise(this.GetComponent<ShopItem>());
            itemAddedToTotal.Raise(price);
        }
        else
        {
            if (currentAmount == amount)
                return;
            currentAmount++;
            itemRemovedFromShoppingList.Raise(this.GetComponent<ShopItem>());
            itemAddedToTotal.Raise(price * (-1));
        }

        RefreshItem();
    }
}

