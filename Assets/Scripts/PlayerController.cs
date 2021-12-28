using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 150f;
    [SerializeField]
    private float rotationSpeed = 16f;

    private CharacterController controller;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;

    private AudioSource audioSource;

    // Gravity Variables
    private float gravityValue = -9.8f;
    private float groundedGravity = -0.05f;                            

    // Jumping Variables
    [SerializeField]
    private float maxJumpHeight = 125f;
    [SerializeField]
    private float maxJumpTime = 1.5f;
    [SerializeField]
    private AudioClip jumpingSound;

    private bool isPlayerGrounded = true;
    private bool isJumping = false;
    private float initialJumpVelocity;                                 

    Vector3 jumpMovement;                                            

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        setupJumpVariables();
    }

    void Update()
    {
        detectGrounded();
        handleGravity();
        
        // get the value from the joystick
        Vector2 input = moveAction.ReadValue<Vector2>().normalized;
        
        // move the player
        Vector3 move = new Vector3(input.x, 0, input.y).normalized;
        if (isPlayerGrounded && !isJumping)
        {
            controller.Move(move * movingSpeed * Time.deltaTime);
        }

        // rotate the player
        if (!isJumping && isPlayerGrounded)
        {
            float degree = Mathf.Atan2(input.x, input.y) * (180 / Mathf.PI);
            Quaternion targetRotation = Quaternion.Euler(0, degree, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
         
        // press jump button
        if(jumpAction.triggered && !isJumping && isPlayerGrounded) {
            isJumping = true;
            jumpMovement.y = initialJumpVelocity;
            audioSource.PlayOneShot(jumpingSound); // play jump sound
        }

        controller.Move(jumpMovement);
    }


    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2 / Time.deltaTime;

        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        gravityValue = -initialJumpVelocity / timeToApex;
    }

    void detectGrounded() 
    {
        isPlayerGrounded = controller.isGrounded;
        if (isPlayerGrounded) isJumping = false;
    }
    void handleGravity()
    {
        if (isPlayerGrounded) jumpMovement.y = groundedGravity;
        else jumpMovement.y += gravityValue;
    }
}
