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
    private float thongsHeight;
    [SerializeField] private float cameraY = 4.5f;
    [SerializeField] private float cameraOffsetX = 0;
    [SerializeField] private float cameraOffsetZ = 0;
    [SerializeField] private float hammerX, thongsX;
    [SerializeField] private float hammerY, thongsY;
    [SerializeField] private float hammerZ, thongsZ;
    [SerializeField] private float thongsSegmentRotation = 46.319f;
    [SerializeField] private int[] numberOfSectionsInRound = new int[3] { 4, 3, 6 };
    [SerializeField] private int numberOfRounds = 3;
    Camera camera;
    Camera anvilCamera;
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
    [SerializeField] private bool ingotOnAnvil = false;
    private bool ingotToTake = false;
    [SerializeField] private Material common;
    [SerializeField] private Material uncommon;
    [SerializeField] private Material rare;
    [SerializeField] private Material supremacy;
    [SerializeField] private Material legendary;
    [SerializeField] private Transform anvilPositionObject;
    private float anvilRange = 3;
    [SerializeField] private int sectionCounter = 0;
    [SerializeField] private int roundCounter = 0;
    private int mouseClickCounter = 0;
    private List<float> sectionList;
    private List<bool> sectionResult;
    private GameObject ingot;
    private GameObject processedIngot;
    private GameObject player;
    private GameObject hammer;
    private GameObject thongsPosition;
    private GameObject thongs;
    private GameObject anvilThongs;
    private GameObject anvilHammer;
    //private GameObject anvilPositionObject;
    private Vector3 anvilPosition;
    
    private Rigidbody playerRb;
    private bool sectionIsVisible = false;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private float secondToLeave = 2;
    [SerializeField] private float sectionDisappearTime = 1;
    [SerializeField] private GameObject ingotSectionPrefab;
    [SerializeField] private GameObject ingotPrefab;
    private GameObject playerTransform;
    private int successfulHits = 0;
    public FloatVariable quality, initialQuality;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private s_GameEvent hint;

    public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log(gameObject.transform.position);
        camera = Camera.main;
        anvilPosition = gameObject.transform.position;
        anvilCamera = GameObject.Find("Anvil Camera").GetComponent<Camera>();
        //anvilPositionObject = GameObject.Find("Anvil Position");
        anvilHeight = gameObject.GetComponent<BoxCollider>().size[2] * 100;
        hammer = GameObject.Find("Hammer");
        thongs = GameObject.Find("Thongs");
        Vector3 thongsColliderSize = thongs.GetComponent<BoxCollider>().bounds.size;
        thongsHeight = Mathf.Min(Mathf.Min(thongsColliderSize[0], thongsColliderSize[1]), thongsColliderSize[2]);
        anvilObjectsPreparation();
        thongsPosition = GameObject.Find("ThongsPosition");
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
        player = GameObject.Find("PLAYER");
        playerRb = player.GetComponent<Rigidbody>();
        ingotSectionWidth = ingotWidth / 10;
        //zMax = anvilPosition.z + ingotLength / 2;
        //zMin = anvilPosition.z + ingotLength / -2;
        //xMax = anvilPosition.x + ingotWidth / 2;
        //xMin = anvilPosition.x + ingotWidth / -2;
        //Debug.Log(ingotSectionWidth);
        //Debug.Log(ingotLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (anvilMode && hasWorkOnAnvil && ingotOnAnvil && InventoryToWork())
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
        returnIngotToHand(processedIngot);
        Destroy(processedIngot.gameObject);
        ingotOnAnvil = false;
        ingotToTake = true;
        WorkHandler();
        yield return new WaitForSeconds(seconds);
        hasWorkOnAnvil = true;
        successfulHits = 0;

    }

    IEnumerator ThongsPreparation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        thongsPreparation();
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
        Vector3 location = new Vector3(generateXLocation(ingotWidth, ingotSectionWidth), ingotHeight + anvilHeight + thongsHeight + 0.001f, processedIngot.transform.position.z);
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
        
        Vector3 ingotCenter = anvilPositionObject.Find("Thongs(Clone)").Find("ThongsPosition").Find("Iron Ingot").position;

        //Vector3 ingotCenter = Vector3.zero;
        Debug.Log(ingotCenter);
        zMax = ingotCenter.z + ingotLength / 2;
        zMin = ingotCenter.z + ingotLength / -2;
        xMax = ingotCenter.x + ingotWidth / 2;
        xMin = ingotCenter.x + ingotWidth / -2;
        Debug.Log(xClick);
        Debug.Log(zClick);
        if (zClick > zMin && zClick < zMax && xClick > xMin && xClick < xMax)
        {
            Debug.Log("Ingot");
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

    private void createIngotOnAnvil(GameObject ingot)
    {
        if (!ingotOnAnvil)
        {
            //Vector3 position = new Vector3(anvilPositionObject.x, anvilHeight, gameObject.transform.position.z);
            //processedIngot = Instantiate(ingot, position, ingotPrefab.transform.rotation);
            //processedIngot = Instantiate(ingot, position, Quaternion.Euler(-90, 0, 90));
            GameObject thongsWithIngot = Instantiate(ingot, anvilPositionObject.position, Quaternion.Euler(90, 0, -90));
            Debug.Log(thongsWithIngot);
            Debug.Log(anvilPositionObject);
            thongsWithIngot.transform.SetParent(anvilPositionObject);
            processedIngot = thongsWithIngot.transform.Find("ThongsPosition").Find("Iron Ingot").gameObject;
            processedIngot.GetComponent<BoxCollider>().enabled = true;
            processedIngot.GetComponent<MeshCollider>().enabled = false;
            // processedIngot.transform.Rotate(Vector3.forward, 90);
            processedIngot.tag = "IngotOnAnvil";
            Destroy(ingot);
            //Debug.Log(ingot);
            //player.GetComponent<Inventory>().IngotIsPicked(false);
            player.GetComponent<Inventory>().SetHasIngotInThongs(false);
            ingotOnAnvil = true;
        }
    }

    private void cameraPreparation()
    {
        anvilCamera.gameObject.SetActive(false);
        anvilCamera.gameObject.transform.position = new Vector3(anvilPosition.x + cameraOffsetX, cameraY, anvilPosition.z + cameraOffsetZ);
    }

    private void hammerPreparation()
    {
        Vector3 position = new Vector3(anvilPosition.x + hammerX, anvilPosition.y + hammerY, anvilPosition.z + hammerZ);
        anvilHammer = Instantiate(GameObject.Find("Hammer"), position, Quaternion.Euler(0, 0, 0));
        anvilHammer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        anvilHammer.transform.SetParent(gameObject.transform);
        anvilHammer.gameObject.SetActive(false);
    }

    private void thongsPreparation()
    {
        Vector3 position = new Vector3(anvilPosition.x + thongsX, anvilPosition.y + thongsY, anvilPosition.z + thongsZ);
        anvilThongs = Instantiate(thongs, position, Quaternion.Euler(90, 180, 90));
        anvilThongs.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //Transform segment1 = anvilThongs.transform.Find("Nail").Find("Tong Segment 1");
        //Transform segment2 = anvilThongs.transform.Find("Nail").Find("Tong Segment 2");
        //segment1.localRotation = Quaternion.Euler(thongsSegmentRotation, segment1.transform.rotation.eulerAngles.y, segment1.transform.rotation.eulerAngles.z);
        //segment2.localRotation = Quaternion.Euler(thongsSegmentRotation, segment2.transform.rotation.eulerAngles.y, segment2.transform.rotation.eulerAngles.z);
        anvilThongs.transform.SetParent(gameObject.transform);
        anvilThongs.gameObject.SetActive(false);

    }

    private void nailsOnAnvil()
    {
        GameObject segment1 = GameObject.Find("Tong Segment 1");
        GameObject segment2 = GameObject.Find("Tong Segment 2");
        segment1.transform.rotation = Quaternion.Euler(46, segment1.transform.rotation.eulerAngles.y, segment1.transform.rotation.eulerAngles.z);
        segment2.transform.rotation = Quaternion.Euler(46, segment2.transform.rotation.eulerAngles.y, segment2.transform.rotation.eulerAngles.z);
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Anvil is used");
        if (player.GetComponent<Inventory>().CheckInventoryForItem("IngotInThongs") && player.GetComponent<Inventory>().CheckInventoryForItem("Hammer") && !ingotOnAnvil && hasWorkOnAnvil)
        {
            //GameObject thongsInHand = GameObject.Find("ThongsPosition");
            // hand flag?
            //GameObject ingotInHand = FindChildByTag(thongsInHand.transform, "Ingot");
            //Debug.Log(ingotInHand);
            createIngotOnAnvil(thongs);
            //Destroy(thongsInHand);
            //thongsPreparation();
            //StartCoroutine(ThongsPreparation(0.5f));
            hammer.gameObject.SetActive(false);
            anvilHammer.gameObject.SetActive(true);
            //thongs.gameObject.SetActive(false);
            //anvilThongs.gameObject.SetActive(true);
            // nailsOnAnvil();
        } else {
            if (player.GetComponent<Inventory>().CheckInventoryForItem("Hammer"))
            {
                hammer.gameObject.SetActive(false);
                anvilHammer.gameObject.SetActive(true);
            }
            //if (player.GetComponent<Inventory>().CheckInventoryForItem("Thongs"))
            //{
                //thongs.gameObject.SetActive(false);
                //anvilThongs.gameObject.SetActive(true);
            //}

        }
        
        camera.gameObject.SetActive(false);
        anvilCamera.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
                if (player.GetComponent<Inventory>().CheckInventoryForItem("Hammer"))
                {
                    hammer.gameObject.SetActive(true);
                    anvilHammer.gameObject.SetActive(false);
                }
                
                if (player.GetComponent<Inventory>().CheckInventoryForItem("Thongs"))
                {
                    thongs.gameObject.SetActive(true);
                    anvilThongs.gameObject.SetActive(false);
                }
               
                Cursor.lockState = CursorLockMode.Locked;
                if (ingotToTake)
                {
                    player.GetComponent<Inventory>().SetHasIngotInThongs(true);
                }
                break;
        }
    }

    private GameObject FindChildByTag(Transform tr, string tag)
    {
        for (int i = 0; i < tr.childCount; i++)
        {
            if (tr.GetChild(i).gameObject.tag == tag)
            {
                return tr.GetChild(i).gameObject;
            }
        }
        return null;
    }

    private void returnIngotToHand(GameObject ingot)
    {
        GameObject ingotInHand = Instantiate(ingot, thongsPosition.transform.position, thongsPosition.transform.rotation);
        ingotInHand.tag = "Ingot";
        ingotInHand.GetComponent<MeshCollider>().enabled = true;
        ingotInHand.GetComponent<BoxCollider>().enabled = false;
        ingotInHand.transform.SetParent(thongsPosition.transform);
    }

    private bool InventoryToWork()
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory.CheckInventoryForItem("Ingot") && !inventory.CheckInventoryForItem("IngotInThongs") && inventory.CheckInventoryForItem("Thongs") && inventory.CheckInventoryForItem("Hammer"))
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void anvilObjectsPreparation()
    {
        cameraPreparation();
        hammerPreparation();
        //thongsPreparation();
    }
}
