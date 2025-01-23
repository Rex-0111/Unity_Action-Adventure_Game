using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    //Script Calling
    IAA_Player inputActions;

    //Basic Variables
    public float Speed = 5f;
    [SerializeField] Transform CameraTransform;
    CharacterController characterController;
    Animator animator;
    public bool canMove;
    Collider Collider;

    // Animator Parameter's Id
    int SpeedId;
    int FallingId;

    // Speed Delay
    float targetspeed;
    float currentVelocity;
    float smoothTime;

    private void Awake()
    {
        canMove = true;
        smoothTime = 7f;
        inputActions = new IAA_Player();
        animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void Start()
    {
        AnimationID();
    }

    private void AnimationID()
    {
        SpeedId = Animator.StringToHash("Speed");
        FallingId = Animator.StringToHash("Falling");
    }

    void FixedUpdate()
    {
        characterController = GetComponent<CharacterController>();
        if (canMove)
        {
            Movement();
        }
        Falling();
    }

    private void Falling()
    {
        if (!characterController.isGrounded)
        {
            animator.SetBool(FallingId, true);
        }
        if (characterController.isGrounded)
        {
            animator.SetBool(FallingId, false);
        }
    }

    void Movement()
    {
        // Get the camera's transform
        Transform cameraTransform = Camera.main.transform;

        // Read input
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        // Calculate the forward and right direction relative to the camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // We want to ignore the vertical component of the camera direction
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction relative to the camera
        Vector3 moveDirection = (cameraForward * input.y + cameraRight * input.x).normalized;

        // If there's movement input
        if (moveDirection != Vector3.zero)
        {
            // Calculate target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Smoothly rotate the character
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Move the character
        if(Collider.enabled == true)
        {
            characterController.SimpleMove(moveDirection * Speed);
        }


        // Handle idle, run, and sprint states
        IdleRunSprint(input);
    }


    private void IdleRunSprint(Vector2 input)
    {
        targetspeed = Mathf.SmoothDamp(targetspeed, Speed, ref currentVelocity, Time.deltaTime * smoothTime);
        float walk = inputActions.Player.Walk.ReadValue<float>();
        if (input == Vector2.zero)
        {
            Speed = 0;
        }
        else if (input != Vector2.zero && walk == 1f) { Speed = 2; }
        else if (input != Vector2.zero && walk == 0f)
        {
            float sprintValue = inputActions.Player.Sprint.ReadValue<float>();
            Speed = sprintValue == 0f ? 4 : 7;
        }
        animator.SetFloat(SpeedId, targetspeed);
    }
    
}

