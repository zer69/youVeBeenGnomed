using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingot : MonoBehaviour
{
    enum Rarity
    {
        Common,
        Uncommon,
        Rare, 
        Epic, 
        Legendary
    };

    enum OreType
    {
        Iron,
        Steel,
        Copper,
        Bronze
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
        Raw,
        Melted,
        Forged,
        Cooled,
        Sharpened,
        Completed

    };

    public enum WeaponType
    {
        None,
        Axe,
        Sword,
        Spear
    }

    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] public CompletionStatus status;
    [SerializeField] public WeaponType weaponType;

    [SerializeField] private Rarity rarity;
    [SerializeField] private OreType type;
    


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
    [SerializeField] private float maxTemperatureValue = 1200;

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

   

    private float price; // calculated based on rarity, type, quality, strength, fragility, sharpness and enchantment
    private void Start()
    {
        coolingRate = airCoolingRate;
        currentTemperature = airTemperature;

        isEnchanted = false;
        enchantment = Enchantment.None;
        enchantmentQuality = 1f;
    }
    private void Update()
    {
        Cooling();
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
        if (currentTemperature >= MeltingPoint)
        {
            status = CompletionStatus.Melted;
        }
        //else
        //{
        //    status = CompletionStatus.Cooled;
        //}
    }
}
