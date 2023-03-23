using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{

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
