using UnityEngine;
/// <summary>
/// Helps the rigid body controller move up stairs
/// </summary>
public class StairClimb : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] Transform playerBody;
    [SerializeField] float raycastOriginHeight = 0.5f;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.3f;
    [SerializeField] float raycastDistance = 2f;

    private void Awake()
    {
        rigidBody = playerBody.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Draws rays to help position step height game objects
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!playerBody) return;

        Gizmos.color = Color.red;
        Vector3 forward = GetForwardDirection();
        Vector3 lowerOrigin = playerBody.position + Vector3.up * raycastOriginHeight;
        Vector3 upperOrigin = lowerOrigin + Vector3.up * stepHeight;

        Gizmos.DrawRay(lowerOrigin, forward * raycastDistance);
        Gizmos.DrawRay(upperOrigin, forward * raycastDistance);
    }

    private void FixedUpdate()
    {
        StepClimb();
    }

    /// <summary>
    /// Moves the rigid body up if the forward raycasts detect a step
    /// </summary>
    void StepClimb()
    {
        Vector3 forward = GetForwardDirection();
        Vector3 backward = GetBackwardDirection();
        Vector3 lowerOrigin = playerBody.position + Vector3.up * raycastOriginHeight;
        Vector3 upperOrigin = lowerOrigin + Vector3.up * stepHeight;

        if (Physics.Raycast(lowerOrigin, forward, out RaycastHit hitLowerFront, raycastDistance))
        {
            if (!Physics.Raycast(upperOrigin, forward, out RaycastHit hitUpperFront, raycastDistance))
            {
                rigidBody.position += Vector3.up * stepSmooth * Time.deltaTime / 2;
            }
        }

        if (Physics.Raycast(lowerOrigin, backward, out RaycastHit hitLowerBack, raycastDistance))
        {
            if (!Physics.Raycast(upperOrigin, backward, out RaycastHit hitUpperBack, raycastDistance))
            {
                rigidBody.position += Vector3.up * stepSmooth * Time.deltaTime / 2;
            }
        }

        // Check for 45 degree angles
        CheckAngle(lowerOrigin, upperOrigin, Quaternion.Euler(0, 45, 0) * forward);
        CheckAngle(lowerOrigin, upperOrigin, Quaternion.Euler(0, -45, 0) * forward);
    }

    /// <summary>
    /// check for step in the given direction 
    /// </summary>
    /// <param name="lowerOrigin">foot position</param>
    /// <param name="upperOrigin">highest step position</param>
    /// <param name="direction">direction to send the raycast</param>
    void CheckAngle(Vector3 lowerOrigin, Vector3 upperOrigin, Vector3 direction)
    {
        if (Physics.Raycast(lowerOrigin, direction, out RaycastHit hitLower, raycastDistance))
        {
            if (!Physics.Raycast(upperOrigin, direction, out RaycastHit hitUpper, raycastDistance))
            {
                rigidBody.position += Vector3.up * stepSmooth * Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// get a x/z direction vector of the forward direction of the player
    /// </summary>
    /// <returns></returns>
    Vector3 GetForwardDirection()
    {
        // Use camera's forward direction, but ignore pitch
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    Vector3 GetBackwardDirection()
    {
        Vector3 backward = transform.forward;
        backward.y = 0;
        backward.x *= -1;
        return backward.normalized;
    }
}