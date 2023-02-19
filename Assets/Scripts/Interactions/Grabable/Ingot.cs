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

    enum Enchantment
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
        Poison,
        Lightning,
        Light,
        Dark
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

    [SerializeField] public CompletionStatus status;

    [SerializeField] private Rarity rarity;
    [SerializeField] private OreType type;
    [SerializeField] private float quality;

    [SerializeField] private float MeltingPoint;
    public float currentTemperature;

    [SerializeField] private float minTemperatureValue = 0;

    [SerializeField] private float strength;

    [SerializeField] private float minStrengthValue = 0;
    [SerializeField] private float maxStrengthValue = 100;


    public float fragility;

    [SerializeField] private float minFragilityValue = 0;
    [SerializeField] private float maxFragilityValue = 100;

    public float sharpness;

    [SerializeField] bool isEnchanted;
    [SerializeField] private Enchantment enchantment;
    [SerializeField] private float enchantmentQuality;

   

    private float price; // calculated based on rarity, type, quality, strength, fragility, sharpness and enchantment

    private void Update()
    {
        //Debug.Log("sharpness: " + sharpness);
        
    }

    //method for ingot temperature reduction, rate - how much does the temperature change
    public bool Cooling(float rate)
    {        
        if (currentTemperature - rate >= minTemperatureValue)
        {
            currentTemperature -= rate;
            Debug.Log("currentTemperature: " + currentTemperature);
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

}
