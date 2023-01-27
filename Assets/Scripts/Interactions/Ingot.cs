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

    enum CompletionStatus
    {
        Raw,
        Melted,
        Forged,
        Cooled,
        Sharpened,
        Completed

    };

    [SerializeField] private CompletionStatus status;

    [SerializeField] private Rarity rarity;
    [SerializeField] private OreType type;
    [SerializeField] private float quality;

    [SerializeField] private float MeltingPoint;
    [SerializeField] private float currentTemperature;

    [SerializeField] private float strength;
    public float fragility;

    public float sharpness;

    [SerializeField] bool isEnchanted;
    [SerializeField] private Enchantment enchantment;
    [SerializeField] private float enchantmentQuality;

   

    private float price; // calculated based on rarity, type, quality, strength, fragility, sharpness and enchantment

    private void Update()
    {
        //Debug.Log(sharpness);
    }

}
