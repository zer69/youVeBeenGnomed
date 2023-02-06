using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class AnvilAct : MonoBehaviour
{
    [SerializeField] private float ingotWidth = 1.6f;
    [SerializeField] private float anvilHeight = 1;
    [SerializeField] private float ingotHeight = 0.4f;
    [SerializeField] private float ingotSectionWidth;
    [SerializeField] private float zMax = 0.3f;
    [SerializeField] private float zMin = -0.3f;
    [SerializeField] private float xMax = 0.8f;
    [SerializeField] private float xMin = -0.8f;
    [SerializeField] private int[] numberOfSectionsInRound = new int[3] { 4, 3, 6 };
    [SerializeField] private int numberOfRounds = 3;
    Camera camera;
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
    [SerializeField] private Material common;
    [SerializeField] private Material uncommon;
    [SerializeField] private Material rare;
    [SerializeField] private Material supremacy;
    [SerializeField] private Material legendary;
    private float anvilRange = 3;
    private int sectionCounter = 0;
    [SerializeField] private int roundCounter = 0;
    private int mouseClickCounter = 0;
    private List<float> sectionList;
    private List<bool> sectionResult;
    private GameObject ingot;
    private GameObject player;
    private Rigidbody playerRb;
    private bool sectionIsVisible = false;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private float secondToLeave = 2;
    [SerializeField] private float sectionDisappearTime = 1;
    [SerializeField] private GameObject ingotSectionPrefab;
    [SerializeField] private GameObject ingotPrefab;
    private int successfulHits = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        camera = Camera.main;
        ingotSectionWidth = ingotWidth / 10;
        createEmptyListsForRoundHandler();
    }

    void Start()
    {
        ingot = GameObject.FindGameObjectWithTag("Ingot");
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < anvilRange && Mathf.Abs(player.transform.position.z - transform.position.z) < anvilRange)
        {
            anvilMode = true;
            if (anvilMode && hasWorkOnAnvil)
            {
                anvilActive();
            }   
        } else
        {
            anvilMode = false;
        }
    }
    
    IEnumerator IngotSectionRoutine(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        GameObject section = GameObject.FindGameObjectWithTag("IngotSection");
        Vector3 currentScale = section.transform.localScale;
        Vector3 targetScale = new Vector3(0, currentScale.y, currentScale.z);
        StartCoroutine(IngotSectionDisappearRoutine(section, targetScale, sectionDisappearTime));
        
        
        
        sectionIsVisible = false;
    }

    IEnumerator IngotSectionDisappearRoutine(GameObject section, Vector3 targetScale, float seconds)
    {
        float time = 0;
        Vector3 currentScale = section.transform.localScale;
        while (time < seconds)
        {
            section.transform.localScale = Vector3.Lerp(currentScale, targetScale, time / seconds);
            time += Time.deltaTime;
            yield return null;
        }
        // section.transform.localScale = targetScale;
        if (section is not null)
        {
            Destroy(section);
        }
        
    }

    IEnumerator RoundBreak(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hitMode = false;
        roundCounter++;
        createEmptyListsForRoundHandler();
    }


    IEnumerator FinishWork(float seconds)
    {
        Debug.Log("Finish Work!");
        WorkHandler();
        yield return new WaitForSeconds(seconds);
        hasWorkOnAnvil = true;
        successfulHits = 0;

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
        sectionList.Add(position[0]);
        sectionIsVisible = true;
        StartCoroutine(IngotSectionRoutine(seconds));
    }

    private void ingotClickHandler(RaycastHit hit)
    {
        float zClick = hit.point.z;
        float xClick = hit.point.x;
        if (zClick > zMin && zClick < zMax && xClick > xMin && xClick < xMax)
        { 
            if (xClick > sectionList[mouseClickCounter] && xClick < sectionList[mouseClickCounter] + ingotSectionWidth)
            {
                sectionResult.Add(true);
            }
            else
            {
                sectionResult.Add(false);
            }
            mouseClickCounter++;
        }
    }

    private void mouseLeftButtonClickInHitModeHandler()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            Ray ray = camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ingotClickHandler(hit);
            }
        }
    }

    private void anvilActive()
    {
        if (roundCounter >= numberOfRounds)
        {
            hasWorkOnAnvil = false;
            roundCounter = 0;
            Debug.Log("Done");
            StartCoroutine(FinishWork(5));
        }
        else
        {
            if (!sectionIsVisible && sectionCounter < numberOfSectionsInRound[roundCounter] && !hitMode && roundCounter < numberOfRounds)
            {
                showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
                sectionCounter++;
            }
            if (!hitMode && sectionCounter >= numberOfSectionsInRound[roundCounter])
            {
                hitMode = true;
                sectionCounter = 0;
            }
            if (hitMode && mouseClickCounter < numberOfSectionsInRound[roundCounter])
            {
                mouseLeftButtonClickInHitModeHandler();
            }
            if (hitMode && mouseClickCounter >= numberOfSectionsInRound[roundCounter])
            {
                mouseClickCounter = 0;
                resultHandler(sectionResult);
                StartCoroutine(RoundBreak(secondToLeave));
            }
        }
    }
    private void resultHandler(List<bool> results)
    {
        //foreach (bool result in results)
        //{
        //Debug.Log("Result" + result);

        //}
        
        int successfulClicks = results.Count(x => x == true);
        successfulHits += successfulClicks;
        
        Debug.Log(successfulClicks);
    }

    private void WorkHandler()
    {
        Debug.Log("Work Handler!");
        float zMax = -3;
        float zMin = -9;
        float xMax = 9;
        float xMin = -9;
        float height = 2;
        Vector3 location = new Vector3(Random.Range(xMin, xMax), height, Random.Range(zMin, zMax));
        GameObject createdObject = Instantiate(ingotPrefab, location, ingotPrefab.transform.rotation);
        int perfection = numberOfSectionsInRound.Sum();
        if (successfulHits == perfection)
        {
            createdObject.GetComponent<Renderer>().material = legendary;
        }
        else if (successfulHits >= perfection * 0.9)
        {
            createdObject.GetComponent<Renderer>().material = supremacy;
        }
        else if (successfulHits >= perfection * 0.7)
        {
            createdObject.GetComponent<Renderer>().material = rare;
        }
        else if (successfulHits >= perfection * 0.4)
        {
            createdObject.GetComponent<Renderer>().material = uncommon;
        }
        else
        {
            createdObject.GetComponent<Renderer>().material = common;
        }
    }

    private void createEmptyListsForRoundHandler()
    {
        sectionList = new List<float>();
        sectionResult = new List<bool>();
    }
}
