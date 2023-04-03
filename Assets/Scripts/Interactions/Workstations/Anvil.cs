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
    [SerializeField] private Camera anvilCamera;
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
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
    [SerializeField] private GameObject anvilHammer;
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
    private bool start = true;
    private bool roundReset = true;

    public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    private void Awake()
    {
        //Debug.Log(gameObject.transform.position);
        camera = Camera.main;
        anvilPosition = gameObject.transform.position;
        anvilHeight = gameObject.GetComponent<BoxCollider>().size[2] * 100;
        hammer = GameObject.Find("Hammer");
        thongs = GameObject.Find("Thongs");
        Vector3 thongsColliderSize = thongs.GetComponent<BoxCollider>().bounds.size;
        thongsHeight = Mathf.Min(Mathf.Min(thongsColliderSize[0], thongsColliderSize[1]), thongsColliderSize[2]);
        thongsPosition = GameObject.Find("ThongsPosition");
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        createEmptyListsForRoundHandler();
    }

    void Start()
    {
        
        ingot = GameObject.FindGameObjectWithTag("Ingot");
        //Debug.Log(ingot.GetComponent<BoxCollider>().bounds.size);
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
        if (anvilMode && hasWorkOnAnvil && InventoryToWork())
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
        Vector3 targetScale = new Vector3(0.00005f, currentScale.y, currentScale.z);
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
        if (section is not null)
        {
            //Destroy(section);
        }
        if (!hitMode && sectionCounter >= numberOfSectionsInRound[roundCounter])
        {
            hint.Raise("Now hit!");
            /*GameObject[] sectionsToClear = GameObject.FindGameObjectsWithTag("IngotSection");
            foreach (GameObject sc in sectionsToClear)
                sc.transform.localScale = new Vector3(0f, sc.transform.localScale.y, sc.transform.localScale.z);*/
            hitMode = true;
        }
        
    }

    IEnumerator RoundBreak(float seconds)
    {
        GameObject[] sectionsToDelete = GameObject.FindGameObjectsWithTag("IngotSection");
        foreach (GameObject section in sectionsToDelete)
            GameObject.Destroy(section);
        yield return new WaitForSeconds(seconds);
        if (!roundReset)
        {
            
            roundCounter++;
            sectionCounter = 0;
            createEmptyListsForRoundHandler();
        }
        roundReset = false;
        
        hint.Raise("Memorize location of sections!");
        
    }


    IEnumerator FinishWork(float seconds)
    {
        Debug.Log("Finish Work!");

        hammer.gameObject.SetActive(true);
        anvilHammer.gameObject.SetActive(false);

        camera.gameObject.SetActive(true);
        anvilCamera.gameObject.SetActive(false);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        //playerInput.transform.localRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;

        thongs.transform.SetParent(camera.transform.Find("Player Transform"));
        thongs.transform.position = camera.transform.Find("Left Hand").position;
        thongs.transform.rotation = camera.transform.Find("Left Hand").rotation;
        thongs.GetComponent<Rigidbody>().isKinematic = false;
        thongs.GetComponent<BoxCollider>().enabled = true;

        processedIngot.GetComponent<BoxCollider>().enabled = false;
        processedIngot.GetComponent<MeshCollider>().enabled = true;



        start = true;
        roundReset = true;
        //WorkHandler();
        
        hasWorkOnAnvil = true;
        successfulHits = 0;
        StopAllCoroutines();
        anvilMode = false;
        yield return null;
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
        float heightOffset = 0f;
        switch(processedIngot.GetComponent<Ingot>().anvilState)
        {
            case Ingot.AnvilState.Raw:
                heightOffset = 0.397548f;
                break;
            case Ingot.AnvilState.Rare:
                heightOffset = 0.397548f + 0.15f;
                break;
            case Ingot.AnvilState.MediumRare:
                heightOffset = 0.397548f + 0.15f + 0.071f;
                break;

        }
        Vector3 location = new Vector3(generateXLocation(ingotWidth, ingotSectionWidth), ingotHeight + anvilHeight + thongsHeight + 0.0001f - heightOffset, processedIngot.transform.position.z);
        return location;
    }

    private void showSection(Vector3 position, float seconds)
    {
        GameObject section = Instantiate(ingotSectionPrefab, position, ingotSectionPrefab.transform.rotation);
        Vector3 scale = new Vector3(ingotSectionWidth / 10, 1, ingotLength / 10);
        section.transform.localScale = scale;
        sectionList.Add(position[0]);
        sectionIsVisible = true;
        StartCoroutine(IngotSectionRoutine(section, seconds));
    }

    private void ingotClickHandler(RaycastHit hit)
    {
        float zClick = hit.point.z;
        float xClick = hit.point.x;
        
        //Vector3 ingotCenter = FindChildByTag(anvilPositionObject.Find("Thongs(Clone)").Find("ThongsPosition"), "Ingot").transform.position;
        Vector3 ingotCenter = processedIngot.transform.position;

        //Debug.Log(ingotCenter);
        zMax = ingotCenter.z + ingotLength / 2;
        zMin = ingotCenter.z + ingotLength / -2;
        xMax = ingotCenter.x + ingotWidth / 2;
        xMin = ingotCenter.x + ingotWidth / -2;
        //Debug.Log(xClick);
        //Debug.Log(zClick);
        if (zClick > zMin && zClick < zMax && xClick > xMin && xClick < xMax)
        {
            //Debug.Log("Ingot");

            if (xClick > sectionList[mouseClickCounter] - ingotSectionWidth / 2 && xClick < sectionList[mouseClickCounter] + ingotSectionWidth / 2)
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
            Ray ray = anvilCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ingotClickHandler(hit);
            }
        }
    }

    private void anvilActive()
    {
        if (start)
        {
            StartCoroutine(RoundBreak(0f));
            start = false;
        }
            

        if (roundCounter >= numberOfRounds)
        {
            hasWorkOnAnvil = false;
            roundCounter = 0;
            

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
    
        int successfulClicks = results.Count(x => x == true);
        successfulHits += successfulClicks;

        processedIngot.GetComponent<Ingot>().anvilState++;
        if (processedIngot.GetComponent<Ingot>().anvilState == Ingot.AnvilState.WellDone)
            processedIngot.GetComponent<Ingot>().status = Ingot.CompletionStatus.Forged;

        Debug.Log(successfulClicks);
    }

    private void WorkHandler()
    {
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

    private void createIngotOnAnvil()
    {

        thongs.transform.SetParent(anvilPositionObject);
        thongs.transform.position = anvilPositionObject.position;
        thongs.transform.rotation = anvilPositionObject.rotation;
        thongs.GetComponent<Rigidbody>().isKinematic = true;
        thongs.GetComponent<BoxCollider>().enabled = false;

        processedIngot = FindChildByTag(thongs.transform.Find("ThongsPosition"), "Ingot");
        processedIngot.GetComponent<BoxCollider>().enabled = true;
        processedIngot.GetComponent<MeshCollider>().enabled = false;

    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Anvil is used");
        if (!(player.GetComponent<Inventory>().CheckInventoryForItem("IngotInThongs") && player.GetComponent<Inventory>().CheckInventoryForItem("Hammer")))
        {
            return false;
        }
        
        createIngotOnAnvil();
        hammer.gameObject.SetActive(false);
        anvilHammer.gameObject.SetActive(true);

        camera.gameObject.SetActive(false);
        anvilCamera.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.None;
        anvilMode = true;
        
        return true;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        /*switch (context.action.name)
        {
            case "Abort":
                Debug.Log("Leave anvil");
                playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                anvilMode = false;
                camera.gameObject.SetActive(true);
                anvilCamera.gameObject.SetActive(false);
                playerInput.actions.FindAction("DropItems").Enable();
                Cursor.lockState = CursorLockMode.Locked;

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
        }*/
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



    private bool InventoryToWork()
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory.CheckInventoryForItem("IngotInThongs")&& inventory.CheckInventoryForItem("Hammer"))
            return true;
        return false;
    }
}
