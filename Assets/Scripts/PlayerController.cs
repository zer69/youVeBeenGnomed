using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float playerHeight;
    [SerializeField] private float groundDrag; 
    [SerializeField] private float angleVelocity;
    [SerializeField] private float maxVelocity;
    private Rigidbody playerRb;
    private bool onGround;
    private LayerMask isGround;
    private Vector2 moveCommand = Vector2.zero;
    private Vector3 angleVelocityV;
    private bool jumping = false;
    // Start is called before the first frame update
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.mass = 70;
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        angleVelocityV = new Vector3(0, angleVelocity, 0);
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                moveCommand = context.action.ReadValue<Vector2>();
                break;

            case "Jump":
                jumping = true;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (moveCommand != Vector2.zero)
        {
            onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);
            // transform.position += (transform.forward * moveCommand.y + transform.right * moveCommand.x) * speed * Time.deltaTime;
            Vector3 direction = new Vector3(moveCommand.x, 0, moveCommand.y);
            // playerRb.AddForce(direction, ForceMode.Impulse);
            // Vector3 direction = orientation.forward * moveCommand.y + orientation.right * moveCommand.x;
            // playerRb.velocity = direction;
            Quaternion deltaRotation = Quaternion.Euler(angleVelocityV * moveCommand.x * Time.deltaTime);
            SpeedControl();
            if (onGround)
            {
                playerRb.drag = groundDrag;
            } else
            {
                playerRb.drag = 0;
            }
            // transform.Rotate(0, moveCommand.x * rotationSpeed * Time.deltaTime, 0);
            playerRb.MoveRotation(playerRb.rotation * deltaRotation);
            playerRb.AddForce(direction * speed, ForceMode.Force);
        }
        if (jumping)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumping = false;
        }
    }

    private void SpeedControl()
    {
        Vector3 moveVelocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);

        if (moveVelocity.magnitude > maxVelocity)
        {
            Vector3 limitedVelocity = moveVelocity.normalized * maxVelocity;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
        Debug.Log(playerRb.velocity.magnitude);
    }
}
