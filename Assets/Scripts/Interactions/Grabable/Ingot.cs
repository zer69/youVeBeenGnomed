using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingot : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare, 
        Epic, 
        Legendary
    };

    public enum OreType
    {
        Iron,
        Copper,
        Silver
    };

    public enum Enchantment
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark,
        Poison,
        Lightning
    };

    public enum CompletionStatus
    {
        Raw, // status for furnace
        Melted, // status after furnace, anvil next

        Forged, // done on anvil, new base status, next is heated/cooled
        
        Heated, // heat on furnace after forging
        Cooled, // cooled after heating

        Sharpened, // status after whetstone
        Completed // status after workbench

    };

    public enum WeaponType
    {
        None,
        Axe,
        Sword,
        Dagger,
        Spear
    }

    public enum AnvilState
    {
        Raw,
        Rare,
        MediumRare,
        WellDone,
        Weapon
    }

    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] public CompletionStatus status;
    [SerializeField] public WeaponType weaponType;


    [SerializeField] public Rarity rarity;
    [SerializeField] public OreType oreType;
    


    [Header("Air temperature")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] public float airTemperature = 24;
    [SerializeField] public float airCoolingRate = 0.09f;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Ignot properties")]
    [SerializeField] private float quality;

    [SerializeField] private float MeltingPoint;

    [SerializeField] public float coolingRate;
    [SerializeField] public float currentTemperature;
    //minTemperatureValue = airTemperature
    //[SerializeField] private float maxTemperatureValue = 1200;

    [SerializeField] private float strength;

    [SerializeField] private float minStrengthValue = 0;
    [SerializeField] private float maxStrengthValue = 100;

    [SerializeField] public float fragility;

    [SerializeField] private float minFragilityValue = 0;
    [SerializeField] private float maxFragilityValue = 100;

    [SerializeField] public float sharpness;

    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] bool isEnchanted;
    [SerializeField] private Enchantment enchantment;
    [SerializeField] private float enchantmentQuality;

    [SerializeField] private col_GameEvent weaponLanded;

    [SerializeField] private Material ingotMaterial;
    private Color emissiveColor = new Color(0.749f, 0.0078f, 0f, 1f);

    [SerializeField] private GameObject state1;
    [SerializeField] private GameObject state2;
    [SerializeField] public AnvilState anvilState;

    [SerializeField] private go_GameEvent sendIngot;
    [SerializeField] private s_GameEvent hint;
    bool readyRaised = true;

    private float price; // calculated based on rarity, type, quality, strength, fragility, sharpness and enchantment
    private void Start()
       
    {
        ingotMaterial.EnableKeyword("_EMISSION");

        //ingotMaterial = this.gameObject.GetComponent<Material>();
        coolingRate = airCoolingRate;
        //currentTemperature = airTemperature;

        isEnchanted = false;
        enchantment = Enchantment.None;
        enchantmentQuality = 1f;
    }
    private void Update()
    {
        Cooling();
        HeatColor();
        if (anvilState != AnvilState.Weapon)
            UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        switch (anvilState)
        {
            case AnvilState.Rare:
                GetComponent<MeshRenderer>().enabled = false;
                transform.Find("Ingot_2_Iron").gameObject.SetActive(true);
                break;
            case AnvilState.MediumRare:
                transform.Find("Ingot_2_Iron").gameObject.SetActive(false);
                transform.Find("Ingot_3_Iron").gameObject.SetActive(true);
                break;
            case AnvilState.WellDone:
                transform.Find("Ingot_3_Iron").gameObject.SetActive(false);
                UpdateWeaponGraphics();
                break;
        }
    }

    public void UpdateWeaponGraphics()
    {
        //Debug.Log("SENTWEAPON");
        sendIngot.Raise(this.gameObject);
        anvilState = AnvilState.Weapon;


    }

    public void HeatColor()
    {
        //Debug.Log(emissiveColor);
        //Debug.Log(currentTemperature);
        emissiveColor = emissiveColor * ((currentTemperature / MeltingPoint) * 64f);
        ingotMaterial.SetColor("_EmissionColor", emissiveColor);
        ingotMaterial.EnableKeyword("_EMISSION");
    }

    //method for ingot temperature reduction, rate - how much does the temperature change
    public bool Cooling()
    {        
        if (currentTemperature - coolingRate >= airTemperature)
        {
            currentTemperature -= coolingRate;
            //Debug.Log("currentTemperature: " + currentTemperature);

            return true;
        }

        
        if (status > CompletionStatus.Forged)
        {
            status = CompletionStatus.Cooled;
        }

        return false;
    }

    public bool Heating(float furnaceTemperature, float smeltingSpeed)
    {
        if (currentTemperature < furnaceTemperature)
        {
            currentTemperature += smeltingSpeed * Time.deltaTime;
            Melting();
            //Debug.Log("Current temperature of ingot is " + ingotTemperature + "*C");

            if (status >= CompletionStatus.Forged)
            {
                status = CompletionStatus.Heated;
            }
            return true;
        }

        return false;
    }

    public void StrengthModification(float rate)
    {
        //to decrease the value, a negative value must be passed
        if (strength + rate > minStrengthValue && strength + rate < maxStrengthValue)
        {
            strength += rate;
        }
    }

   
    public void FragilityModification(float rate)
    {
        //to decrease the value, a negative value must be passed
        if (fragility + rate > minFragilityValue && strength + rate < maxFragilityValue)
        {
            fragility += rate;
        }
    }

    public void setNormalCoolingRate()
    {
        coolingRate = airCoolingRate;
        Debug.Log("coolingRate: " + airCoolingRate);
    }

    public void setZeroCoolingRate()
    {
        coolingRate = 0;
        Debug.Log("coolingRate: ZeroCoolingRate");
    }

    public void setSpecificCoolingRate(float rate)
    {
        coolingRate = rate;
        Debug.Log("coolingRate: " + rate);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TeleportPlatform")
            weaponLanded.Raise(this.GetComponent<BoxCollider>());
    }

    public void setEnchantment(int enchantmentId, float quality)
    {        
        isEnchanted = true;
        enchantment = (Enchantment)Enum.GetValues(typeof(Enchantment)).GetValue(enchantmentId); ;
        enchantmentQuality = quality;
    }

    public void setEnchantment(Enchantment enchantment, float quality)
    {
        isEnchanted = true;
        this.enchantment = enchantment;
        enchantmentQuality = quality;
    }

    public Enchantment getEnchantment()
    {
        return enchantment;
    }

    public void Melting()
    {
        if (anvilState == AnvilState.Weapon)
            return;
        if (currentTemperature >= MeltingPoint)
        {
            status = CompletionStatus.Melted;
            if (readyRaised)
            {
                hint.Raise("Ingot ready for forging!");
                readyRaised = false;
            }
        }
        else
        {
            status = CompletionStatus.Raw;
        }
    }

    public ValueTuple<CompletionStatus, WeaponType, Rarity, OreType> getData()
    {
        return (status, weaponType, rarity, oreType);
    }

    public void setData(ValueTuple<CompletionStatus, WeaponType, Rarity, OreType> data)
    {
        (status, weaponType, rarity, oreType) = data;
    }
}
