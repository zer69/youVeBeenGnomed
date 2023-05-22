using System.Collections.Generic;

[System.Serializable]
public class GameData 
{
    public int money;
    public int reputation;
    public int reputationLevel;
    public int day;
    public List<Order> orders;
    public int ordersDone;
    public int ordersExpired;
    public List<int> amounts;
    public List<List<int>> ordersOnHold;
    public List<int> deliveryDay;

    public GameData()
    {
        money = 500;
        reputation = 0;
        reputationLevel = 1;
        ordersDone = 0;
        ordersExpired = 0;
        day = 1;

        orders = new List<Order>();
        amounts = new List<int>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        ordersOnHold = new List<List<int>>();
        deliveryDay = new List<int>();
    }
}
