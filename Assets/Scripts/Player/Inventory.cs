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
    public GameObject coal;

    [SerializeField] private int childNumber = 0;
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

            ingot.GetComponent<BoxCollider>().enabled = false;
            ingot.GetComponent<Rigidbody>().isKinematic = true;

            hasIngot = true;
        }

        else if (ingotPicked)
        {
            ingot = playerTransform.transform.GetChild(0).gameObject;

            ingot.GetComponent<BoxCollider>().enabled = false;
            ingot.GetComponent<Rigidbody>().isKinematic = true;

            hasIngot = true;
            //Debug.Log(hasIngot);
        }

        else if (ingot != null)
        {
            ingot.GetComponent<BoxCollider>().enabled = true;
            ingot.GetComponent<Rigidbody>().isKinematic = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().rightHand = true;
            playerTransform.gameObject.GetComponentInParent<CameraClicker>().leftWithIngot = false;

            ingot = null;
            hasIngot = false;
            //Debug.Log(hasIngot);
        }

        return true;
    }

    public bool ThongsIsPicked(bool thongsPicked)
    {
        if (thongsPicked)
        {
            thongs = playerTransform.transform.GetChild(childNumber).gameObject;
            hasThongs = true;

            childNumber += 1;
        }

        else if (thongs != null)
        {
            if (hasIngot)
            {
                ingot.GetComponent<BoxCollider>().enabled = true;
                ingot.GetComponent<Rigidbody>().isKinematic = false;
                ingot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                thongs.transform.GetChild(1).SetParent(null);
            }
            thongs = null;
            hasThongs = false;

            childNumber -= 1;
            //Debug.Log(hasThongs);
        }

        return true;
    }

    public bool CoalIsPicked(bool coalPicked)
    {
        if (coalPicked)
        {
            coal = playerTransform.transform.GetChild(childNumber).gameObject;
            coal.GetComponent<BoxCollider>().enabled = false;
            coal.GetComponent<Rigidbody>().isKinematic = true;

            //playerTransform.GetComponent<CameraClicker>().rightHand = false;
            hasCoal = true;

            childNumber += 1;
            Debug.Log("Coal Picked");
        }

        else if (coal != null)
        {
            coal.GetComponent<BoxCollider>().enabled = true;
            coal.GetComponent<Rigidbody>().isKinematic = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().rightHand = true;

            coal = null;
            hasCoal = false;

            childNumber -= 1;
        }

        return true;
    }
}
