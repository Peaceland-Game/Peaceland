using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    private const int PLAYER_INTERACT_LAYER = 6;
    [SerializeField]
    private GameObject indicator;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.layer == PLAYER_INTERACT_LAYER)
        {
            indicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PLAYER_INTERACT_LAYER)
        {
            indicator.SetActive(true);
        }
    }
}
