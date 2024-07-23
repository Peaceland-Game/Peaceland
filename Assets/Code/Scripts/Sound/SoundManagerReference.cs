using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Gives the player singleton and user interface object a reference to the sound manager
/// </summary>
public class SoundManagerReference : MonoBehaviour
{
    private UniversalSoundManager soundManager;
    private UserInterface ui;

    private void Start()
    {
        soundManager = GetComponent<UniversalSoundManager>();

        ui = FindObjectOfType<UserInterface>();

        if (PlayerSingleton.Instance)
        {
            PlayerSingleton.Instance.GetSoundManager(soundManager);
        }

        if (ui)
        {
            ui.GetSoundManager(soundManager);
        }
    }

    private void Update()
    {

    }
}

