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


    private void Start()
    {
        cartItems = new Hashtable();
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
    }

    public void RemoveItem(int key)
    {

        cartItems.Remove(key);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CartItem>().key == key)
                Destroy(child.gameObject);
        }
        currentIndent -= indentIncrement;
        GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 30.72f - currentIndent);
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
