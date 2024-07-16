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
}
