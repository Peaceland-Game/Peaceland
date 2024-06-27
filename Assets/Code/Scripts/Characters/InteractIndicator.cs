using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactIndicator;
    [SerializeField] private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        if (!interactIndicator) Debug.LogWarning("Interact Indicator missing game object reference");
        if (!outline) Debug.LogWarning("Outline Indicator missing game object reference");
        else
        {
            outline.OutlineColor = Color.black;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerReference.PLAYER)
        {
            //if (interactIndicator)
            //{
            //    interactIndicator.SetActive(true);
            //}
            if (outline)
            {
                outline.OutlineColor = Color.yellow;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerReference.PLAYER)
        {
            //if (!interactIndicator)
            //{
            //    interactIndicator.SetActive(false);
            //}
            if (outline)
            {
                outline.OutlineColor = Color.black;
            }
        }
    }
    
}
