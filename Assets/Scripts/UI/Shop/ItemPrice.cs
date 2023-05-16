using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPrice : MonoBehaviour
{
    private Transform reputation;
    private float priceModifier;

    [SerializeField] private int basePrice;
    public int price;
    private TMP_Text priceText;
    
    private void Start()
    {
        reputation = GameObject.Find("ReputationManager").transform;
        priceText = transform.Find("Price").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        GetPriceModifier();
        SetPrice();
    }

    void GetPriceModifier()
    {
        priceModifier = 1f - (reputation.GetComponent<ReputationTmp>().reputation / reputation.GetComponent<ReputationTmp>().maxRep)/2f;
    }

   void SetPrice()
    {
        price = (int)(basePrice * priceModifier);
        priceText.text = price.ToString();
    }
}
