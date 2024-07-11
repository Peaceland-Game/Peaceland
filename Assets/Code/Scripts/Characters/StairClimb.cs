using Cinemachine.Utility;
using EasyRoads3Dv3;
using UnityEditor.EditorTools;
using UnityEngine;
/// <summary>
/// Helps the rigid body controller move up stairs
/// </summary>
public class StairClimb : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform lowerOrigin;
    [SerializeField] float raycastOriginHeight = 0.5f;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;
    [SerializeField] float raycastDistance = 0.5f;
    [SerializeField] LayerMask stepLayerMask;

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
        Vector3 lower = lowerOrigin.position; 
        Vector3 upperOrigin = lower + Vector3.up * stepHeight;

        Gizmos.DrawRay(lower, forward * raycastDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(upperOrigin, forward * raycastDistance);
        Gizmos.DrawRay(lower, Vector3.down * raycastDistance);
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
        Vector3 lower = lowerOrigin.position;
        Vector3 upperOrigin = lower + Vector3.up * stepHeight;

        if (Physics.Raycast(lower, forward, out RaycastHit hitLower, raycastDistance, stepLayerMask))
        {
            if (!Physics.Raycast(upperOrigin, forward, out RaycastHit hitUpper, raycastDistance, stepLayerMask))
            {
               // Debug.LogWarning($"Detected step by hitting object {hitLower.collider.name}");
                rigidBody.position += Vector3.up * stepSmooth * Time.deltaTime;
            }
        }
        /*
        if (Physics.Raycast(lower, Vector3.down, out hitLower, raycastDistance))
        {
            if (hitLower.collider.gameObject.layer == 6)
            {
                float normalizedX = playerBody.position.x / hitLower.collider.GetComponent<Terrain>().terrainData.size.x;
                float normalizedY = playerBody.position.z / hitLower.collider.GetComponent<Terrain>().terrainData.size.z;
                float steepness = hitLower.collider.GetComponent<Terrain>().terrainData.GetSteepness(normalizedX, normalizedY);
                
                if (steepness > 10.0f)
                {
                    Debug.Log(steepness + "   steep");
                    rigidBody.position -= 1 * Vector3.up * stepSmooth * Time.deltaTime;
                }
                else
                {
                    Debug.Log(steepness + "    not steep");
                }
            }
        }
        */

        // Check for 45 degree angles
        // CheckAngle(lower, upperOrigin, Quaternion.Euler(0, 45, 0) * forward);
        //  CheckAngle(lower, upperOrigin, Quaternion.Euler(0, -45, 0) * forward);
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
                Debug.Log("climb stairs");
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
}