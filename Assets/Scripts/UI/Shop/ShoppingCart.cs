using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private List<ShopItem.ShopItemType> shoppingCart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToShoppingCart(ShopItem shopItem)
    {
        shoppingCart.Add(shopItem.shopItemType);
    }

    public void RemoveFromShoppingCart(ShopItem shopItem)
    {
        foreach (ShopItem.ShopItemType item in shoppingCart)
        {
            if (item == shopItem.shopItemType)
            {
                shoppingCart.Remove(item);
                break;
            }
                
        }
    }
}
