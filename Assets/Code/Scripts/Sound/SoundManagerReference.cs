using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Gives a reference to the sound manager to objects that need it
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

        //Debug.Log($"Sending sound manager reference: {soundManager != null}");

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

