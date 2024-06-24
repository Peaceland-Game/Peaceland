using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePole : MonoBehaviour
{
    [SerializeField] public List<WirePole> connections;
    [SerializeField] Vector3 connectionpoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position + connectionpoint, 0.1f);
    }
}
