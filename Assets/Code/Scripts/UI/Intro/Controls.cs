using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used for player controls during the authorization form at the beginning of the game
/// </summary>
public class Controls : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlayerSingleton.Instance.EnableMovement();
            PlayerSingleton.Instance.playerInMemorySelection = false;
            this.gameObject.SetActive(false);
        }
    }
}
