using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10.0f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 1.0f, maxAirAcceleration = 1.0f;

    [SerializeField]
    float jumpHeight = 5.0f;

    private Vector3 desiredVel;
    private Vector3 vel;

    private bool desiredJump;
    private bool onGround;

    Rigidbody rb;
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        desiredVel = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        desiredVel = Vector3.ClampMagnitude(desiredVel, 1.0f) * maxSpeed;

        desiredJump |= Input.GetButtonDown("Jump") && onGround;
    }

    private void FixedUpdate()
    {
        vel = rb.velocity;
        // Normal Movement
        float maxChangeSpeed = (onGround ? maxAcceleration : maxAirAcceleration) * Time.deltaTime;

        vel.x =
            Mathf.MoveTowards(vel.x, desiredVel.x, maxChangeSpeed);
        vel.z =
            Mathf.MoveTowards(vel.z, desiredVel.z, maxChangeSpeed);
        
        // Jumping 
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        // Send to rigidbody 
        rb.velocity = vel;


        onGround = false;
    }

    void Jump()
    {
        // Change displacement to a velocity 
        vel.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
    }


    private void Displace(Vector2 displacement)
    {
        Vector3 target = this.transform.position + new Vector3(displacement.x, 0.0f, displacement.y); ;
        this.transform.position = target;
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollisions(collision);
    }

    private void EvaluateCollisions(Collision collision)
    {
        for(int i = 0; i < collision.contactCount; i++) 
        {
            onGround |= collision.contacts[i].normal.y >= 0.9f;
        }
    }
}
