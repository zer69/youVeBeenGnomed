using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Anvil : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    private float ingotWidth;
    private float anvilHeight;
    private float ingotLength;
    private float ingotHeight;
    private float ingotSectionWidth;
    private float zMax;
    private float zMin;
    private float xMax;
    private float xMin;
    [SerializeField] private float cameraY = 4.5f;
    [SerializeField] private float cameraOffsetX = 0;
    [SerializeField] private float cameraOffsetZ = 0;
    [SerializeField] private int[] numberOfSectionsInRound = new int[3] { 4, 3, 6 };
    [SerializeField] private int numberOfRounds = 3;
    Camera camera;
    Camera anvilCamera;
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
    [SerializeField] private bool ingotOnAnvil = false;
    [SerializeField] private Material common;
    [SerializeField] private Material uncommon;
    [SerializeField] private Material rare;
    [SerializeField] private Material supremacy;
    [SerializeField] private Material legendary;
    private float anvilRange = 3;
    [SerializeField] private int sectionCounter = 0;
    [SerializeField] private int roundCounter = 0;
    private int mouseClickCounter = 0;
    private List<float> sectionList;
    private List<bool> sectionResult;
    private GameObject ingot;
    private GameObject processedIngot;
    private GameObject player;
    private Vector3 anvilPosition;
    private Rigidbody playerRb;
    private bool sectionIsVisible = false;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private float secondToLeave = 2;
    [SerializeField] private float sectionDisappearTime = 1;
    [SerializeField] private GameObject ingotSectionPrefab;
    [SerializeField] private GameObject ingotPrefab;
    private int successfulHits = 0;
    public FloatVariable quality, initialQuality;
    [SerializeField] private PlayerInput playerInput;

    public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log(gameObject.transform.position);
        camera = Camera.main;
        anvilPosition = gameObject.transform.position;
        anvilCamera = GameObject.Find("Anvil Camera").GetComponent<Camera>();
        cameraPreparation();
        anvilHeight = gameObject.GetComponent<BoxCollider>().size[2] * 100;
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        createEmptyListsForRoundHandler();
    }

    void Start()
    {
        
        ingot = GameObject.FindGameObjectWithTag("Ingot");
        Debug.Log(ingot.GetComponent<BoxCollider>().bounds.size);
        ingotWidth = Mathf.Max(ingot.GetComponent<BoxCollider>().bounds.size[0], ingot.GetComponent<BoxCollider>().bounds.size[2]);
        ingotLength = Mathf.Min(ingot.GetComponent<BoxCollider>().bounds.size[0], ingot.GetComponent<BoxCollider>().bounds.size[2]);
        ingotHeight = ingot.GetComponent<BoxCollider>().bounds.size[1];
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
        ingotSectionWidth = ingotWidth / 10;
        zMax = anvilPosition.z + ingotLength / 2;
        zMin = anvilPosition.z + ingotLength / -2;
        xMax = anvilPosition.x + ingotWidth / 2;
        xMin = anvilPosition.x + ingotWidth / -2;
        //Debug.Log(ingotSectionWidth);
        //Debug.Log(ingotLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (anvilMode && hasWorkOnAnvil && ingotOnAnvil)
        {
            anvilActive();
        }
        //if (Mathf.Abs(player.transform.position.x - transform.position.x) < anvilRange && Mathf.Abs(player.transform.position.z - transform.position.z) < anvilRange)
        //{

        //camera.gameObject.SetActive(false);
        //anvilCamera.gameObject.SetActive(true);
        //anvilMode = true;
        //if (anvilMode && hasWorkOnAnvil)
        //{
        //anvilActive();
        //}   
        //} else
        //{
        //camera.gameObject.SetActive(true);
        //anvilCamera.gameObject.SetActive(false);
        //anvilMode = false;
        //}
    }
    
    IEnumerator IngotSectionRoutine(GameObject section, float seconds)
    {
        //Debug.Log("IngotSectionRoutine");

        yield return new WaitForSeconds(seconds);
        Vector3 currentScale = section.transform.localScale;
        Vector3 targetScale = new Vector3(0, currentScale.y, currentScale.z);
        StartCoroutine(IngotSectionDisappearRoutine(section, targetScale, sectionDisappearTime));
        
        
        
        sectionIsVisible = false;
    }

    IEnumerator IngotSectionDisappearRoutine(GameObject section, Vector3 targetScale, float seconds)
    {
        //Debug.Log("IngotSectionDisappearRoutine");
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
        if (!hitMode && sectionCounter >= numberOfSectionsInRound[roundCounter])
        {
            hitMode = true;
        }
        
    }

    IEnumerator RoundBreak(float seconds)
    {
        //hitMode = false;
        //Debug.Log("RoundBreak");
        yield return new WaitForSeconds(seconds);
        
        roundCounter++;
        sectionCounter = 0;
        createEmptyListsForRoundHandler();
    }


    IEnumerator FinishWork(float seconds)
    {
        Debug.Log("Finish Work!");
        Destroy(processedIngot.gameObject);
        ingotOnAnvil = false;
        WorkHandler();
        yield return new WaitForSeconds(seconds);
        hasWorkOnAnvil = true;
        successfulHits = 0;

    }
    private float generateXLocation(float ingotWidth, float ingotSectionWidth)
    {
        float leftBound = processedIngot.transform.position.x - ingotWidth / 2 + ingotSectionWidth / 2;
        float rightBound = leftBound + ingotWidth - ingotSectionWidth;
        float xLocation = Random.Range(leftBound, rightBound);
        return xLocation;
    }

    private Vector3 generateSectionLocation(float ingotWidth, float ingotHeight, float anvilHeight, float ingotSectionWidth)
    {
        Vector3 location = new Vector3(generateXLocation(ingotWidth, ingotSectionWidth), ingotHeight + anvilHeight + 0.001f + anvilPosition.y, processedIngot.transform.position.z);
        return location;
    }

    private void showSection(Vector3 position, float seconds)
    {
        //Debug.Log(position);
        //Debug.Log("showSection");
        GameObject section = Instantiate(ingotSectionPrefab, position, ingotSectionPrefab.transform.rotation);
        Vector3 scale = new Vector3(ingotSectionWidth / 10, 1, ingotLength / 10);
        section.transform.localScale = scale;
        //scale.z = ingotSectionWidth;
        //scale.x = ingotLength;
        //section.transform.localScale = scale;
        //section.transform.SetParent(ingot.transform, false);
        sectionList.Add(position[0]);
        sectionIsVisible = true;
        StartCoroutine(IngotSectionRoutine(section, seconds));
    }

    private void ingotClickHandler(RaycastHit hit)
    {
        float zClick = hit.point.z;
        float xClick = hit.point.x;
        //Debug.Log("ingotClickHandler");
        //sCounter++;
        //Debug.Log("Clicked" + sCounter);
        //Debug.Log(xClick);
        //Debug.Log(zClick);
        if (zClick > zMin && zClick < zMax && xClick > xMin && xClick < xMax)
        {
            //if (xClick > sectionList[mouseClickCounter] && xClick < sectionList[mouseClickCounter] + ingotSectionWidth)
            if (xClick > sectionList[mouseClickCounter] - ingotSectionWidth / 2 && xClick < sectionList[mouseClickCounter] + ingotSectionWidth / 2)
            {
                sectionResult.Add(true);
                //quality.value += 0.1f;
            }
            else
            {
                sectionResult.Add(false);
                //quality.value -= 0.1f;
            }
            mouseClickCounter++;
        }
    }

    private void mouseLeftButtonClickInHitModeHandler()
    {
        //Debug.Log("mouseLeftButtonClickInHitModeHandler");
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            Ray ray = anvilCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ingotClickHandler(hit);
            }
        }
    }

    private void anvilActive()
    {
        //Debug.Log("anvilActive");
        if (roundCounter >= numberOfRounds)
        {
            hasWorkOnAnvil = false;
            roundCounter = 0;
            //Debug.Log("Done");
            StartCoroutine(FinishWork(5));
        }
        else
        {
            if (!sectionIsVisible && sectionCounter < numberOfSectionsInRound[roundCounter] && !hitMode)
            {
                showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
                sectionCounter++;
            }
            //if (!sectionIsVisible && !hitMode && sectionCounter >= numberOfSectionsInRound[roundCounter])
            //{
                //hitMode = true;
                //sectionCounter = 0;
            //}
            if (hitMode && mouseClickCounter < numberOfSectionsInRound[roundCounter])
            {
                mouseLeftButtonClickInHitModeHandler();
            }
            if (hitMode && mouseClickCounter >= numberOfSectionsInRound[roundCounter])
            {
                hitMode = false;
                mouseClickCounter = 0;
                resultHandler(sectionResult);
                StartCoroutine(RoundBreak(secondToLeave));
            }
        }
    }
    private void resultHandler(List<bool> results)
    {
        //Debug.Log("resultHandler");
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
        //Debug.Log("Work Handler!");
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
        //Debug.Log("createEmptyListsForRoundHandler");
        sectionList = new List<float>();
        sectionResult = new List<bool>();
    }

    private void createIngotOnAnvil()
    {
        if (!ingotOnAnvil)
        {
            Vector3 position = new Vector3(gameObject.transform.position.x, anvilHeight, gameObject.transform.position.z);
            processedIngot = Instantiate(ingotPrefab, position, ingotPrefab.transform.rotation);
            processedIngot.transform.Rotate(Vector3.forward, 90);
            processedIngot.tag = "IngotOnAnvil";
            ingotOnAnvil = true;
        }
    }

    private void cameraPreparation()
    {
        anvilCamera.gameObject.SetActive(false);
        anvilCamera.gameObject.transform.position = new Vector3(anvilPosition.x + cameraOffsetX, cameraY, anvilPosition.z + cameraOffsetZ);
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Anvil is used");
        createIngotOnAnvil();
        camera.gameObject.SetActive(false);
        anvilCamera.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        anvilMode = true;
        
        return true;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
                Debug.Log("Leave anvil");
                playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                anvilMode = false;
                camera.gameObject.SetActive(true);
                anvilCamera.gameObject.SetActive(false);

                break;
        }
    }
}
