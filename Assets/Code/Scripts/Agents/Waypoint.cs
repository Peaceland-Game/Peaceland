using UnityEngine;

/// <summary>
/// Simple class to show a red sphere gizmo at the objects location
/// </summary>
public class Waypoint : MonoBehaviour {
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 4f);
    }
}
