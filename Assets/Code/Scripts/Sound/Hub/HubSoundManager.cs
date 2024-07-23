using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HubSoundManager : UniversalSoundManager
{
    private GameObject controller;

    [Header("Hub Sounds")]

    [Space]

    [SerializeField] AudioClip incomingCall;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {

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

