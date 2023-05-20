using System.Collections;
using UnityEngine;

[System.Serializable]                         
public class Order : MonoBehaviour
{
    //public int id = 0;
    //public string itemName = "New Item";
    public Ingot.WeaponType weaponType = Ingot.WeaponType.None;
    public Ingot.OreType oreType = Ingot.OreType.Iron;
    public Ingot.OreQuality oreQuality = Ingot.OreQuality.Poor;
    public Ingot.Enchantment enchantment = Ingot.Enchantment.None;
    public int requiredStrength = 0;
    public int requiredSharpness = 0;
    public int requiredFragility = 0;
    public int enchantmentLevel = 1;
    public int orderComplexity = 1;
    // public string description = "On time or beheaded";
    public int price = 2000;
    public int reputation = 10;
    public int daysToExpire = 2;
    // public int[] hitsPerSection = { 4, 3, 6 };
    //public int reputationLevelRequired = 1;

    public override string ToString()
    {
        string value = "OrderComplexity: " + orderComplexity + "\n"
            + "WeaponType: " + weaponType + "\n"
            + "OreType: " + oreType + "\n"
            + "RequiredOreQuality: " + oreQuality + "\n"
            + "       -----------------------       " + "\n";
        if (enchantment != Ingot.Enchantment.None)
        {
            value = value + "Enchantment: " + enchantment + "\n";
            value = value + "EnchantmentLevel: " + enchantmentLevel + "\n";
        }
        if (requiredSharpness != 0)
        {
            value = value + "RequiredSharpness: " + requiredSharpness + "\n";
        }
        if (requiredStrength != 0)
        {
            value = value + "RequiredStrength: " + requiredStrength + "\n";
        }
        if (requiredFragility != 0)
        {
            value = value + "RequiredFragility: " + requiredFragility + "\n";
        }

        return value;
    }
}