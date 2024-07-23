using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemChain1SoundManager : UniversalSoundManager
{
    private GameObject gryf;
    private GameObject rabidDog;

    [Header("Memory Chain 1 Sounds")]

    [Space]

    [SerializeField] AudioClip gryfGrowl;
    [SerializeField] AudioClip gryfWhine;

    // Start is called before the first frame update
    void Start()
    {
        GetUniversalSoundSources();

        foreach (GameObject a in allCharacters)
        {
            switch (a.name)
            {
                case "Gryf":
                    gryf = a;
                    //Debug.Log("Found Gryf");
                    break;
                case "RabidDog":
                    rabidDog = a;
                    //Debug.Log("Found rabid dog");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GryfGrowl()
    {
        PlaySound(gryf, gryfGrowl);
    }

    public void GryfWhine()
    {
        PlaySound(gryf, gryfWhine);
    }

    public void StrayGrowl()
    {
        PlaySound(rabidDog, gryfGrowl);
    }

    public void StrayWhine()
    {
        PlaySound(rabidDog, gryfWhine);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Lua.RegisterFunction(nameof(GryfGrowl), this, SymbolExtensions.GetMethodInfo(() => GryfGrowl()));
        Lua.RegisterFunction(nameof(GryfWhine), this, SymbolExtensions.GetMethodInfo(() => GryfWhine()));
        Lua.RegisterFunction(nameof(StrayGrowl), this, SymbolExtensions.GetMethodInfo(() => StrayGrowl()));
        Lua.RegisterFunction(nameof(StrayWhine), this, SymbolExtensions.GetMethodInfo(() => StrayWhine()));
    }
}

