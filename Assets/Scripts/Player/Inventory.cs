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

    private int pickableLayer = 10;
    private int inHandsLayer = 15;

    [Header("Sound Events")]
    public AK.Wwise.Event IngotPikedSoundEvent;
    public AK.Wwise.Event ToolPikedSoundEvent;
    public AK.Wwise.Event IngotDropedSoundEvent;
    public AK.Wwise.Event ToolDropedSoundEvent;
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
            ingotInThongs = currentObject;
            ingotInThongs.transform.position = thongsPosition.position;
            ingotInThongs.transform.rotation = thongsPosition.rotation;
            ingotInThongs.transform.SetParent(thongsPosition);

            ingotInThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ingotInThongs.GetComponent<BoxCollider>().enabled = false;
            SwitchToInHandsLayer(ingotInThongs.transform);

            hasIngotInThongs = true;
        }

        else if (ingotPicked)
        {
            ingot = currentObject;
            ingot.transform.position = rightHandPosition.position;
            ingot.transform.rotation = rightHandPosition.rotation;
            ingot.transform.SetParent(playerTransform);

            ingot.layer = inHandsLayer;
            ingot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ingot.GetComponent<BoxCollider>().enabled = false;
            SwitchToInHandsLayer(ingot.transform);

            rightHandFree = false;
            hasIngot = true;

            
        }

        else if (ingot != null)
        {
            ingot.GetComponent<BoxCollider>().enabled = true;
            SwitchToPickableLayer(ingot.transform);

            ingot = null;
            rightHandFree = true;
            hasIngot = false;
            IngotDropedSoundEvent.Post(gameObject);
        }
        else if (ingotInThongs != null)
        {
            SwitchToPickableLayer(ingotInThongs.transform);

            ingotInThongs = null;
            hasIngotInThongs = false;
            IngotDropedSoundEvent.Post(gameObject);
        }

            return true;
    }

    public bool ThongsIsPicked(bool thongsPicked)
    {
        if (thongsPicked)
        {
            thongs = currentObject;
            thongs.transform.position = leftHandPosition.position;
            thongs.transform.rotation = leftHandPosition.rotation;
            thongs.transform.SetParent(playerTransform);

            SwitchToInHandsLayer(thongs.transform);

            thongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            hasThongs = true;
        }

        else if (thongs != null)
        {
            SwitchToPickableLayer(thongs.transform);

            if (hasIngotInThongs)
            {
                ingotInThongs.GetComponent<BoxCollider>().enabled = true;
                ingotInThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                ingotInThongs.transform.SetParent(null);
                ingotInThongs = null;
                hasIngotInThongs = false;
            }
            thongs = null;
            hasThongs = false;
            ToolDropedSoundEvent.Post(gameObject);
        }

        return true;
    }

    public bool CoalIsPicked(bool coalPicked)
    {
        if (coalPicked)
        {
            coal = currentObject;
            coal.transform.position = rightHandPosition.position;
            coal.transform.rotation = rightHandPosition.rotation;
            coal.transform.SetParent(playerTransform);

            coal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            coal.GetComponent<BoxCollider>().enabled = false;
            SwitchToInHandsLayer(coal.transform);

            rightHandFree = false;
            hasCoal = true;

            Debug.Log("Coal Picked");
        }

        else if (coal != null)
        {
            coal.GetComponent<BoxCollider>().enabled = true;
            SwitchToPickableLayer(coal.transform);

            coal = null;
            rightHandFree = true;
            hasCoal = false;
        }

        return true;
    }

    public bool BatteryIsPicked(bool batteryPicked)
    {
        if (batteryPicked)
        {
            battery = currentObject;
            battery.transform.position = rightHandPosition.position;
            battery.transform.rotation = rightHandPosition.rotation;
            battery.transform.SetParent(playerTransform);

            battery.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            battery.GetComponent<BoxCollider>().enabled = false;
            SwitchToInHandsLayer(battery.transform);

            rightHandFree = false;
            hasBattery = true;

            Debug.Log("Battery Picked");
        }

        else if (battery != null)
        {
            battery.GetComponent<BoxCollider>().enabled = true;
            SwitchToPickableLayer(battery.transform);

            battery = null;
            rightHandFree = true;
            hasBattery = false;
        }

        return true;
    }

    public bool HammerIsPicked(bool hammerPicked)
    {
        if (hammerPicked)
        {
            hammer = currentObject;
            hammer.transform.position = rightHandPosition.position;
            hammer.transform.rotation = rightHandPosition.rotation;
            hammer.transform.SetParent(playerTransform);

            SwitchToInHandsLayer(hammer.transform);

            hammer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            rightHandFree = false;
            hasHammer = true;
        }

        else if (hammer != null)
        {
            SwitchToPickableLayer(hammer.transform);

            hammer = null;
            
            rightHandFree = true;
            hasHammer = false;
            ToolDropedSoundEvent.Post(gameObject);
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
            case "Ingot":
                if ((hasIngot == false && rightHandFree) || (hasIngotInThongs == false && hasThongs == true))
                {
                    IngotIsPicked(true);

                    IngotPikedSoundEvent.Post(gameObject);
                }
                else
                {
                    Debug.Log("Both hands are busy");
                }
                break;
            case "Hammer":
                if (rightHandFree == true)
                {
                    HammerIsPicked(true);
                    ToolPikedSoundEvent.Post(gameObject);
                }
                else
                {
                    Debug.Log("Your right hand is busy");
                }
                break;
            case "Thongs":
                if (leftHandFree == true)
                {
                    ThongsIsPicked(true);
                    ToolPikedSoundEvent.Post(gameObject);
                }
                else
                {
                    Debug.Log("Your left hand is busy");
                }
                break;
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

    void SwitchToInHandsLayer(Transform objectInHand)
    {
        objectInHand.gameObject.layer = inHandsLayer;
        foreach (Transform child in objectInHand)
        {
            child.gameObject.layer = inHandsLayer;
            foreach (Transform grandchild in child.transform)
            {
                grandchild.gameObject.layer = inHandsLayer;
            }
        }
    }

    void SwitchToPickableLayer(Transform objectInHand)
    {
        objectInHand.gameObject.layer = pickableLayer;
        foreach (Transform child in objectInHand)
        {
            child.gameObject.layer = pickableLayer;
            foreach (Transform grandchild in child.transform)
            {
                grandchild.gameObject.layer = pickableLayer;
            }
        }
    }
}
        
    

