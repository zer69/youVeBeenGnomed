using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int money;
    public int reputation;
    public int reputationLevel;
    public int currentOrder;
    public int day;
    public Vector3 playerPosition;
    public Vector3 thongsPosition;
    public Vector3 hammerPosition;
    // public Vector3 glassesPosition;
    public List<Order> orders;
    public int charge;
    public int ordersDone;
    public int ordersExpired;
    public bool thongsInHand;
    public bool hammerInHand;
    public bool ingotInHand;
    public bool ingotInThongs;
    public int ingotsObtained;

    public GameData()
    {
        money = 500;
        reputation = 500;
        reputationLevel = 5;
        currentOrder = -1;
        ordersDone = 0;
        ordersExpired = 0;
        ingotsObtained = 0;
        charge = 100;
        day = 1;

        playerPosition = new Vector3(-14.44f, -2.89f, -3.44f);
        thongsPosition = new Vector3(-15.55f, 3.754186f, 4.886346f);
        hammerPosition = new Vector3(-12.42f, -0.85f, 0.99f);

        orders = new List<Order>();

        thongsInHand = false;
        hammerInHand = false;
        ingotInHand = false;
        ingotInThongs = false;
    }
}
