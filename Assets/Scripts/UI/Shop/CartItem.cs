using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartItem : MonoBehaviour
{
    public int key;

    public void PlusItem(int key)
    {
        transform.parent.GetComponent<Cart>().AddItem(key);
    }

    public void MinusItem(int key)
    {
        transform.parent.GetComponent<Cart>().DecreaseItem(key);
    }

    public void RemoveItem(int key)
    {
        transform.parent.GetComponent<Cart>().RemoveItem(key);
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
