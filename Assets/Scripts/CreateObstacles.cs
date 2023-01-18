using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacles : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float range = 50;
    [SerializeField] int numberOfObstacles = 1;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 position = new Vector3(Random.Range(-range, range), 1, Random.Range(-range, range));
            Instantiate(obstaclePrefab, position, obstaclePrefab.transform.rotation);
        }

        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 position = new Vector3(Random.Range(-range, range), 1, Random.Range(-range, range));
            Instantiate(boxPrefab, position, boxPrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
