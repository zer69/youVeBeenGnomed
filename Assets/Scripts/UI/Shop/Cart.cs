using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cart : MonoBehaviour
{
    [SerializeField] private Hashtable cartItems;

    [SerializeField] private List<GameObject> cartItemPrefabs;
    [SerializeField] private float currentIndent = 0;
    private float indentIncrement = -38.72f;

    [SerializeField] private TMP_Text totalOrder;
    [SerializeField] private TMP_Text totalNow;

    public Transform stock;
    [SerializeField] private List<int> itemAmounts;
    [SerializeField] private List<ItemPrice> itemPrices;

    [SerializeField] private ShopOrderManager shopOrderManager;


    private void Start()
    {
        cartItems = new Hashtable();
        
    }

    private void Update()
    {
        CalculateTotal();
    }

    public void SendOrder(int days)
    {
        if (CartEmpty())
            return;
        shopOrderManager.RecieveOrder(itemAmounts, days);
        PrintList(itemAmounts);
        ClearAmounts();
    }

    private void PrintList(List<int> amounts)
    {
        string outputStr = "";
        foreach (int amount in amounts)
            outputStr += " " + amount.ToString();
        Debug.Log(outputStr);
    }

    private bool CartEmpty()
    {
        foreach (int item in itemAmounts)
        {
            if (item > 0)
                return false;
        }
        return true;
    }

    private void ClearAmounts()
    {
        for (int i = 0; i < itemAmounts.Count; i++)
        {
            itemAmounts[i] = 0;
            RemoveItem(i);
        }
            
    }

    private void CalculateTotal()
    {
        int tmpTotal = 0;
        for (int i = 0; i < itemAmounts.Count; i++)
            tmpTotal += itemAmounts[i] * itemPrices[i].price;
        totalOrder.text = tmpTotal.ToString();
        totalNow.text = (tmpTotal * 1.5f).ToString();
    }

    public void AddItem(int key)
    {
        
        if (cartItems.ContainsKey(key))
        {
            cartItems[key] = int.Parse(cartItems[key].ToString()) + 1;
            
            TMP_Text itemQuantity = transform.Find(cartItemPrefabs[key].name + "(Clone)").Find("Quantity").GetComponent<TMP_Text>();
            itemQuantity.text = cartItems[key].ToString();
        }
        else
        {
            cartItems.Add(key, 1);
            RenderItem(key);
            RectTransform thisRect = GetComponent<RectTransform>();
            thisRect.sizeDelta = new Vector2(200f, 30.72f - currentIndent);
        }
        itemAmounts[key]++;


    }

    public void DecreaseItem(int key)
    {
        cartItems[key] = int.Parse(cartItems[key].ToString()) - 1;
        if (int.Parse(cartItems[key].ToString()) == 0)
        {
            RemoveItem(key);
            return;
        }

        TMP_Text itemQuantity = transform.Find(cartItemPrefabs[key].name + "(Clone)").Find("Quantity").GetComponent<TMP_Text>();
        itemQuantity.text = cartItems[key].ToString();
        itemAmounts[key]--;
    }

    public void RemoveItem(int key)
    {
        if (!cartItems.Contains(key))
            return;
        cartItems.Remove(key);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CartItem>().key == key)
                Destroy(child.gameObject);
        }
        currentIndent -= indentIncrement;
        GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 30.72f - currentIndent);
        itemAmounts[key] = 0;
    }


    public void ResetCart()
    {
        cartItems.Clear();
    }

    private void RenderItem(int key)
    {
        GameObject newItem = Instantiate(cartItemPrefabs[key], transform);
        newItem.transform.Translate(new Vector3(0f, currentIndent, 0f));
        currentIndent += indentIncrement;
    }



}
