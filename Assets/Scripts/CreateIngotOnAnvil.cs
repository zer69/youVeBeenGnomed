using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIngotOnAnvil : MonoBehaviour
{
    [SerializeField] private GameObject ingotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(0, 1, 0);
        Instantiate(ingotPrefab, position, ingotPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
