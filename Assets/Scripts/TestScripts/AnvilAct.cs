using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnvilAct : MonoBehaviour
{
    [SerializeField] private float ingotWidth = 1.6f;
    [SerializeField] private float anvilHeight = 1;
    [SerializeField] private float ingotHeight = 0.4f;
    [SerializeField] private float ingotSectionWidth;
    [SerializeField] private int numberOfSectionsInRound = 4;
    Camera camera;
    private bool sequenceStartAvailable = true;
    [SerializeField] private bool hitMode = false;
    private float anvilRange = 3;
    private int sectionCounter = 0;
    private int mouseClickCounter = 0;
    private List<float> sectionList;
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
        sectionList = new List<float>();
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
            if (!sectionIsVisible && sectionCounter < numberOfSectionsInRound && !hitMode)
            {
                showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
                sectionCounter++;
                foreach (var el in sectionList)
                {
                    Debug.Log("Section" + el);
                }
                
            }
            if (!hitMode && sectionCounter >= numberOfSectionsInRound)
            {
                hitMode = true;
                sectionCounter = 0;
            }
            if (hitMode && mouseClickCounter < numberOfSectionsInRound)
            {
                Mouse mouse = Mouse.current;
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    Vector3 mousePosition = mouse.position.ReadValue();
                    Ray ray = camera.ScreenPointToRay(mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Debug.Log(hit.point.x + "Click"+ hit.point.z);
                        mouseClickCounter++;
                    }
                }
            }
            if (hitMode && mouseClickCounter >= numberOfSectionsInRound)
            {
                mouseClickCounter = 0;
                StartCoroutine(LeaveAnvil(secondToLeave));
            }

            //Debug.Log(sectionCounter);
            
        }
        else
        {
            
        }
    }
    
    IEnumerator IngotSectionRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject section = GameObject.FindGameObjectWithTag("IngotSection");
        Destroy(section);
        sectionIsVisible = false;
    }

    IEnumerator LeaveAnvil(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hitMode = false;
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
}
