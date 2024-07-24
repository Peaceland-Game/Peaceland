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
    //private DialogueSystemController dialogueSystemController;

    private void Awake()
    {
        Debug.Log("Sending sound manager references...");

        soundManager = GetComponent<UniversalSoundManager>();
        ui = FindObjectOfType<UserInterface>();
        introController = FindObjectOfType<IntroController>();
        //dialogueSystemController = FindObjectOfType<DialogueSystemController>();

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

        //if(dialogueSystemController)
        //{
        //    dialogueSystemController.
        //}
    }

    private void Update()
    {
        
    }
}

