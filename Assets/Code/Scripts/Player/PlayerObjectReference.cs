using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Helper class to reduce GetComponent calls when loading a new scene
/// this is used to get the required references in the player singleton from the player object
/// </summary>
public class PlayerObjectReference : MonoBehaviour
{
    public Tablet tablet;
    public UserInterface userInterface;
    public FirstPersonController controller;
    public Selector selector;
    private bool infoSent = false;

   
    private void Start()
    {
        if (PlayerSingleton.Instance)
        {
            PlayerSingleton.Instance.InitPlayer(this);
        }
    }
    /// <summary>
    /// Will try and find the player singleton and send the references to it until it succeeds
    /// </summary>
    private void Update()
    {
        if (!infoSent)
        {
            if (PlayerSingleton.Instance)
            {
                infoSent = true;    
                PlayerSingleton.Instance.InitPlayer(this);
            }
        }
    }
}
