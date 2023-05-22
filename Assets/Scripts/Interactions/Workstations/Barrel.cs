using UnityEngine;
using UnityEngine.InputSystem;

public class Barrel : MonoBehaviour, IInteractable
{
    [SerializeField] private s_GameEvent hint;
    [SerializeField] private go_GameEvent setCamera;
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponStartingPosition;
    [SerializeField] private Rigidbody tongsRB;

    private Transform tongs;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float movementSensitivity = 0.4f;

    public bool canControlTongs = false;
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
        //Debug.Log("Interact");

        Inventory inventory = playerTransform.GetComponentInParent<Inventory>();

        if (inventory.hasIngotInThongs)
        {
            //Debug.Log("Interact ok");
            tongsRB.transform.rotation = Quaternion.identity;
            //tongsRB.transform.rotation = Quaternion.AngleAxis(0, Vector3.right);
            tongsRB.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            tongsRB.transform.position = weaponStartingPosition.position;
            tongsRB.transform.SetParent(weaponStartingPosition);

            tongsRB.transform.Rotate(180, 30, 0);

            tongs = tongsRB.transform;

            cam.gameObject.SetActive(false);

            cam2.gameObject.SetActive(true);
            setCamera.Raise(cam2.gameObject);
            playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerInput.transform.localRotation = Quaternion.identity;

            playerInput.actions.FindAction("DropItems").Disable();
            canControlTongs = true;
            //Debug.Log("Barrel is used");
            return true;
        }
        //Debug.Log("No thongs and ingot");
        hint.Raise("Hey, man, where's your weapon?");
        return false;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        if (canControlTongs)
        {
            switch (context.action.name)
            {
                case "Abort":

                    canControlTongs = false;
                    moveWeaponCommand = Vector2.zero;

                    cam.gameObject.SetActive(true);
                    cam2.gameObject.SetActive(false);
                    setCamera.Raise(cam.gameObject);
                    playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    
                    tongsRB.transform.rotation = Quaternion.identity;
                    tongsRB.transform.position = cam.transform.Find("Left Hand").position;
                    tongsRB.transform.rotation = cam.transform.Find("Left Hand").rotation;
                    tongsRB.transform.SetParent(playerTransform);

                    //thongs = null;
                    playerInput.actions.FindAction("DropItems").Enable();

                    
                    break;

                case "MoveIntoBarrel":
                    
                    moveWeaponCommand = context.action.ReadValue<Vector2>();
                    break;
            }
        }
    }

    void MoveWeapon()
    {
        float yMouse = moveWeaponCommand.y * Time.deltaTime * movementSensitivity;

        Vector3 tongsVector = new Vector3(0, yMouse, 0);

        tongs.position += tongsVector;

    }



}
