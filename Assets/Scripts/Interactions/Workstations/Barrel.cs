using UnityEngine;
using UnityEngine.InputSystem;

public class Barrel : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponStartingPosition;

    private Transform thongs;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float movementSensitivity;

    private bool canControlThongs = false;
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
        Debug.Log("Interact");
        Rigidbody thongsRB = playerTransform.GetComponentInChildren<Rigidbody>();

        thongsRB.transform.rotation = Quaternion.identity;
        thongsRB.transform.position = weaponStartingPosition.position;
        thongsRB.transform.SetParent(weaponStartingPosition);

        thongsRB.transform.Rotate(180, 0, 0);

        thongs = thongsRB.transform;

        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;

      


        canControlThongs = true;
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

                Rigidbody thongsRB = thongs.GetComponent<Rigidbody>();
                thongsRB.transform.rotation = Quaternion.identity;
                thongsRB.transform.position = cam.transform.Find("Left Hand").position;
                thongsRB.transform.SetParent(playerTransform);
                
                //thongs = null;

                canControlThongs = false;
                break;

            case "MoveIntoBarrel":

                if (canControlThongs)
                {
                    moveWeaponCommand = context.action.ReadValue<Vector2>();
                }
                break;
        }
    }

    void MoveWeapon()
    {
        float yMouse = moveWeaponCommand.y * Time.deltaTime * movementSensitivity;

        Vector3 thongsVector = new Vector3(0, yMouse, 0);

        thongs.position += thongsVector;

    }




}
