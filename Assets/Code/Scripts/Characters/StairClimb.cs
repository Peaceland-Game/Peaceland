using UnityEngine;

public class StairClimb : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] Transform playerBody;
    [SerializeField] float raycastOriginHeight = 0.5f;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;
    [SerializeField] float raycastDistance = 0.5f;

    private void Awake()
    {
        rigidBody = playerBody.GetComponent<Rigidbody>();
    }

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

    void StepClimb()
    {
        Vector3 forward = GetForwardDirection();
        Vector3 lowerOrigin = playerBody.position + Vector3.up * raycastOriginHeight;
        Vector3 upperOrigin = lowerOrigin + Vector3.up * stepHeight;

        if (Physics.Raycast(lowerOrigin, forward, out RaycastHit hitLower, raycastDistance))
        {
            if (!Physics.Raycast(upperOrigin, forward, out RaycastHit hitUpper, raycastDistance))
            {
                rigidBody.position += Vector3.up * stepSmooth * Time.deltaTime;
            }
        }

        // Check for 45 degree angles
        CheckAngle(lowerOrigin, upperOrigin, Quaternion.Euler(0, 45, 0) * forward);
        CheckAngle(lowerOrigin, upperOrigin, Quaternion.Euler(0, -45, 0) * forward);
    }

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

    Vector3 GetForwardDirection()
    {
        // Use camera's forward direction, but ignore pitch
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
}