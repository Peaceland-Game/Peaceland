using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlighter : MonoBehaviour
{
    //private const int PLAYER_INTERACT_LAYER = 6;

    //[SerializeField]
    //private GameObject interactSprite;

    [SerializeField]
    private Transform playerTransform;

    //SpriteRenderer indicateSprite;

    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
        Debug.Log(originalColor);
        //indicateSprite = GetComponent<SpriteRenderer>();
        //indicateSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //interactSprite.transform.LookAt(playerTransform);
    }

    public void OnSelect()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void OnDeselect()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }
}
