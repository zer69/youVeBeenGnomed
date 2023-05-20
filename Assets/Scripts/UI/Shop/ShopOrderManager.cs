using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOrderManager : MonoBehaviour
{
    [SerializeField] private List<i_GameEvent> itemEvents;

    [SerializeField]private List<List<int>> ordersOnHold;

    private List<int> deliveryDay;

    public int currentDay = 0;

    private void Start()
    {
        ordersOnHold = new List<List<int>>();
        deliveryDay = new List<int>();
    }

    public void RecieveOrder(List<int> itemAmount, int days)
    {
        ordersOnHold.Add(itemAmount);
        deliveryDay.Add(currentDay + days);
        CheckForDelivery();
    }



    private void CheckForDelivery()
    {
        List<int> recievedOrders = new List<int>();
        for (int i = 0; i < deliveryDay.Count; i++)
        {
            if (deliveryDay[i] == currentDay)
            {
                RecieveShopOrder(ordersOnHold[i]);
            }
                
        }

        for (int i = recievedOrders.Count-1; i >=0 ; i--)
        {
            ordersOnHold.RemoveAt(recievedOrders[i]);
        }

    }

    private void RecieveShopOrder(List<int> itemAmount)
    {
        for (int i = 0; i < itemAmount.Count; i++)
            if (itemAmount[i] > 0)
                itemEvents[i].Raise(itemAmount[i]);
        Debug.Log(itemAmount);
    }

    public void NewDay()
    {
        currentDay += 1;
        CheckForDelivery();
    }
}
