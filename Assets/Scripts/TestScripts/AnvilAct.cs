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
    private bool sectionIsVisible = false;
    private int sectionCounter = 0;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private GameObject ingotSectionPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        ingotSectionWidth = ingotWidth / 10;
    }

    void Start()
    {
        ingot = GameObject.FindGameObjectWithTag("Ingot");

        showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!sectionIsVisible)
        {
            showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
        }
        Debug.Log(sectionCounter);
    }
    
    IEnumerator IngotSectionRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject section = GameObject.FindGameObjectWithTag("IngotSection");
        Destroy(section);
        sectionIsVisible = false;
    }

    private float generateXLocation(float ingotWidth, float ingotSectionWidth)
    {
        float leftBound = ingot.transform.position.x - ingotWidth / 2 + ingotSectionWidth / 2;
        float rightBound = leftBound + ingotWidth - ingotSectionWidth;
        float xLocation = Random.Range(leftBound, rightBound);
        return xLocation;
    }

    private Vector3 generateSectionLocation(float ingotWidth, float ingotHeight, float anvilHeight, float ingotSectionWidth)
    {
        Vector3 location = new Vector3(generateXLocation(ingotWidth, ingotSectionWidth), ingotHeight + anvilHeight + 0.01f, ingot.transform.position.z);
        return location;
    }

    private void showSection(Vector3 position, float seconds)
    {
        Instantiate(ingotSectionPrefab, position, ingotSectionPrefab.transform.rotation);
        StartCoroutine(IngotSectionRoutine(seconds));
        sectionIsVisible = true;
        sectionCounter++;
    }
}
