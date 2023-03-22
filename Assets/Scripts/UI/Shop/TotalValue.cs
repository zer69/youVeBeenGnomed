using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalValue : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI totalOrder;
    [SerializeField] private TextMeshProUGUI totalNow;


    private int currentValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePrice(int value)
    {
        currentValue += value;
        totalOrder.text = currentValue.ToString();
        totalNow.text = (currentValue * 1.5f).ToString();
    }
}
