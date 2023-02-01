using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Whetstone : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f,0f,0f,1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Rigidbody whetStoneWheel;
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] private Vector3 rotationForce;

    private float currentAngleVelocity;
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float sharpeningAngleVelocity = 12f;
    [SerializeField] private float mouseSensetivity;
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    private Transform ingot;
    [SerializeField] private float sharpeningSpeed;
    [BackgroundColor()]



    private bool isTired = false;
    private Vector2 moveWeaponCommand = Vector2.zero;
    private bool increasing = true;
    private bool canControlIngot = false;
    private Transform weaponStartingPosition;
    private Transform playerTransform;
    //private bool interacting = false;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        weaponStartingPosition = GameObject.Find("Weapon Starting Position").transform;
        playerTransform = GameObject.Find("Player Transform").transform;


    }

    private void Update()
    {
        
        
        currentAngleVelocity = whetStoneWheel.angularVelocity.magnitude;
        sharpeningSpeed = currentAngleVelocity / 10.0f;
        if (moveWeaponCommand != Vector2.zero)
        {
            MoveWeapon();
        }
    }

    public bool Interact(Interactor interactor)
    {
        Rigidbody ingotRB = playerTransform.GetComponentInChildren<Rigidbody>();
        //ingotRB.constraints = RigidbodyConstraints.FreezeAll;
        ingotRB.transform.rotation = Quaternion.identity;
        ingotRB.transform.position = weaponStartingPosition.position;
        ingotRB.transform.SetParent(this.transform);
        ingot = ingotRB.transform;

        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        this.GetComponent<CapsuleCollider>().enabled = false;
        canControlIngot = true;

        

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
                    playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    this.GetComponent<CapsuleCollider>().enabled = true;
                    canControlIngot = false;

                    Rigidbody ingotRB = ingot.GetComponent<Rigidbody>();
                    ingotRB.transform.position = cam.transform.Find("Right Hand").position;
                    ingotRB.transform.SetParent(playerTransform);
                    ingot = null;
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
            //hint maybe
        }
        else
        {
            whetStoneWheel.AddTorque(rotationForce);
            isTired = true;
            StartCoroutine(LegCooldown());
        }
    }

    IEnumerator LegCooldown()
    {
        yield return new WaitForSeconds(1f);
        isTired = false;
    }

    void MoveWeapon()
    {
        float xMouse = moveWeaponCommand.x  * Time.deltaTime;
        float yMouse = moveWeaponCommand.y  * Time.deltaTime;

        Vector3 weaponVector = new Vector3(xMouse, yMouse, 0);

        ingot.position += weaponVector;
        //Debug.Log(ingot.position);

    }

    void SharpenWeapon(Collision ingot)
    {
        if (currentAngleVelocity >= sharpeningAngleVelocity)
        {
            float ingotFragility = ingot.gameObject.GetComponent<Ingot>().fragility;
            float sharpnessIncrement = ChangeSharpness(ingotFragility);
            if (ingot.gameObject.GetComponent<Ingot>().sharpness > 100f)
                increasing = false;
            if (increasing)
                ingot.gameObject.GetComponent<Ingot>().sharpness += sharpnessIncrement;
            else
                ingot.gameObject.GetComponent<Ingot>().sharpness -= sharpnessIncrement;
            if (ingot.gameObject.GetComponent<Ingot>().sharpness < 0f && !increasing)
                ingot.gameObject.GetComponent<Ingot>().sharpness = 0;

        }
        else
        {
            //not enough angular velocity
        }
    }

    private float ChangeSharpness(float ingotFragility)
    {
        return sharpeningSpeed * Time.deltaTime * ingotFragility;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.tag);
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Ingot")
        {
            //Debug.Log("sharpening now");
            SharpenWeapon(collision);
        }
    }
}
