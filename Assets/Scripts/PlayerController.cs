using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;
using MLAPI.Messaging;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    public bool isRev = false;

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

    // Gravity Variables
    private float gravityValue = -6f;
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
            if (isRev) input = -input;
            if ((input.x != 0 || input.y != 0) && !isJumping) run = true;

            // move the player
            Vector3 move = new Vector3(input.x, 0, input.y).normalized;
            controller.Move(move * movingSpeed * Time.deltaTime);

            // rotate the player
            if (!isJumping && isPlayerGrounded) {
                float degree = Mathf.Atan2(input.x, input.y) * (180 / Mathf.PI);
                Quaternion targetRotation = Quaternion.Euler(0, degree, 0);
                transform.rotation = targetRotation;
            }

            // press jump button
            if (jumpAction.triggered && !isJumping && isPlayerGrounded) {
                isJumping = true;
                jumpMovement.y = initialJumpVelocity;
                audioManager.Play("Jump");
                jump = true;
            }

            if (hitAction.WasPerformedThisFrame()) {
                hitDirection = hitAction.ReadValue<Vector2>();
                if (isRev) hitDirection = -hitDirection;
            }

            if (hitAction.WasReleasedThisFrame()) {
                if (IsHost) {
                    PlayerHit(hitDirection);
                }
                else {
                    HitServerRpc(hitDirection);
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


    [ServerRpc]
    void HitServerRpc(Vector2 inputDir) 
    {
        PlayerHit(inputDir);
    }

    void PlayerHit(Vector2 inputDir) {
        float ballx = ball.transform.position.x;
        float bally = ball.transform.position.y;
        float ballz = ball.transform.position.z;
        float distance = Mathf.Sqrt(Mathf.Pow(inputDir.x,2)+Mathf.Pow(inputDir.y,2));

        float curX = transform.position.x;
        float curY = transform.position.y;
        float curZ = transform.position.z;

        if (ballx > curX - 150 && ballx < curX + 150 &&
            bally > curY - 150 && bally < curY + 150 &&
            ballz > curZ - 180 && ballz < curZ + 180 &&
            ball.GetComponent<BallController>().canHit)
        {
            audioManager.Play("Hit");
            if (!isJumping)
            {
                Vector3 newVelocity = new Vector3(strength * inputDir.x, 300 * distance, strength * inputDir.y);
                ball.GetComponent<Rigidbody>().velocity = newVelocity;
                ball.GetComponent<BallController>().setStart();
                ball.GetComponent<BallController>().beenHit = true;
            }
            else
            {
                Vector3 newVelocity = new Vector3(2f * strength * inputDir.x, -500 * distance, 2f * strength * inputDir.y);
                ball.GetComponent<Rigidbody>().velocity = newVelocity;
            }
        }
    }
}
