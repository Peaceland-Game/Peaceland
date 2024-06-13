using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")] // Header for organizing movement-related variables in the Unity Inspector
    public float moveSpeed; // Speed at which the player moves
    public float walkSpeed; // Speed when walking
    public float sprintSpeed; // Speed when sprinting

    bool isSprinting;

    public Rigidbody rb; // Rigidbody component of the player

   public Vector2 horizontalInput;

    [SerializeField] float gravity = -30f; // -9.81
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    [SerializeField] float jumpHeight = 0;
    [SerializeField] Transform feetPosition;
    // bool keeping track of if the player is jumping.
    bool jump;

    // keeps track of whether playing is crouching or not
    bool isCrouching;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = walkSpeed;
        jump = false;
        isCrouching = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(feetPosition.position, 0.1f, groundMask);
        if (isGrounded)
        {
            //verticalVelocity.y = 0;
            Debug.Log("is grouended");
        }
        // changes move speed if player is sprinting
        if (isSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        } 
        
        if (isCrouching)
        {
            moveSpeed = walkSpeed/2;
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            rb.AddForce(transform.up * -1 *jumpHeight, ForceMode.Impulse);
        }
        else
        {

            transform.localScale = new Vector3(transform.localScale.x, 0.75f, transform.localScale.z);
        }
        
        //gets horizontal velocity from keyboard inputs and then applies it to the rigidbody
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * moveSpeed;
        rb.MovePosition(transform.position + horizontalVelocity * Time.fixedDeltaTime);

        if (jump && isGrounded)
        {
            verticalVelocity = transform.up * jumpHeight;
            rb.AddForce(verticalVelocity, ForceMode.Impulse);
            jump = false;
        }

       
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        //Debug.Log("attempting");
        horizontalInput = _horizontalInput;
    }

    public void OnSprintPressed(bool sprintState)
    {
        isSprinting = sprintState;
    }

    public void OnJumpPress()
    {
        jump = true;
    }

    public void OnCrouchPress(bool crouchState)
    {
        isCrouching = crouchState;
    }
}

