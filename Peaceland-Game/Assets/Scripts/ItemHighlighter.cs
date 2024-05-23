using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlighter : MonoBehaviour
{
    private const int PLAYER_INTERACT_LAYER = 6;
    [SerializeField]
    private GameObject[] keys = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PLAYER_INTERACT_LAYER)
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
