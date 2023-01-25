using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Whetstone : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Rigidbody whetStoneWheel;
    [SerializeField] private Vector3 rotationForce;

    [SerializeField] private float currentAngleVelocity;
    [SerializeField] private float sharpeningAngleVelocity = 12f;
    [SerializeField] private float mouseSensetivity;

    [SerializeField] private Transform ingot;


    private bool isTired = false;
    private Vector2 moveWeaponCommand = Vector2.zero;


    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
       
            
    }

    private void Update()
    {
        currentAngleVelocity = whetStoneWheel.angularVelocity.magnitude;
        if (moveWeaponCommand != Vector2.zero)
        {
            MoveWeapon();
        }
    }

    public bool Interact(Interactor interactor)
    {
        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        //Debug.Log("Whetstone is used");
        return true;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
                cam.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);
                break;
            case "RotateWhetStone":
                RotateWhetStone();
                break;
            case "MoveToSharpen":
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
        float xMouse = moveWeaponCommand.x * mouseSensetivity * Time.deltaTime;
        float yMouse = moveWeaponCommand.y * mouseSensetivity * Time.deltaTime;

        Vector3 weaponVector = new Vector3(xMouse, yMouse, 0);

        ingot.position += weaponVector;

    }

    void SharpenWeapon()
    {
        if (currentAngleVelocity >= sharpeningAngleVelocity)
        {

        }
        else
        {
            //not enough angular velocity
        }
    }
}
