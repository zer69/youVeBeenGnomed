using System.Collections;
using UnityEngine;

[System.Serializable]                         
public class Order
{
    public int id = 0;
    public string itemName = "New Item";
    public Ingot.WeaponType typeId = Ingot.WeaponType.None;
    //public int metallId = 0;
    public Ingot.OreType metallId = Ingot.OreType.Iron;
    public int requiredQuality = 80;
    public string description = "On time or beheaded";
    
}