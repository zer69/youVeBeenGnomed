using UnityEngine;
using UnityEngine.InputSystem;

public class Barrel : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    // Cooling Rate per Time.deltaTime
    [SerializeField] private float CoolingRate;
    [SerializeField] private float StrengthRate;
    [SerializeField] private float FragilityRate;

    [SerializeField] private Transform ingot;

    private bool canControlIngot = false;
    private Vector2 moveWeaponCommand = Vector2.zero;
    
    public string InteractionPrompt => _prompt;

    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    void Update()
    {
        
        if (moveWeaponCommand != Vector2.zero)
        {
            MoveWeapon();
        }
    }

    public bool Interact(Interactor interactor)
    {
         
        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        canControlIngot = true;
        //Debug.Log("Barrel is used");
        return true;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
                cam.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);
                playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                canControlIngot = false;
                break;

            case "MoveIntoBarrel":
                
                if (canControlIngot)
                {
                    moveWeaponCommand = context.action.ReadValue<Vector2>();
                }
                break;
        }
    }

    private void CoolingWeapon()
    {
        //check that the change of fragility and strength values ??occurs only when the ingot is cooling 
        if (ingot.gameObject.GetComponent<Ingot>().Cooling(CoolingRate))
        {
            ingot.gameObject.GetComponent<Ingot>().Cooling(CoolingRate);
            ingot.gameObject.GetComponent<Ingot>().FragilityModification(FragilityRate);
            ingot.gameObject.GetComponent<Ingot>().StrengthModification(StrengthRate);
        }
        
    }


    void MoveWeapon()
    {
        float yMouse = moveWeaponCommand.y * Time.deltaTime;

        Vector3 weaponVector = new Vector3(0, yMouse, 0);

        ingot.position += weaponVector;

    }


    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Ingot")
        {
            Debug.Log("Stay");
            CoolingWeapon();
        }
    }


}
