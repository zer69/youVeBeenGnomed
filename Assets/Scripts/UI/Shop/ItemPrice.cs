using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPrice : MonoBehaviour
{
    public GameStateManager reputationLevel;
    private float priceModifier;

    [SerializeField] private int basePrice;
    public int price;
    private TMP_Text priceText;
    private TMP_Text quantityText;
    private Button addButton;
    private string parentName;
    

    public int maxQuantity;
    public int dayQuantity;
    public int currentQuantity;
    
    private void Start()
    {
        parentName = transform.parent.gameObject.name;
        if (parentName == "Info")
        {
            quantityText = transform.Find("InStock").Find("Quantity").GetComponent<TMP_Text>();
            addButton = transform.Find("Add").GetComponent<Button>();
        }
            
        priceText = transform.Find("Price").GetComponent<TMP_Text>();
        
    }

    public void ResetShopWithDay()
    {
        currentQuantity = dayQuantity = maxQuantity;
    }

    public void ResetShopInDay()
    {
        dayQuantity = currentQuantity;
    }

    private void Update()
    {
        GetPriceModifier();
        SetPrice();
        if (parentName == "Info")
        {
            UpdateQuantity();
            UpdateAdd();
        }
           
    }

    void UpdateAdd()
    {
        addButton.interactable = currentQuantity > 0 ? true : false;
    }

    void GetPriceModifier()
    {
        priceModifier = 1f;
    }

   void SetPrice()
    {
        price = (int)(basePrice * priceModifier);
        priceText.text = price.ToString();
    }

    public void UpdateQuantity()
    {
        quantityText.text = currentQuantity.ToString();
    }

    public void ChangeQuantity(bool increase)
    {
        if (increase)
        {
            currentQuantity++;
            currentQuantity = Mathf.Clamp(currentQuantity, 0, dayQuantity);
        }
        else
        {
            currentQuantity--;
            currentQuantity = Mathf.Clamp(currentQuantity, 0, dayQuantity);
        }
    }
}
