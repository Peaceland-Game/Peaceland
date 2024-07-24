using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSoundManager : UniversalSoundManager
{
    private GameObject controller;

    [Header("Intro Sounds")]

    [Space]

    [SerializeField] AudioClip incomingCall;

    // Start is called before the first frame update
    protected override void Awake()
    {
        GetUniversalSoundSources();

        List<GameObject> gameObjects = FindObjectsOfType<GameObject>(true).ToList();

        foreach (GameObject a in gameObjects) 
        { 
            if(a.name == "IntroController")
            {
                controller = a;
                break;
            }
        }
    }

    public void IncomingCall()
    {
        PlaySound(player, incomingCall);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Lua.RegisterFunction(nameof(IncomingCall), this, SymbolExtensions.GetMethodInfo(() => IncomingCall()));
    }
}

