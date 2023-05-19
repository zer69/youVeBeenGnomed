using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayTest : MonoBehaviour
{
    [SerializeField] private b_GameEvent nextDay;
    [SerializeField] private eo_GameEvent orderDone;
    public int i;
    public Order order;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NextDay());
        //Next line is important for testing. Please, do not delete it!
        //StartCoroutine(OrderDone(order, i));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NextDay()
    {
        yield return new WaitForSeconds(5);
        nextDay.Raise(true);
        Debug.Log("Next Day");
    }

    IEnumerator OrderDone(Order order, int i)
    {
        yield return new WaitForSeconds(7);
        orderDone.Raise(order, i);
        Debug.Log("Order number " + i + " is done");
    }

    public void LevelUp(bool levelUp)
    {
        if (levelUp)
        {
            Debug.Log("Level UP!");
        }
    }

    public void LevelDown(bool levelDown)
    {
        if (levelDown)
        {
            Debug.Log("Level DOWN!");
        }
    }

    public void ListUpdate(List<Order> orderList)
    {
        Debug.Log("Order list has just been updated. There are " + orderList.Count + " items now.");
    }
}
