using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactIndicator;
    // Start is called before the first frame update
    void Start()
    {
        if (!interactIndicator) Debug.LogWarning("Iteract Indicator missing game object reference");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerReference.PLAYER)
        {
            interactIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerReference.PLAYER)
        {
            interactIndicator.SetActive(false);
        }
    }
}
