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
    Camera camera;
    private List<float> sectionList;
    private List<bool> sectionResult;
    private Vector3 ingotBoundsSize;
    private GameObject processedIngot;
    private Transform hammer;
    private Transform thongs;
    private Inventory playerInventory;

    [Header("Anvil sections parameters")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private int[] numberOfSectionsInRound = new int[3] { 4, 3, 6 };
    [SerializeField] private int numberOfRounds = 3;
    [SerializeField] private float sectionLiveTime = 1;
    [SerializeField] private float secondToLeave = 2;
    [SerializeField] private float sectionDisappearTime = 1;
    

    [Header("To track")]
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] private bool anvilMode = false;
    [SerializeField] private bool hitMode = false;
    [SerializeField] private bool hasWorkOnAnvil = true;
    [SerializeField] private int sectionCounter = 0;
    [SerializeField] private int roundCounter = 0;
    private bool start = true;
    private bool roundReset = true;
    private bool sectionIsVisible = false;
    private int successfulHits = 0;
    private int mouseClickCounter = 0;

    [Header("Do not touch objects")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera anvilCamera;
    [SerializeField] private Transform anvilPositionObject;
    [SerializeField] private Transform anvilHammer;
    [SerializeField] private GameObject ingotSectionPrefab;
    [SerializeField] private GameObject ingotPrefab;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private s_GameEvent hint;
    [SerializeField] private go_GameEvent setCamera;
    [SerializeField] private GameEvent resetAnvil;
    [SerializeField] private Transform crosshair;
    [SerializeField] private s_GameEvent hotkey;

    [Header("To delete if useless")]
    [BackgroundColor(1.5f, 0f, 1.5f, 1f)]
    [SerializeField] private Material common;
    [SerializeField] private Material uncommon;
    [SerializeField] private Material rare;
    [SerializeField] private Material supremacy;
    [SerializeField] private Material legendary;

    [Header("Sound Events")]
    public AK.Wwise.Event DoneSoundEvent;
    public AK.Wwise.Event PartlyDoneSoundEvent;
    public AK.Wwise.Event HammerSoundEvent;
    public AK.Wwise.Event WeaponPutDownSoundEvent;
    public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    private void Awake()
    {
        camera = Camera.main;
        anvilHeight = gameObject.GetComponent<BoxCollider>().size[2] * 100;
        hammer = GameObject.Find("Hammer").transform;
        thongs = GameObject.Find("Thongs").transform;
        Vector3 thongsColliderSize = thongs.gameObject.GetComponent<BoxCollider>().bounds.size;
        thongsHeight = Mathf.Min(Mathf.Min(thongsColliderSize[0], thongsColliderSize[1]), thongsColliderSize[2]);
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        createEmptyListsForRoundHandler();
    }

    void Start()
    {
        ingotBoundsSize = GameObject.FindGameObjectWithTag("Ingot").GetComponent<BoxCollider>().bounds.size;
        ingotWidth = Mathf.Max(ingotBoundsSize[0], ingotBoundsSize[2]);
        ingotLength = Mathf.Min(ingotBoundsSize[0], ingotBoundsSize[2]);
        ingotHeight = ingotBoundsSize[1];
        playerInventory = GameObject.Find("PLAYER").GetComponent<Inventory>();
        ingotSectionWidth = ingotWidth / 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (anvilMode)
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
        if (!hitMode && sectionCounter >= numberOfSectionsInRound[roundCounter])
        {
            StartCoroutine(hint.DelaySeconds("Now Hit!", 1f));
            /*GameObject[] sectionsToClear = GameObject.FindGameObjectsWithTag("IngotSection");
            foreach (GameObject sc in sectionsToClear)
                sc.transform.localScale = new Vector3(0f, sc.transform.localScale.y, sc.transform.localScale.z);*/
            StartCoroutine(ActivateHitMode());
        }
        
    }

    IEnumerator ActivateHitMode()
    {
        yield return new WaitForSeconds(sectionDisappearTime);
        hitMode = true;
        yield return null;
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


        if (roundCounter < 3)
            
            hint.Raise("Memorize location of sections!");
        
    }


    private void FinishWork()
    {
        Debug.Log("Finish Work!");

        DoneSoundEvent.Post(gameObject);

        hammer.gameObject.SetActive(true);
        anvilHammer.gameObject.SetActive(false);

        camera.gameObject.SetActive(true);
        anvilCamera.gameObject.SetActive(false);
        setCamera.Raise(camera.gameObject);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        //playerInput.transform.localRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;

        thongs.SetParent(camera.transform.Find("Player Transform"));
        thongs.position = camera.transform.Find("Left Hand").position;
        thongs.rotation = camera.transform.Find("Left Hand").rotation;
        thongs.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        thongs.gameObject.GetComponent<BoxCollider>().enabled = true;

        processedIngot.GetComponent<BoxCollider>().enabled = false;
        processedIngot.GetComponent<MeshCollider>().enabled = true;



        start = true;
        roundReset = true;
        //WorkHandler();
        
        successfulHits = 0;
        hotkey.Raise("inHands");
        //crosshair.gameObject.SetActive(false);



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
                heightOffset =  0.397548f + 0.15f;
                break;
            case Ingot.AnvilState.MediumRare:
                heightOffset =  0.397548f + 0.15f + 0.071f;
                break;

        }
        Vector3 location = new Vector3(generateXLocation(ingotWidth, ingotSectionWidth), ingotHeight + anvilHeight + thongsHeight + 0.0001f - heightOffset + 2.603f, processedIngot.transform.position.z);
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
        
        
        Vector3 ingotCenter = processedIngot.transform.position;

        
        zMax = ingotCenter.z + ingotLength / 2;
        zMin = ingotCenter.z + ingotLength / -2;
        xMax = ingotCenter.x + ingotWidth / 2;
        xMin = ingotCenter.x + ingotWidth / -2;
        
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

            HammerSoundEvent.Post(gameObject);
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
            roundCounter = 0;
            anvilMode = false;
            FinishWork();
        }
        else
        {
            if (!sectionIsVisible && sectionCounter < numberOfSectionsInRound[roundCounter] && !hitMode)
            {
                showSection(generateSectionLocation(ingotWidth, ingotHeight, anvilHeight, ingotSectionWidth), sectionLiveTime);
                sectionCounter++;
                
            }
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

        float threshhold = successfulClicks / results.Count;

        processedIngot.GetComponent<Ingot>().anvilState++;
        if (processedIngot.GetComponent<Ingot>().anvilState < Ingot.AnvilState.WellDone)
        {
            PartlyDoneSoundEvent.Post(gameObject);
        }
        if (processedIngot.GetComponent<Ingot>().anvilState == Ingot.AnvilState.WellDone)
            processedIngot.GetComponent<Ingot>().status = Ingot.CompletionStatus.Forged;



        if (successfulClicks >= threshhold)
        {
            processedIngot.GetComponent<Ingot>().quality = (Ingot.OreQuality)Mathf.Clamp((int)++processedIngot.GetComponent<Ingot>().quality, (int)Ingot.OreQuality.Poor, (int)Ingot.OreQuality.Legendary);
        }
            
        else
            processedIngot.GetComponent<Ingot>().quality = (Ingot.OreQuality)Mathf.Clamp((int)--processedIngot.GetComponent<Ingot>().quality, (int)Ingot.OreQuality.Poor, (int)Ingot.OreQuality.Legendary);

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

    private bool createIngotOnAnvil()
    {
        processedIngot = FindChildByTag(thongs.Find("ThongsPosition"), "Ingot");
        if (processedIngot.GetComponent<Ingot>().status < Ingot.CompletionStatus.Melted)
            return false;
        processedIngot.GetComponent<BoxCollider>().enabled = true;
        processedIngot.GetComponent<MeshCollider>().enabled = false;

        

        thongs.SetParent(anvilPositionObject);
        thongs.position = anvilPositionObject.position;
        thongs.rotation = anvilPositionObject.rotation;
        thongs.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        thongs.gameObject.GetComponent<BoxCollider>().enabled = false;
        GenerateRoundsAndSections(processedIngot.GetComponent<Ingot>().oreType);


        return true;




    }

    private void GenerateRoundsAndSections(Ingot.OreType oreType)
    {
        switch(oreType)
        {
            case Ingot.OreType.Copper:
                numberOfRounds = 3;
                numberOfSectionsInRound = new int[3] { 4, 3, 6 };
                break;
            case Ingot.OreType.Iron:
                numberOfRounds = 4;
                numberOfSectionsInRound = new int[4] { 4, 3, 6, 5 };
                break;
            case Ingot.OreType.Silver:
                numberOfRounds = 4;
                numberOfSectionsInRound = new int[4] { 6, 4, 7, 3 };
                break;
        }
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Anvil is used");
        if (!InventoryToWork())
        {
            hint.Raise("Missing an instrument");
            return false;
        }
        WeaponPutDownSoundEvent.Post(gameObject);
        if (!createIngotOnAnvil())
        {
            hint.Raise("Ingot not ready");
            return false;
        }
        
        hammer.gameObject.SetActive(false);
        anvilHammer.gameObject.SetActive(true);

        camera.gameObject.SetActive(false);
        anvilCamera.gameObject.SetActive(true);
        setCamera.Raise(anvilCamera.gameObject);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.None;
        anvilMode = true;
        //crosshair.gameObject.SetActive(false);
        hotkey.Raise("anvil");
        

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
        if (playerInventory.CheckInventoryForItem("IngotInThongs")&& playerInventory.CheckInventoryForItem("Hammer"))
            return true;
        return false;
    }
}
