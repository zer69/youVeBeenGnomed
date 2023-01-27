using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilAct : MonoBehaviour
{
    [SerializeField] private float ingotWidth = 1.6f;
    [SerializeField] private float anvilHeight = 1;
    [SerializeField] private float ingotHeight = 0.4f;
    [SerializeField] private float ingotSectionWidth;
    private GameObject ingot;
    private Vector3 sectionLocation;
    private float leftBound;
    [SerializeField] private GameObject ingotSectionPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        ingotSectionWidth = ingotWidth / 10;
    }

    void Start()
    {
        ingot = GameObject.FindGameObjectWithTag("Ingot");
        leftBound = ingot.transform.position.x - ingotWidth / 2 + ingotSectionWidth / 2;
        sectionLocation = new Vector3(leftBound, ingotHeight + anvilHeight + 0.01f, ingot.transform.position.z);
        showSection(sectionLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void showSection(Vector3 position)
    {
        Instantiate(ingotSectionPrefab, position, ingotSectionPrefab.transform.rotation);
    }
}
