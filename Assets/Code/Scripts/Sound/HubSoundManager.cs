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

    protected override void Awake()
    {
        GetUniversalSoundSources();

        //List<GameObject> gameObjects = FindObjectsOfType<GameObject>(true).ToList();
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

