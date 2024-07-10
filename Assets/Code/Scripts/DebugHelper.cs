using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    [SerializeField] GameObject freeCam;

    private GameObject player;
    private GameObject freeCamObj;
    private bool inDebugMode = false;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<FirstPersonController>().gameObject;
    }


    void Update()
    {
        ToggleDebugMode();
    }

    /// <summary>
    /// Whether or not to set as free cam 
    /// </summary>
    private void ToggleDebugMode()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            inDebugMode = !inDebugMode;
            player.SetActive(!inDebugMode);

            if (inDebugMode)
            {
                // Spawn free cam 
                freeCamObj = Instantiate(freeCam, player.transform.position, player.transform.rotation);
            }
            else
            {
                // Teleport player character to free cam position 
                player.transform.position = freeCamObj.transform.position;

                // Delete free cam 
                Destroy(freeCamObj);
            }
        }
    }
}
