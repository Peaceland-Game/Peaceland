using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] // Header for organizing movement-related variables in the Unity Inspector
    private float moveSpeed; // Speed at which the player moves
    public float walkSpeed; // Speed when walking
    public float sprintSpeed; // Speed when sprinting

    public float groundDrag; // Drag applied when the player is on the ground
    public float jumpForce; // Force applied when the player jumps
    public float jumpCooldown; // Cooldown time between jumps
    public float airMultiplier; // Multiplier for movement speed when the player is in the air
    bool readyToJump; // Flag indicating whether the player is ready to jump or not

    [Header("Crouching")] // Header for organizing crouch-related variables in the Unity Inspector
    public float crouchSpeed; // Speed when crouching
    public float crouchYScale; // Y-scale of the player when crouching
    private float startYScale; // Initial Y-scale of the player

    [Header("Keybinds")] // Header for organizing keybind variables in the Unity Inspector
    public KeyCode jumpKey = KeyCode.Space; // Key used for jumping
    public KeyCode sprintKey = KeyCode.LeftShift; // Key used for sprinting
    public KeyCode crouchKey = KeyCode.LeftControl; // Key used for crouching
    public KeyCode interactKey = KeyCode.E; // Key used for interacting with specfic objects

    [Header("Ground Check")] // Header for organizing ground check variables in the Unity Inspector
    public float playerHeight; // Height of the player
    public LayerMask whatIsGround; // Layer mask for identifying ground objects
    bool grounded; // Flag indicating whether the player is grounded or not

    [Header("Wall Check")] // Header for organizing wall check variables in the Unity Inspector
    public float wallCheckDistance = 0.5f; // Distance for wall check
    public LayerMask whatIsWall; // Layer mask for identifying wall objects
    bool touchingWall; // Flag indicating whether the player is touching a wall or not

    [Header("Slope Handling")] // Header for organizing slope handling variables in the Unity Inspector
    public float maxSlopeAngle; // Maximum angle for the player to walk on a slope
    private RaycastHit slopeHit; // Information about the slope the player is on
    private bool exitingSlope; // Flag indicating whether the player is exiting a slope

    [Header("Interact")] // Header for organizing interact variables in the Unity Inspector
    public float interactSphereRadius = 3.0f; // Size of the player's interact range
    public LayerMask interactPlayerMask; // The layer mask which all interactable objects are on
                                         // public DialogueRunner dialogueRunner;

    [Header("Camera Rotation")]
    public GameObject playerCamHolder;
    public Transform playerCam;
    public Transform orientation; // Orientation of the player


    [Space(10)]
    float horizontalInput; // Input for horizontal movement
    float verticalInput; // Input for vertical movement

    Vector3 moveDirection; // Direction of movement

    Rigidbody rb; // Rigidbody component of the player

    public MovementState state; // Current movement state of the player
    public enum MovementState {
        Walking, // Walking state
        Sprinting, // Sprinting state
        Crouching, // Crouching state
        Air, // Air state
        Talking, // Talking state
        InMenu
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Getting the Rigidbody component of the player
        rb.freezeRotation = true; // Freezing rotation of the Rigidbody

        readyToJump = true; // Setting the initial jump state to ready

        startYScale = transform.localScale.y; // Storing the initial Y-scale
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        
        // Checking if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        // Checking if the player is touching a wall
        touchingWall = Physics.Raycast(transform.position, orientation.forward, wallCheckDistance, whatIsWall);

        // Processing player input
        MyInput();

        // Controlling speed based on player input
        SpeedControl();

        // Handling different movement states
        StateHandler();

        // Setting drag based on whether the player is grounded or not
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        // Moving the player
        MovePlayer();
    }
    public void OnConversationStart(Transform actor)
    {
        Debug.Log($"Starting conversation with {actor.name}");
        Cursor.lockState = CursorLockMode.None;
        state = MovementState.Talking;
    }
    

    private void MyInput()
    {
        // Getting horizontal and vertical input from the player
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Checking if jump key is pressed and conditions are met to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded && !touchingWall)
        {
            readyToJump = false; // Preventing consecutive jumps

            Jump(); // Calling jump function

            Invoke(nameof(ResetJump), jumpCooldown); // Resetting jump after cooldown
        }

        // Handling crouch input
        if (Input.GetKeyDown(crouchKey))
        {
            // Changing Y-scale to crouch
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        // Resetting Y-scale when crouch key is released
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        // Handling interact input
        if (Input.GetKeyDown(interactKey))
        {
            var closestObject = GetClosest3DObjectOnLayers(interactPlayerMask);
            Transform whereToLook = null;
            if (closestObject)
            {
                if (closestObject.TryGetComponent(out HeadTurnGetter headTurner))
                {
                    whereToLook = headTurner.ObjectToLookAt;
                }
                else if (closestObject.CompareTag("InteractChar"))
                {
                    whereToLook = closestObject.transform;
                }
            }

            if (whereToLook)
            {
                
                state = MovementState.Talking;

                // var angles = playerCamHolder.transform.rotation.eulerAngles;
                // transform.SetPositionAndRotation(transform.position, Quaternion.Euler(angles.x, 0, angles.z));
                // playerCamHolder.transform.localRotation = Quaternion.identity;
                Debug.Log($"Turning to look at {whereToLook.name}");
                StartCoroutine(TurnToLookAt(whereToLook.transform, 1.0f));
            }

        }
    }

    public IEnumerator TurnToLookAt(Transform target, float duration)
    {
        // Store the initial rotation of the CameraHolder
        Quaternion initialRotation = playerCamHolder.transform.rotation;

        // Calculate the final rotation to look at the target
        Quaternion finalRotation = Quaternion.LookRotation(target.position - transform.position);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate between the initial rotation and the final rotation
            playerCamHolder.transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }

        // Ensure the camera ends at the exact final rotation
        playerCamHolder.transform.rotation = finalRotation;

        //target.GetComponent<YarnInteractable>().StartConversation();
        
        //dialogueRunner.onDialogueComplete.AddListener(StopTalking);
    }

    public void OnConversationEnd(Transform actor)
    {
        Debug.Log($"stopped talking to: {actor.name}");
        Cursor.lockState = CursorLockMode.Locked;
        state = MovementState.Walking;
    }

    public GameObject GetClosest3DObjectOnLayers(LayerMask layers)
    {
        // Perform the overlap sphere and get the colliders within the specified radius.
        Collider[] interactableColliders = Physics.OverlapSphere(transform.position, interactSphereRadius, layers);

        return GetClosest3DObjectInColliderArray(interactableColliders);
    }

    private GameObject GetClosest3DObjectInColliderArray(List<Collider> interactableColliders)
    {
        return GetClosest3DObjectInColliderArray(interactableColliders.ToArray());
    }
    private GameObject GetClosest3DObjectInColliderArray(Collider[] colliders)
    {
        if (colliders.Length == 0)
            return null;
        if (colliders.Length == 1)
            return colliders[0].gameObject;
        // Initialize variables to keep track of the closest object.
        GameObject closestObject = null;
        float smallestOrthogonalDistance = float.MaxValue;
        Ray cameraray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        foreach (var collider in colliders)
        {
            // get the closest point on the camera ray to the object's position.
            Vector3 closestpointonray = cameraray.GetPoint(Vector3.Dot(collider.transform.position - cameraray.origin, cameraray.direction));
            // calculate the orthogonal distance from the object to the ray.
            float orthogonaldistance = Vector3.Distance(collider.transform.position, closestpointonray);

            // check if this collider is closer to the camera's forward direction than the previous ones.
            if (orthogonaldistance < smallestOrthogonalDistance)
            {
                Vector3 viewportpos = Camera.main.WorldToViewportPoint(collider.transform.position);

                // check if the object is within the viewport bounds
                bool isonscreen = viewportpos.x >= 0 && viewportpos.x <= 1 && viewportpos.y >= 0 && viewportpos.y <= 1;

                //allow picking up items in 90 degree cone in front of camera

                if (isonscreen)
                {
                    smallestOrthogonalDistance = orthogonaldistance;
                    closestObject = collider.gameObject;
                }
            }
        }

        // return the closest interactable object or null if none was found.
        return closestObject;
    }

    private void StateHandler()
    {
        //if(state == MovementState.Talking || state == MovementState.InMenu) 
        //{
        //    return;
        //}

        // Handling sprinting state
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.Sprinting;
            moveSpeed = sprintSpeed;
        }
        // Handling walking state
        else if (grounded)
        {
            state = MovementState.Walking;
            moveSpeed = walkSpeed;
        }
        // Handling crouching state
        else if (grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.Crouching;
            moveSpeed = crouchSpeed;
        }
        // Handling air state
        else
        {
            state = MovementState.Air;
        }
    }

    private void MovePlayer()
    {
        //if (state == MovementState.Talking || state == MovementState.InMenu)
        //{
        //    return;
        //}

        // Calculating move direction based on player orientation and input
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Handling movement on slopes
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // Applying force to move the player on the ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // Applying force to move the player in the air
    else if (!grounded)
    {
        // Check if the player is touching a wall
        if (touchingWall)
        {
            // Handle wall sliding or stopping movement
            Vector3 wallNormal = slopeHit.normal; // Normal of the wall the player is touching
            Vector3 perpendicularDirection = Vector3.ProjectOnPlane(moveDirection, wallNormal); // Move direction perpendicular to the wall

            // Applying force to move the player perpendicular to the wall
            rb.AddForce(perpendicularDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        else
        {
            // If not touching a wall, apply normal air movement
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // Limiting speed on slopes
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        // Limiting speed on flat surfaces
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limiting velocity
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed; // Limiting velocity to moveSpeed
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Applying limited velocity
            }
        }
    }

    private void Jump()
    {
        if (state == MovementState.Talking || state == MovementState.InMenu)
        {
            return;
        }

        exitingSlope = true; // Setting exiting slope flag
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zeroing out vertical velocity

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Applying upward force for jump
    }

    private void ResetJump()
    {
        readyToJump = true; // Resetting jump flag
        exitingSlope = false; // Resetting exiting slope flag
    }

    private bool OnSlope()
    {
        // Raycast downward from the player's position to check for slopes
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            // Calculate the angle between the up vector and the normal of the slope
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            // Return true if the angle is less than the max slope angle and not zero (indicating a slope)
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        // Project the move direction onto the plane defined by the slope's normal
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}



