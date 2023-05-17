using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TotalDeactivator : MonoBehaviour
{
    [SerializeField] private Money money;
    [SerializeField] TMP_Text moneyText;

    [SerializeField] private TMP_Text totalOrder;
    [SerializeField] private TMP_Text totalNow;

    [SerializeField] private Button orderButton;
    [SerializeField] private Button nowButton;

    [SerializeField] private i_GameEvent moneySpent;

    private void Update()
    {
        CheckOrder();
        CheckNow();
        moneyText.text = money.money.ToString();
    }

    private void CheckOrder()
    {
        if ((float)money.money >= float.Parse(totalOrder.text))
            orderButton.interactable = true;
        else
            orderButton.interactable = false;
    }

    private void CheckNow()
    {
        if ((float)money.money >= float.Parse(totalNow.text))
            nowButton.interactable = true;
        else
            nowButton.interactable = false;
    }

    public void SpendMoneyOnOrder()
    {
        moneySpent.Raise(int.Parse(totalOrder.text));
    }

    public void SpendMoneyNow()
    {
        moneySpent.Raise(int.Parse(totalNow.text));
    }
}
