using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


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
    public List<Order> orders = new List<Order>();
    // Some values for basic calculations
    [SerializeField] private float fineMultiplier;
    [SerializeField] private int[] levelValues;
    // Game events to declare change of player's reputation level
    [SerializeField] private b_GameEvent reputationLevelUp;
    [SerializeField] private b_GameEvent reputationLevelDown;
    // Game event to declare current list of available orders has changed
    [SerializeField] private o_GameEvent ordersListUpdate;
    [SerializeField] private OrderPool orderPool;
    [SerializeField] private s_GameEvent hint;
    // Info script reference to refresh stocks every dat
    [SerializeField] private InfoSwitch info;

    public TMP_Text rep;

    void Awake()
    {
        fineMultiplier = 0.5f;
        levelValues = new int[] {80, 200, 360, 560, 800, 1080};
    }

    void Start()
    {
        //StartCoroutine(TestOrders(3f));
    }

    // Update is called once per frame
    void Update()
    {
        rep.text = reputation.ToString();
    }

    IEnumerator TestOrders(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //orders = new List<Order>();
        
        List<Order> generateOrders = GenerateOrders(reputationLevel, 1);
    
        foreach (Order order in generateOrders)
        {
            orders.Add(order);
        }
      
        //currentOrder = 1;
        money += 20;
        orderPool.orderList = orders;
        //Debug.Log(orders[currentOrder].oreType);
    }

    public void SpendMoney(int value)
    {
        money -= value;
    }

    public List<Order> GenerateOrders(int reputationLevel, int quantity)
    {
        OrderGenerator generator = new OrderGenerator();

        List<Order> orders = generator.generateOrdersToOneDay(reputationLevel, quantity);

        return orders;
    }

    public void GenerateOrdersOnStart(bool newGame)
    {
        Debug.Log("Generate On Start");
        if (newGame)
        {
            List<Order> generateOrders = GenerateOrders(reputationLevel, Random.Range(1, 4));

            foreach (Order order in generateOrders)
            {
                orders.Add(order);
            }

            orderPool.orderList = orders;
        }
    }

    public void DayProcessing(bool nextDay)
    {
        Debug.Log("NEXT DAY");
        if (nextDay){
            day += 1;
            info.RefreshStock();
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

            orderPool.orderList = orders;

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
        reputation -= (int)(order.reputation * fineMultiplier);
        CheckReputationLevel();
    }

    // Function for calculating effects of order player has finished
    public void DoneOrderCalculations(Order result, Order order)
    {
        float rewardCoefficient = 1f;

        if (result.weaponType != order.weaponType)
            rewardCoefficient *= 0.5f;

        if (result.oreType != order.oreType)
            rewardCoefficient *= 0.5f;

        if (result.oreQuality > order.oreQuality)
            rewardCoefficient *= 1.2f;
        if (result.oreQuality < order.oreQuality)
            rewardCoefficient *= 0.9f;

        if (result.requiredStrength > order.requiredStrength)
            rewardCoefficient *= 1.2f;
        if (result.requiredStrength < order.requiredStrength)
            rewardCoefficient *= 0.9f;

        if (result.requiredSharpness > order.requiredSharpness)
            rewardCoefficient *= 1.2f;
        if (result.requiredSharpness < order.requiredSharpness)
            rewardCoefficient *= 0.9f;

        if (result.requiredFragility < order.requiredFragility)
            rewardCoefficient *= 1.2f;
        if (result.requiredFragility < order.requiredFragility)
            rewardCoefficient *= 0.9f;

        if (result.enchantment != order.enchantment)
        {
            if (order.enchantment == Ingot.Enchantment.None)
                rewardCoefficient *= 1.1f;
            else
                rewardCoefficient *= 0.8f;
        }
        else
            rewardCoefficient *= 1.2f;
            
        
        
        



        money += (int)((float)order.price * rewardCoefficient);
        reputation += (int)((float)order.reputation * rewardCoefficient);
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
        //this.currentOrder = data.currentOrder;
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
        //data.currentOrder = this.currentOrder;
        data.orders = this.orders;
        data.day = this.day;
        data.ordersDone = this.ordersDone;
        data.ordersExpired = this.ordersExpired;
    }
}
