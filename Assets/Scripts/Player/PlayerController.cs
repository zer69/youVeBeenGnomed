using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player controller parameters")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float angleVelocity;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float mouseSensetivity;

    [Header("Do not touch objects")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float playerHeight;
    
    private GameObject playerCamera;
    private Rigidbody playerRb;
    private bool onGround;
    private float xRotation = 0;
    private LayerMask isGround;
    private Vector2 lookCommand = Vector2.zero;
    private Vector2 moveCommand = Vector2.zero;
    private bool jumping = false;
    // Initializing variables on awake
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCamera = GameObject.Find("Main Camera");
        playerRb.mass = 70;
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        isGround = LayerMask.GetMask("jumpingSurface", "Pickable");
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    // Gain inputs
    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                //if (onGround)
                //{
                    moveCommand = context.action.ReadValue<Vector2>();
                //}
                    
                break;

            case "Jump":
                if (context.phase == InputActionPhase.Started)
                {
                    jumping = true;
                } else if (context.phase == InputActionPhase.Canceled)
                {
                    jumping = false;
                }
                
                break;

            case "Look":
                lookCommand = context.action.ReadValue<Vector2>();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

        // Camera control
        if (lookCommand != Vector2.zero)
        {
            Look();
        }

        if (!playerInput.actions["Move"].IsPressed())
        {
           // moveCommand = Vector2.zero;
        }
        
        ControlDrag();
        
    }
    // Updating physic movement
    private void FixedUpdate()
    {
        // Check player is on ground with raycast
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight + 0.02f, isGround);
        if (moveCommand != Vector2.zero)
        {
            Move();
        }
        if (jumping && onGround)
        {
            Jump();
            onGround = false;
        }
        
    }

    private void ControlDrag()
    {
        if (onGround)
        {
            playerRb.drag = groundDrag;
        }
        else
        {
            playerRb.drag = 1;
        }
    }

    private void Move()
    {
        Vector3 direction = transform.forward * moveCommand.y + transform.right * moveCommand.x;
        
        SpeedControl();

       
        playerRb.AddForce(direction * speed, ForceMode.Force);
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     
    }

    private void Look()
    {
        float xMouse = lookCommand.x * mouseSensetivity * Time.deltaTime;
        float yMouse = lookCommand.y * mouseSensetivity * Time.deltaTime;

        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * xMouse);
    }

    // Limit body's velocity to not allow permanent acceleration
    private void SpeedControl()
    {
        Vector3 moveVelocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);

        if (moveVelocity.magnitude > maxVelocity)
        {
            Vector3 limitedVelocity = moveVelocity.normalized * maxVelocity;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
    }
}
