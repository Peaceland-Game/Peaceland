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
    private IntroController introController;

    private void Start()
    {
        soundManager = GetComponent<UniversalSoundManager>();
        ui = FindObjectOfType<UserInterface>();
        introController = FindObjectOfType<IntroController>();

        if (PlayerSingleton.Instance)
        {
            PlayerSingleton.Instance.GetSoundManager(soundManager);
        }

        if (ui)
        {
            ui.GetSoundManager(soundManager);
        }

        if(introController)
        {
            introController.GetSoundManager(soundManager);
        }
    }

    private void Update()
    {

    }
}

