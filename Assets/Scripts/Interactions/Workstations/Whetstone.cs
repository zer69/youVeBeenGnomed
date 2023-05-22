using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Whetstone : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [Header("No Edit")]
    [BackgroundColor(1.5f,0f,0f,1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody whetStoneWheel;
    [SerializeField] private float sharpeningSpeed;
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]

    [Header("One Rotation Stat")]
    [SerializeField] private Vector3 rotationForce;

    private float currentAngleVelocity;
    [Header("Sharpening Stats")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float sharpeningAngleVelocity;
    [SerializeField] private float mouseSensetivity;
    private Transform ingot;
    
    [Header("Events")]
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)]

    [SerializeField] private s_GameEvent hint;
    [SerializeField] private go_GameEvent setCamera;
    [SerializeField] private s_GameEvent hotkey;

    [Header("Sound Events")]
    public AK.Wwise.Event SpiningPlaySoundEvent;
    public AK.Wwise.Event SpiningStopSoundEvent;

    public AK.Wwise.Event SharpenPlaySoundEvent;
    public AK.Wwise.Event SharpenStopSoundEvent;

    private bool isPlayingSpining = false;
    private bool isTired = false;
    private Vector2 moveWeaponCommand = Vector2.zero;
    private bool increasing = true;
    private bool canControlIngot = false;
    private Transform weaponStartingPosition;
    private Transform playerTransform;
    //[SerializeField] private float ingotMovementSensetivity;
    //[SerializeField] private float movementMultiplier;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        weaponStartingPosition = transform.Find("Weapon Starting Position");
        playerTransform = GameObject.Find("Player Transform").transform;


    }

    private void Update()
    {
        
        
        currentAngleVelocity = whetStoneWheel.angularVelocity.magnitude;
        sharpeningSpeed = currentAngleVelocity / 10.0f;
        if (canControlIngot && (moveWeaponCommand != Vector2.zero))
        {
            MoveWeapon();
        }

        if (sharpeningSpeed != 0 && !isPlayingSpining)
        {
            SpiningPlaySoundEvent.Post(gameObject);
            isPlayingSpining = true;
        }
        else if (sharpeningSpeed == 0 && isPlayingSpining)
        {
            SpiningStopSoundEvent.Post(gameObject);
        }
    }

    public bool Interact(Interactor interactor)
    {
        Rigidbody ingotRB = playerTransform.GetComponentInChildren<Rigidbody>();
        if (ingotRB == null)
        {
            hint.Raise("No sharpy for nails");
            //Debug.Log("No ingot detected");
            return false;
        }
        if (ingotRB.gameObject.tag == "Thongs" || ingotRB.gameObject.tag == "Hammer")
        {
            hint.Raise("No instrument needed");
            return false;
        }

        if (!((ingotRB.GetComponent<Ingot>().status == Ingot.CompletionStatus.Sharpened) || (ingotRB.GetComponent<Ingot>().status == Ingot.CompletionStatus.Cooled)))
        {
            hint.Raise("Weapon not ready to sharpen");
            return false;
        }

        ingotRB.transform.rotation = weaponStartingPosition.rotation;
        ingotRB.transform.position = weaponStartingPosition.position;
        ingotRB.transform.SetParent(this.transform);
        ingotRB.GetComponent<BoxCollider>().enabled = true;
        ingot = ingotRB.transform;

        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        setCamera.Raise(cam2.gameObject);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        playerInput.actions.FindAction("DropItems").Disable();
        this.GetComponent<CapsuleCollider>().enabled = false;
        canControlIngot = true;


        hotkey.Raise("whetstone");
        //Debug.Log("Whetstone is used");
        return true;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
                if (canControlIngot)
                {
                    cam.gameObject.SetActive(true);
                    cam2.gameObject.SetActive(false);
                    setCamera.Raise(cam.gameObject);
                    playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    this.GetComponent<CapsuleCollider>().enabled = true;
                    canControlIngot = false;

                    Rigidbody ingotRB = ingot.GetComponent<Rigidbody>();
                    ingotRB.transform.position = cam.transform.Find("Right Hand").position;
                    ingotRB.transform.rotation = cam.transform.Find("Right Hand").rotation;
                    ingotRB.transform.SetParent(playerTransform);
                    ingotRB.GetComponent<BoxCollider>().enabled = false;
                    ingot = null;
                    playerInput.actions.FindAction("DropItems").Enable();
                    hotkey.Raise("inHands");
                }
                


                break;
            case "RotateWhetStone":
                RotateWhetStone();
                break;
            case "MoveToSharpen":
                if (canControlIngot)
                    moveWeaponCommand = context.action.ReadValue<Vector2>();
                break;
        }
    }

    void RotateWhetStone()
    {
        if (isTired)
        {
           
        }
        else
        {
            whetStoneWheel.AddTorque(rotationForce);
            isTired = true;
            StartCoroutine(LegCooldown());
            //Debug.LogWarning("Rotation added");
        }
    }

    IEnumerator LegCooldown()
    {
        yield return new WaitForSeconds(1f);
        isTired = false;
    }

    void MoveWeapon()
    {
        float xMouse = moveWeaponCommand.x * 0.25f  * Time.deltaTime;
        float yMouse = moveWeaponCommand.y * 0.25f * Time.deltaTime;

        Vector3 weaponVector = new Vector3(0f, xMouse, -yMouse);

        ingot.localPosition += weaponVector;
        Vector3 tmppos = ingot.localPosition;

        float newY = Mathf.Clamp(tmppos.y, -1f, 1f);
        float newZ = Mathf.Clamp(tmppos.z, -1.5f, -1.2f);

        ingot.localPosition = new Vector3(ingot.localPosition.x, newY, newZ);

        //Debug.Log(ingot.position);ew

    }

    void SharpenWeapon(Collision ingot)
    {
        if (currentAngleVelocity >= sharpeningAngleVelocity)
        {

            float ingotFragility = ingot.gameObject.GetComponent<Ingot>().fragility;
            float sharpnessIncrement = ChangeSharpness(ingotFragility);
            if (ingot.gameObject.GetComponent<Ingot>().sharpness > 100f)
            {
                increasing = false;
                hint.Raise("It's razor sharp, pull out!");
            }    
                
            if (increasing)
                ingot.gameObject.GetComponent<Ingot>().sharpness += sharpnessIncrement;
            else
                ingot.gameObject.GetComponent<Ingot>().sharpness -= sharpnessIncrement;
            if (ingot.gameObject.GetComponent<Ingot>().sharpness < 0f && !increasing)
                ingot.gameObject.GetComponent<Ingot>().sharpness = 0;
            //Debug.Log("Sharpening now, current sharpness: " + ingot.gameObject.GetComponent<Ingot>().sharpness);
            ingot.gameObject.GetComponent<Ingot>().status = Ingot.CompletionStatus.Sharpened;
            

        }
        else
        {
            //hint.Raise("The wheel should spin faster");
        }
    }

    private float ChangeSharpness(float ingotFragility)
    {
        return sharpeningSpeed * ingotFragility * 0.5f * 0.25f * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ingot")
        {
            //movementMultiplier = 0.5f;
            SharpenWeapon(collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayingSpining && canControlIngot)
        {
            SharpenPlaySoundEvent.Post(gameObject);
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        SharpenStopSoundEvent.Post(gameObject);
    }
    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ingot")
        {
            //movementMultiplier = 1f;


        }
    }*/
}
