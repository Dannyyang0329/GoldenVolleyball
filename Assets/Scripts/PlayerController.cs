using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    // Component
    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;

    // Input Variables
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction hitAction;

    public AudioManager audioManager;

    // Move Variables
    [SerializeField] private float movingSpeed = 150f;
    [SerializeField] private float rotationSpeed = 16f;

    // Gravity Variables
    private float gravityValue = -0.16f;
    private float groundedGravity = -0.05f;                            

    // Jumping Variables
    public AudioClip jumpingSound;
    [SerializeField] private float initialJumpVelocity;

    // hit strength
    [SerializeField] private float strength = 500;
    [SerializeField] Vector2 hitDirection;

    private bool isPlayerGrounded = true;
    private bool isJumping = false;
    
    Vector3 jumpMovement;

    // animator controller
    private bool jump;
    private bool run;
    private bool hit;
    private bool smash;

    // camera
    private GameObject cine_camera;

    // ball target
    private GameObject ball;

    private void Start() 
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        hitAction = playerInput.actions["Hit"];

        /*cine_camera = GameObject.Find("Camera");
        cine_camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform.GetChild(0);
        cine_camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = transform.GetChild(0);
        */

        ball = GameObject.FindGameObjectWithTag("Ball").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (IsLocalPlayer) {
            detectGrounded();
            handleGravity();
            if (isPlayerGrounded) jump = false;
            run = false;
            smash = false;
            hit = false;

            // get the value from the joystick
            Vector2 input = moveAction.ReadValue<Vector2>().normalized;
            if ((input.x != 0 || input.y != 0) && !isJumping) run = true;

            // move the player
            Vector3 move = new Vector3(input.x, 0, input.y).normalized;
            if (isPlayerGrounded && !isJumping) {
                controller.Move(move * movingSpeed * Time.deltaTime);
            }

            // rotate the player
            if (!isJumping && isPlayerGrounded) {
                float degree = Mathf.Atan2(input.x, input.y) * (180 / Mathf.PI);
                Quaternion targetRotation = Quaternion.Euler(0, degree, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // press jump button
            if (jumpAction.triggered && !isJumping && isPlayerGrounded) {
                isJumping = true;
                jumpMovement.y = initialJumpVelocity;
                audioManager.Play("Jump");
                jump = true;
            }
            /*
            // press hit button
            if (hitAction.triggered)
            {
                if (isJumping) smash = true;
                else hit = true;
            }
            */

            if (hitAction.WasPerformedThisFrame()) {
                hitDirection = hitAction.ReadValue<Vector2>();
            }

            if (hitAction.WasReleasedThisFrame()) {

                float ballx = ball.transform.position.x;
                float bally = ball.transform.position.y;
                float ballz = ball.transform.position.z;

                if (ballx > transform.position.x - 150 && ballx < transform.position.x + 150 &&
                    bally > transform.position.y - 150 && bally < transform.position.y + 150 &&
                    ballz > transform.position.z - 150 && ballz < transform.position.z + 150) {
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(strength * hitDirection.x, 300, strength * hitDirection.y);
                }
                if (isJumping) smash = true;
                else hit = true;
            }

            controller.Move(jumpMovement);
            animator.SetBool("isRun", run);
            animator.SetBool("isJump", jump);
            animator.SetBool("isSmash", smash);
            animator.SetBool("isHit", hit);
        }
    }

    /*
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2 / Time.deltaTime;

        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        gravityValue = -initialJumpVelocity / timeToApex;
    }
    */

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
