using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderMono : MonoBehaviour
{
    public Order order;

    [SerializeField] private List<Sprite> weaponSpriteList;
    // Start is called before the first frame update
    private void Start()
    {
        SetupButtonPresses();
    }

    
    private void SetupButtonPresses()
    {
        GameObject orderReciever = GameObject.Find("OrderInfo");
        GameObject detectionZone = GameObject.Find("DetectionZone");
        transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            orderReciever.GetComponent<OrderReciever>().ReadOrderStats(order, transform.GetSiblingIndex());
        });
        transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            detectionZone.GetComponent<TeleportPlatform>().PreSend();
        });

    }

    public void SetOrderInContent()
    {

    }

    public void GetOrderInfo(Order order)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = weaponSpriteList[(int)order.weaponType - 1];
        transform.GetChild(1).GetComponent<TMP_Text>().text = order.oreType.ToString() + " " + order.weaponType.ToString();
        transform.GetChild(2).GetComponent<TMP_Text>().text = "Days left: " + order.daysToExpire.ToString();
        transform.GetChild(3).GetComponent<TMP_Text>().text = order.price.ToString();
        transform.GetChild(4).GetComponent<TMP_Text>().text = order.reputation.ToString();
    }
}
