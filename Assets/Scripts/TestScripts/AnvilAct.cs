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
    [SerializeField] private int numberOfSectionsInRound = 4;
    [SerializeField] private int numberOfRounds = 3;
    Camera camera;
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
    private float anvilRange = 3;
    private int sectionCounter = 0;
    private int roundCounter = 0;
    private int mouseClickCounter = 0;
    private List<float> sectionList;
    private List<bool> sectionResult;
    private GameObject ingot;
    private GameObject player;
    private Rigidbody playerRb;
    private bool sectionIsVisible = false;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private float secondToLeave = 2;
    [SerializeField] private GameObject ingotSectionPrefab;

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
        Destroy(section);
        sectionIsVisible = false;
    }

    IEnumerator RoundBreak(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hitMode = false;
        roundCounter++;
        createEmptyListsForRoundHandler();
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
        if (!sectionIsVisible && sectionCounter < numberOfSectionsInRound && !hitMode && roundCounter < numberOfRounds)
        {
            showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
            sectionCounter++;
        }
        if (!hitMode && sectionCounter >= numberOfSectionsInRound)
        {
            hitMode = true;
            sectionCounter = 0;
        }
        if (hitMode && mouseClickCounter < numberOfSectionsInRound)
        {
            mouseLeftButtonClickInHitModeHandler();
        }
        if (hitMode && mouseClickCounter >= numberOfSectionsInRound)
        {
            mouseClickCounter = 0;
            resultHandler(sectionResult);
            StartCoroutine(RoundBreak(secondToLeave));
        }
        if (roundCounter >= numberOfRounds)
        {
            hasWorkOnAnvil = false;
            Debug.Log("Done");
        }

    }
    private void resultHandler(List<bool> results)
    {
        //foreach (bool result in results)
        //{
        //Debug.Log("Result" + result);

        //}
        int successfulClicks = results.Count(x => x == true);
        Debug.Log(successfulClicks);
    }

    private void createEmptyListsForRoundHandler()
    {
        sectionList = new List<float>();
        sectionResult = new List<bool>();
    }
}
