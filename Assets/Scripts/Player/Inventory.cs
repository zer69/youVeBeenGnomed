using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasCoal = false;
    public bool hasIngot = false;
    public bool hasIngotInThongs = false;
    public bool hasThongs = false;
    public bool hasBattery = false;
    public bool hasHammer = false;
    public bool rightHandFree = true;
    public bool leftHandFree = true;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform thongsPosition;
    public GameObject ingot;
    public GameObject ingotInThongs;
    public GameObject thongs;
    public GameObject coal;
    public GameObject battery;
    public GameObject hammer;

    [SerializeField] private GameObject currentObject;
        
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
        if (ingotPicked && hasThongs && ! hasIngotInThongs)
        {
            ingotInThongs = thongs.transform.GetChild(0).GetChild(0).gameObject;

            ingotInThongs.GetComponent<BoxCollider>().enabled = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().leftWithIngot = true;
            //ingot.GetComponent<Rigidbody>().isKinematic = true;

            hasIngotInThongs = true;
            //childNumber += 1;
        }

        else if (ingotPicked)
        {
            ingot = playerTransform.transform.GetChild(childNumber).gameObject;

            ingot.GetComponent<BoxCollider>().enabled = false;
            //ingot.GetComponent<Rigidbody>().isKinematic = true;

            playerTransform.GetComponentInParent<CameraClicker>().rightHand = false;
            rightHandFree = false;
            hasIngot = true;
            childNumber += 1;
        }

        else if (ingot != null)
        {
            ingot.GetComponent<BoxCollider>().enabled = true;
            //ingot.GetComponent<Rigidbody>().isKinematic = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().rightHand = true;
            playerTransform.gameObject.GetComponentInParent<CameraClicker>().leftWithIngot = false;

            ingot = null;
            rightHandFree = true;
            playerTransform.GetComponentInParent<CameraClicker>().rightHand = false;
            hasIngot = false;
            childNumber -= 1;

            //if (ingotInThongs != null)
            //{
            //    ingotInThongs = null;
            //    hasIngotInThongs = false;
            //    //childNumber -= 1;
            //}
        }
        else if (ingotInThongs != null)
        {
            ingotInThongs = null;
            hasIngotInThongs = false;
            //childNumber -= 1;
        }

            return true;
    }

    public bool ThongsIsPicked(bool thongsPicked)
    {
        if (thongsPicked)
        {
            thongs = playerTransform.transform.GetChild(childNumber).gameObject;
            if(thongs.transform.GetChild(0).childCount == 1)
            {
                ingotInThongs = thongs.transform.GetChild(0).GetChild(0).gameObject;
                ingotInThongs.transform.position = thongs.transform.Find("ThongsPosition").position;
                ingotInThongs.transform.rotation = thongs.transform.Find("ThongsPosition").rotation;

                //ingotInThongs.GetComponent<BoxCollider>().enabled = false;

                playerTransform.gameObject.GetComponentInParent<CameraClicker>().leftWithIngot = true;

                hasIngotInThongs = true;
            }
            hasThongs = true;

            childNumber += 1;
        }

        else if (thongs != null)
        {
            if (hasIngotInThongs)
            {
                ingotInThongs.GetComponent<BoxCollider>().enabled = true;
                //ingotInThongs.GetComponent<Rigidbody>().isKinematic = false;
                ingotInThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                //ingotInThongs.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezeRotationX;
                //ingotInThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                //ingotInThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

                ingotInThongs = null;
                hasIngotInThongs = false;

                thongs.transform.GetChild(0).GetChild(0).SetParent(null);
                //childNumber -= 1;
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
            coal = currentObject;
            //coal = playerTransform.transform.GetChild(childNumber).gameObject;
            coal.transform.position = rightHandPosition.position;
            coal.transform.rotation = rightHandPosition.rotation;
            coal.transform.SetParent(playerTransform);

            coal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            coal.GetComponent<BoxCollider>().enabled = false;
            //coal.GetComponent<Rigidbody>().isKinematic = true;

            playerTransform.GetComponentInParent<CameraClicker>().rightHand = false;
            rightHandFree = false;
            hasCoal = true;

            childNumber += 1;
            Debug.Log("Coal Picked");
        }

        else if (coal != null)
        {
            coal.GetComponent<BoxCollider>().enabled = true;
            //coal.GetComponent<Rigidbody>().isKinematic = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().rightHand = true;

            coal = null;
            rightHandFree = true;
            hasCoal = false;

            childNumber -= 1;
        }

        return true;
    }

    public bool BatteryIsPicked(bool batteryPicked)
    {
        if (batteryPicked)
        {
            battery = currentObject;
            //coal = playerTransform.transform.GetChild(childNumber).gameObject;
            battery.transform.position = rightHandPosition.position;
            battery.transform.rotation = rightHandPosition.rotation;
            battery.transform.SetParent(playerTransform);

            battery.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            battery.GetComponent<BoxCollider>().enabled = false;
            //coal.GetComponent<Rigidbody>().isKinematic = true;

            playerTransform.GetComponentInParent<CameraClicker>().rightHand = false;
            rightHandFree = false;
            hasBattery = true;

            childNumber += 1;
            Debug.Log("Battery Picked");
        }

        else if (battery != null)
        {
            battery.GetComponent<BoxCollider>().enabled = true;
            //coal.GetComponent<Rigidbody>().isKinematic = false;

            playerTransform.gameObject.GetComponentInParent<CameraClicker>().rightHand = true;

            battery = null;
            rightHandFree = true;
            hasBattery = false;

            childNumber -= 1;
        }

        return true;
    }

    public bool HammerIsPicked(bool hammerPicked)
    {
        if (hammerPicked)
        {
            hammer = playerTransform.transform.GetChild(childNumber).gameObject;

            rightHandFree = false;
            hasHammer = true;

            childNumber += 1;
        }

        else if (hammer != null)
        {
            hammer = null;

            rightHandFree = true;
            hasHammer = false;

            childNumber -= 1;
        }

        return true;
    }
    
    public bool CheckInventoryForItem(string item)
    {
        switch (item)
        {
            case "Ingot":
                if (hasIngot)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "Thongs":
                if (hasThongs)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "IngotInThongs":
                if (hasIngotInThongs)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "Hammer":
                if (hasHammer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default:
                return false;
        }
    }

    public void SetHasIngotInThongs(bool setTo)
    {
        if (setTo)
        {
            hasIngotInThongs = true;
        } else
        {
            hasIngotInThongs = false;
        }
    }

    public void GrabInteraction(GameObject pickableObject)
    {
        currentObject = pickableObject;

        switch (pickableObject.gameObject.tag)
        {
            //case "Ingot":
            //    InteractWithIngot();
            //    pickableObject = null;
            //    break;
            //case "Tool":
            //    InteractWithTool();
            //    break;
            case "Coal":
                if(rightHandFree == true)
                {
                    CoalIsPicked(true);
                }
                else
                {
                    Debug.Log("Your right hand is busy");
                }
                break;
            case "Battery":
                if (rightHandFree == true)
                {
                    BatteryIsPicked(true);
                }
                else
                {
                    Debug.Log("Your right hand is busy");
                }
                break;
        }
    }
}
        
    

