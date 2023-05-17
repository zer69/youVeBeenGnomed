using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayTest : MonoBehaviour
{
    [SerializeField] private b_GameEvent nextDay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NextDay());
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
}
