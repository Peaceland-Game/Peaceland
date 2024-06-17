//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Yarn.Unity;

//public class NewPlayerMove : MonoBehaviour
//{
//    [Header("Movement")]
//    private float moveSpeed;
//    public float walkSpeed;
//    public float sprintSpeed;

//    public float groundDrag;
//    public float jumpForce;
//    public float jumpCooldown;
//    public float airMultiplier;
//    bool readyToJump;

//    [Header("Crouching")]
//    public float crouchSpeed;
//    public float crouchYScale;
//    private float startYScale;

//    [Header("Ground Check")]
//    public float playerHeight;
//    public LayerMask whatIsGround;
//    bool grounded;

//    [Header("Wall Check")]
//    public float wallCheckDistance = 0.5f;
//    public LayerMask whatIsWall;
//    bool touchingWall;

//    [Header("Slope Handling")]
//    public float maxSlopeAngle;
//    private RaycastHit slopeHit;
//    private bool exitingSlope;

//    [Header("Interact")]
//    public float interactSphereRadius = 3.0f;
//    public LayerMask interactPlayerMask;

//    [Header("Camera Rotation")]
//    public GameObject playerCamHolder;
//    public Transform playerCam;
//    public Transform orientation;

//    [Space(10)]
//    Vector2 moveInput;
//    bool sprintInput;
//    bool crouchInput;
//    bool interactInput;
//    bool jumpInput;

//    Vector3 moveDirection;
//    Rigidbody rb;

//    public MovementState state;
//    public enum MovementState
//    {
//        Walking,
//        Sprinting,
//        Crouching,
//        Air,
//        Talking,
//        InMenu
//    }

//    private PlayerControls playerControls;

//    private void Awake()
//    {
//        playerControls = new PlayerControls();

//        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

//        playerControls.Player.Jump.performed += ctx => jumpInput = true;
//        playerControls.Player.Jump.canceled += ctx => jumpInput = false;

//        playerControls.Player.Sprint.performed += ctx => sprintInput = true;
//        playerControls.Player.Sprint.canceled += ctx => sprintInput = false;

//        playerControls.Player.Crouch.performed += ctx => crouchInput = true;
//        playerControls.Player.Crouch.canceled += ctx => crouchInput = false;

//        playerControls.Player.Interact.performed += ctx => interactInput = true;
//        playerControls.Player.Interact.canceled += ctx => interactInput = false;
//    }

//    private void OnEnable()
//    {
//        playerControls.Enable();
//    }

//    private void OnDisable()
//    {
//        playerControls.Disable();
//    }

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;

//        readyToJump = true;

//        startYScale = transform.localScale.y;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    private void Update()
//    {
//        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
//        touchingWall = Physics.Raycast(transform.position, orientation.forward, wallCheckDistance, whatIsWall);

//        SpeedControl();
//        StateHandler();

//        if (grounded)
//            rb.drag = groundDrag;
//        else
//            rb.drag = 0;

//        if (jumpInput && readyToJump && grounded && !touchingWall)
//        {
//            readyToJump = false;
//            Jump();
//            Invoke(nameof(ResetJump), jumpCooldown);
//        }

//        if (crouchInput)
//        {
//            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
//        }
//        else
//        {
//            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
//        }

//        if (interactInput)
//        {
//            var closestObject = GetClosest3DObjectOnLayers(interactPlayerMask);
//            Transform whereToLook = null;
//            if (closestObject)
//            {
//                if (closestObject.TryGetComponent(out HeadTurnGetter headTurner))
//                {
//                    whereToLook = headTurner.ObjectToLookAt;
//                }
//                else if (closestObject.CompareTag("InteractChar"))
//                {
//                    whereToLook = closestObject.transform;
//                }
//            }

//            if (whereToLook)
//            {
//                state = MovementState.Talking;
//                Debug.Log($"Turning to look at? {whereToLook.name}");
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        MovePlayer();
//    }

//    public void OnConversationStart(Transform actor)
//    {
//        Debug.Log($"Starting conversation with {actor.name}");
//        Cursor.lockState = CursorLockMode.None;
//        state = MovementState.Talking;
//    }

//    public IEnumerator TurnToLookAt(Transform target, float duration)
//    {
//        Quaternion initialRotation = playerCamHolder.transform.rotation;
//        Quaternion finalRotation = Quaternion.LookRotation(target.position - transform.position);
//        float elapsedTime = 0f;

//        while (elapsedTime < duration)
//        {
//            playerCamHolder.transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / duration);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        playerCamHolder.transform.rotation = finalRotation;
//    }

//    public void OnConversationEnd(Transform actor)
//    {
//        Debug.Log($"stopped talking to: {actor.name}");
//        Cursor.lockState = CursorLockMode.Locked;
//        state = MovementState.Walking;
//    }

//    public GameObject GetClosest3DObjectOnLayers(LayerMask layers)
//    {
//        Collider[] interactableColliders = Physics.OverlapSphere(transform.position, interactSphereRadius, layers);
//        return GetClosest3DObjectInColliderArray(interactableColliders);
//    }

//    private GameObject GetClosest3DObjectInColliderArray(List<Collider> interactableColliders)
//    {
//        return GetClosest3DObjectInColliderArray(interactableColliders.ToArray());
//    }

//    private GameObject GetClosest3DObjectInColliderArray(Collider[] colliders)
//    {
//        if (colliders.Length == 0) return null;
//        if (colliders.Length == 1) return colliders[0].gameObject;

//        GameObject closestObject = null;
//        float smallestOrthogonalDistance = float.MaxValue;
//        Ray cameraray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

//        foreach (var collider in colliders)
//        {
//            Vector3 closestpointonray = cameraray.GetPoint(Vector3.Dot(collider.transform.position - cameraray.origin, cameraray.direction));
//            float orthogonaldistance = Vector3.Distance(collider.transform.position, closestpointonray);

//            if (orthogonaldistance < smallestOrthogonalDistance)
//            {
//                Vector3 viewportpos = Camera.main.WorldToViewportPoint(collider.transform.position);
//                bool isonscreen = viewportpos.x >= 0 && viewportpos.x <= 1 && viewportpos.y >= 0 && viewportpos.y <= 1;

//                if (isonscreen)
//                {
//                    smallestOrthogonalDistance = orthogonaldistance;
//                    closestObject = collider.gameObject;
//                }
//            }
//        }

//        return closestObject;
//    }

//    private void StateHandler()
//    {
//        if (grounded && sprintInput)
//        {
//            state = MovementState.Sprinting;
//            moveSpeed = sprintSpeed;
//        }
//        else if (grounded)
//        {
//            state = MovementState.Walking;
//            moveSpeed = walkSpeed;
//        }
//        else if (grounded && crouchInput)
//        {
//            state = MovementState.Crouching;
//            moveSpeed = crouchSpeed;
//        }
//        else
//        {
//            state = MovementState.Air;
//        }
//    }

//    private void MovePlayer()
//    {
//        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

//        if (OnSlope() && !exitingSlope)
//        {
//            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
//            if (rb.velocity.y > 0)
//            {
//                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
//            }
//        }
//        else if (grounded)
//        {
//            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
//        }
//        else if (!grounded)
//        {
//            if (touchingWall)
//            {
//                Vector3 wallNormal = slopeHit.normal;
//                Vector3 perpendicularDirection = Vector3.ProjectOnPlane(moveDirection, wallNormal);
//                rb.AddForce(perpendicularDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
//            }
//            else
//            {
//                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
//            }
//        }

//        rb.useGravity = !OnSlope();
//    }

//    private void SpeedControl()
//    {
//        if (OnSlope() && !exitingSlope)
//        {
//            if (rb.velocity.magnitude > moveSpeed)
//            {
//                rb.velocity = rb.velocity.normalized * moveSpeed;
//            }
//        }
//        else
//        {
//            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//            if (flatVel.magnitude > moveSpeed)
//            {
//                Vector3 limitedVel = flatVel.normalized * moveSpeed;
//                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
//            }
//        }
//    }

//    private void Jump()
//    {
//        if (state == MovementState.Talking || state == MovementState.InMenu)
//        {
//            return;
//        }

//        exitingSlope =true;
//rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
//rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
//}
//private void ResetJump()
//{
//    readyToJump = true;
//    exitingSlope = false;
//}

//private bool OnSlope()
//{
//    if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
//    {
//        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
//        return angle < maxSlopeAngle && angle != 0;
//    }
//    return false;
//}

//private Vector3 GetSlopeMoveDirection()
//{
//    return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
//}
//}
