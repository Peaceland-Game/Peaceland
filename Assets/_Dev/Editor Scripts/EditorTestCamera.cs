using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorTestCamera : MonoBehaviour
{

    public GameObject player;
    public GameObject flyCam;
    public bool playerEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if (UNITY_EDITOR)
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            playerEnabled = !playerEnabled;
            flyCam.SetActive(!playerEnabled);
            player.SetActive(playerEnabled);
        }
#endif
    }
}
