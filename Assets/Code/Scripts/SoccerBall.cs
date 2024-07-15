using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    [SerializeField] Vector2 hitForceRange;
    [SerializeField] Vector2 vertForceRange;
    private Rigidbody rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnUse(Transform player)
    {
        // Direction horizontal to ground 
        Vector3 dir = Vector3.ProjectOnPlane((this.transform.position - player.position).normalized, Vector3.up);

        rb.AddForce(
            dir * Random.Range(hitForceRange.x, hitForceRange.y) + 
            Vector3.up * Random.Range(vertForceRange.x, vertForceRange.y),
            ForceMode.Impulse);
    }
}
