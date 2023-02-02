using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasCoal = false;
    public bool hasIngot = false;

    [SerializeField] private GameObject playerTransform;
    public GameObject ingot;
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
        if (ingotPicked)
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
}
