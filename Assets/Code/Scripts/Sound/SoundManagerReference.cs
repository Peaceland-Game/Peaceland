using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Helper class to reduce GetComponent calls when loading a new scene
/// Gives the player singleton a reference to the sound manager
/// </summary>
public class SoundManagerReference : MonoBehaviour
{
    private UniversalSoundManager soundManager;

    private void Start()
    {
        soundManager = GetComponent<UniversalSoundManager>();

        if (PlayerSingleton.Instance)
        {
            PlayerSingleton.Instance.GetSoundManager(soundManager);
        }
    }

    private void Update()
    {
        
    }
}
