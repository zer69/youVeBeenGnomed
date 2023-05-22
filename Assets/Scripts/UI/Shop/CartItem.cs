using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartItem : MonoBehaviour
{
    public int key;
    [SerializeField] private Transform infoContainer;
    
    [SerializeField] private b_GameEvent itemInfoUpdate;

    private void Start()
    {
        infoContainer = transform.parent.parent.parent.parent.parent.parent.GetChild(1);
    }

    public void PlusItem(int key)
    {
        if (infoContainer.GetChild(key).GetComponent<ItemPrice>().currentQuantity > 0)
        {
            transform.parent.GetComponent<Cart>().AddItem(key);
            itemInfoUpdate.Raise(false);
        }
        
    }

    public void MinusItem(int key)
    {
        if (infoContainer.GetChild(key).GetComponent<ItemPrice>().currentQuantity < infoContainer.GetChild(key).GetComponent<ItemPrice>().dayQuantity)
        {
            transform.parent.GetComponent<Cart>().DecreaseItem(key);
            itemInfoUpdate.Raise(true);
        }
            
    }

    public void RemoveItem(int key)
    {
        transform.parent.GetComponent<Cart>().RemoveItem(key);
        for (int i = 0; i < 10; i++)
            itemInfoUpdate.Raise(true);
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {

        transform.localPosition = new Vector3(transform.localPosition.x, 0f + (-38.72f * transform.GetSiblingIndex()), transform.localPosition.z);

            
    }
}
