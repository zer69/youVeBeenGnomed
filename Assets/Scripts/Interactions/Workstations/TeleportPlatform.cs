using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatform : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    public List<GameObject> colliderList = new List<GameObject>();
    public OrderReciever orderReciever;

    public void AddToList(Collider weapon)
    {
        if (!colliderList.Contains(weapon.gameObject) && weapon.gameObject.tag == "Ingot")
        {
            if (weapon.transform.parent != null)
                return;
            colliderList.Add(weapon.gameObject);
            Debug.Log("Added " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }

    public void RemoveFromList(Collider weapon)
    {
        if (colliderList.Contains(weapon.gameObject))
        {
            colliderList.Remove(weapon.gameObject);
            Debug.Log("Removed " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }

    public void Send()
    {
        colliderList.RemoveAt(0);
    }

    private void Update()
    {
        PreSend();
    }

    public void PreSend()
    {
        if (colliderList.Count == 0)
            orderReciever.RecieveOrder(null);
        else
            orderReciever.RecieveOrder(colliderList[0].GetComponent<Ingot>());
    }
}
