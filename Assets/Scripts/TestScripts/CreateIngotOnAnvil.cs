using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIngotOnAnvil : MonoBehaviour
{
    [SerializeField] private GameObject ingotPrefab;
    private float anvilHeight;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject anvil = GameObject.Find("Anvil");
        anvilHeight = anvil.GetComponent<BoxCollider>().bounds.size[1];
        Vector3 position = new Vector3(0, anvilHeight, 0);
        GameObject ingot = Instantiate(ingotPrefab, position, ingotPrefab.transform.rotation);
        ingot.transform.Rotate(Vector3.forward, 180);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
