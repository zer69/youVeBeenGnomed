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
    public List<Order> orders;
    // Start is called before the first frame update
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
            foreach (Order order in orders)
            {
                order.daysToExpire--;
                if (order.daysToExpire == 0)
                {
                    ExpiredOrder();
                }
            }

            List<Order> generateOrders = GenerateOrders(reputationLevel, Random.Range(1, 4));

            foreach (Order order in generateOrders)
            {
                orders.Add(order);
            }
            Debug.Log("Processing");
        }
    }

    public void ExpiredOrder()
    {

    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
        this.reputation = data.reputation;
        this.reputationLevel = data.reputationLevel;
        this.currentOrder = data.currentOrder;
        this.orders = data.orders;
        this.day = data.day;
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
        data.reputation = this.reputation;
        data.reputationLevel = this.reputationLevel;
        data.currentOrder = this.currentOrder;
        data.orders = this.orders;
        data.day = this.day;
    }
}
