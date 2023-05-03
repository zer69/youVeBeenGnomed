using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int money;
    public int reputation;
    public Vector3 playerPosition;
    public Vector3 thongsPosition;
    public Vector3 hammerPosition;
    // public Vector3 glassesPosition;
    public List<int> currentOrders;
    public int charge;
    public int ordersDone;
    public bool thongsInHand;
    public bool hammerInHand;
    public bool ingotInHand;
    public bool ingotInThongs;
    public int ingotsObtained;

    public GameData()
    {
        money = 500;
        reputation = 0;
        ordersDone = 0;
        ingotsObtained = 0;
        charge = 100;

        playerPosition = new Vector3(-14.44f, -2.89f, -3.44f);
        thongsPosition = new Vector3(-15.55f, 3.754186f, 4.886346f);
        hammerPosition = new Vector3(-12.42f, -0.85f, 0.99f);

        currentOrders = new List<int>();

        thongsInHand = false;
        hammerInHand = false;
        ingotInHand = false;
        ingotInThongs = false;
    }
}
