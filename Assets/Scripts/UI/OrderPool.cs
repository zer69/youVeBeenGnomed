using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPool : MonoBehaviour
{
    public List<Order> orderList = new List<Order>();

    float currentIndent = 0f;
    float indent = 38.4f + 8f;

    [SerializeField] private GameObject orderPrefab;

    private void Update()
    {
        SetHeight();
        SetChildren();
    }

    private void SetHeight()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, indent * transform.childCount);
    }

    public void SetOrderList(List<Order> newOrderList)
    {
        orderList = newOrderList;
        
    }

    private void SetChildren()
    {
        
        while (transform.childCount < orderList.Count)
        {
            int childCounttmp = transform.childCount;
            GameObject tmpOrder = Instantiate(orderPrefab, transform);
            tmpOrder.transform.localPosition = new Vector3(0f, -(indent * childCounttmp), 0f);
            tmpOrder.GetComponent<OrderMono>().order = orderList[tmpOrder.transform.GetSiblingIndex()];
            tmpOrder.GetComponent<OrderMono>().GetOrderInfo(orderList[tmpOrder.transform.GetSiblingIndex()]);
        }
    }

}
