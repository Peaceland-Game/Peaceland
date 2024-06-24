using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleWireManager : MonoBehaviour
{
    [SerializeField] List<WirePole> wirePoles;
    [SerializeField] LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        UpdateWires();
    }

    private void UpdateWires()
    {
        Vector3[] positions;
        int connectionCount = 0;

        // Get count 
        for (int i = 0; i < wirePoles.Count; ++i)
        {
            // One point for wirepole and one for its connection 
            connectionCount += wirePoles[i].connections.Count * 2;
        }
        positions = new Vector3[connectionCount];

        // Set wire positions 
        for (int i = 0; i < wirePoles.Count; i++)
        {
            
        }
    }

    private void SetUpPole(WirePole wirePole)
    {
        for(int i = 0;i < wirePole.connections.Count; i++)
        {
            //lineRenderer.SetPosition
        }
    }
}
