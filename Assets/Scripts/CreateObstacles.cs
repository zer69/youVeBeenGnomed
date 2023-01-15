using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacles : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float range = 50;
    [SerializeField] int numberOfObstacles = 7;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 position = new Vector3(Random.Range(-range, range), 1, Random.Range(-range, range));
            Instantiate(obstaclePrefab, position, obstaclePrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
