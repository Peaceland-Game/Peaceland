using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlighter : MonoBehaviour
{
    private const int PLAYER_INTERACT_LAYER = 6;
    [SerializeField]
    private GameObject interactSprite;

    [SerializeField]
    private Transform playerTransform;

    SpriteRenderer indicateSprite;

    // Start is called before the first frame update
    void Start()
    {
        indicateSprite = GetComponent<SpriteRenderer>();
        indicateSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        interactSprite.transform.LookAt(playerTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.layer == PLAYER_INTERACT_LAYER)
        {
            indicateSprite.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PLAYER_INTERACT_LAYER)
        {
            indicateSprite.enabled = false;
        }
    }
}
