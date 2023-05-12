using System.Collections;
using UnityEngine;

[System.Serializable]                         
public class Order
{
    public int id = 0;
    public string itemName = "New Item";
    public Ingot.WeaponType typeId = Ingot.WeaponType.None;
    public Ingot.OreType metallId = Ingot.OreType.Iron;
    public int requiredQuality = 80;
    public int requiredSharpness = 0;
    public int requiredFragility = 0;
    public string description = "On time or beheaded";
    public int price = 2000;
    public int reputation = 10;
    public int[] hitsPerSection = { 4, 3, 6 };
    public int reputationLevelRequired = 1;
}