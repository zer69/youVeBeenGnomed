using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour, IDataPersistence
{
    [SerializeField] public int money;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoneyIncreaseRoutine(2, 20));
        StartCoroutine(MoneyIncreaseRoutine(10, 100));
        StartCoroutine(MoneyIncreaseRoutine(30, 2000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
    }

    IEnumerator MoneyIncreaseRoutine(float seconds, int income)
    {
        yield return new WaitForSeconds(seconds);
        money += income;
    }

    public void SpendMoney(int value)
    {
        money -= value;
    }
}
