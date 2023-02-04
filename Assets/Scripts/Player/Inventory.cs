using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasCoal = false;
    public bool hasIngot = false;
    public bool hasThongs = false;

    [SerializeField] private GameObject playerTransform;
    public GameObject ingot;
    public GameObject thongs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IngotIsPicked(bool ingotPicked)
    {
        if (ingotPicked && hasThongs)
        {
            ingot = thongs.transform.GetChild(1).gameObject;

            hasIngot = true;
            Debug.Log(hasIngot);
        }

        else if (ingotPicked)
        {
            ingot = playerTransform.transform.GetChild(0).gameObject;
            hasIngot = true;
            Debug.Log(hasIngot);
        }

        else
        {
            ingot = null;
            hasIngot = false;
            Debug.Log(hasIngot);
        }

        return true;
    }

    public bool ThongsIsPicked(bool thongsPicked)
    {
        if (thongsPicked)
        {
            thongs = playerTransform.transform.GetChild(0).gameObject;
            hasThongs = true;
            Debug.Log(hasThongs);
        }

        else
        {
            thongs = null;
            hasThongs = false;
            Debug.Log(hasThongs);
        }

        return true;
    }
}
