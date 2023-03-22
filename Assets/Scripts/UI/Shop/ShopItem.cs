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

    public void SendItemInfo()
    {
        if (currentAmount == 0)
            return;
        currentAmount--;
        RefreshItem();
        itemAddedToShoppingCart.Raise(this.GetComponent<ShopItem>());
        itemAddedToTotal.Raise(price);
    }
}
