using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameStateManager : MonoBehaviour, IDataPersistence
{
    public int day;
    public int money;
    public int reputation;
    public int reputationLevel;
    // public int day;
    public int currentOrder;
    public int ordersDone;
    public int ordersExpired;
    public List<Order> orders;
    // Some values for basic calculations
    [SerializeField] private int fineMultiplier;
    [SerializeField] private int[] levelValues;
    // Game events to declare change of player's reputation level
    [SerializeField] private b_GameEvent reputationLevelUp;
    [SerializeField] private b_GameEvent reputationLevelDown;
    // Game event to declare current list of available orders has changed
    [SerializeField] private o_GameEvent ordersListUpdate;
    // Start is called before the first frame update
    void Awake()
    {
        fineMultiplier = 2;
        levelValues = new int[] {80, 200, 360, 560, 800, 1080};
    }

    void Start()
    {
        //StartCoroutine(TestOrders(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TestOrders(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //orders = new List<Order>();
        
        List<Order> generateOrders = GenerateOrders(reputationLevel, 3);
    
        foreach (Order order in generateOrders)
        {
            orders.Add(order);
        }
      
        currentOrder = 1;
        money += 20;

        Debug.Log(orders[currentOrder].oreType);
    }

    public List<Order> GenerateOrders(int reputationLevel, int quantity)
    {
        OrderGenerator generator = new OrderGenerator();

        List<Order> orders = generator.generateOrdersToOneDay(reputationLevel, quantity);

        return orders;
    }

    public void DayProcessing(bool nextDay)
    {
        if (nextDay){
            day += 1;
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].daysToExpire--;
                if (orders[i].daysToExpire == 0)
                {
                    ordersExpired++;
                    ExpiredOrderCalculations(orders[i]);
                    orders.RemoveAt(i);
                    i--;
                }
            }

            List<Order> generateOrders = GenerateOrders(reputationLevel, Random.Range(1, 4));

            foreach (Order order in generateOrders)
            {
                orders.Add(order);
            }

            ordersListUpdate.Raise(orders);

            Debug.Log("Processing");
        }
    }

    public void OrderProcessing(Order result, int i)
    {
        ordersDone++;
        Order order = orders[i];
        DoneOrderCalculations(result, order);
        orders.RemoveAt(i);
        ordersListUpdate.Raise(orders);
    }

    // Function for calculating effects of expired order
    private void ExpiredOrderCalculations(Order order)
    {
        reputation -= order.reputation * fineMultiplier;
        CheckReputationLevel();
    }

    // Function for calculating effects of order player has finished
    private void DoneOrderCalculations(Order result, Order order)
    {
        money += order.price;
        reputation += order.reputation;
        CheckReputationLevel();
    }

    private void CheckReputationLevel()
    {
        int newLevel = 1;
        for (int i = levelValues.Length - 1; i > -1; i--)
        {
            if (reputation > levelValues[i])
            {
                newLevel = i + 2;
                break;
            }
        }
        if (newLevel > reputationLevel)
        {
            reputationLevelUp.Raise(true);
            reputationLevel = newLevel;
        }
        if (newLevel < reputationLevel)
        {
            reputationLevelDown.Raise(true);
            reputationLevel = newLevel;
        }
    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
        this.reputation = data.reputation;
        this.reputationLevel = data.reputationLevel;
        this.currentOrder = data.currentOrder;
        this.orders = data.orders;
        this.day = data.day;
        this.ordersDone = data.ordersDone;
        this.ordersExpired = data.ordersExpired;
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
        data.reputation = this.reputation;
        data.reputationLevel = this.reputationLevel;
        data.currentOrder = this.currentOrder;
        data.orders = this.orders;
        data.day = this.day;
        data.ordersDone = this.ordersDone;
        data.ordersExpired = this.ordersExpired;
    }
}
