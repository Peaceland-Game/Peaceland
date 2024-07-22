using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemChain1SoundManager : UniversalSoundManager
{
    private AudioSource gryf;
    private AudioSource rabidDog;

    [Header("Memory Chain 1 Sounds")]

    [Space]

    [SerializeField] AudioClip gryfGrowl;
    [SerializeField] AudioClip gryfWhine;

    // Start is called before the first frame update
    void Start()
    {
        GetUniversalSoundSources();

        foreach(GameObject a in allCharacters) // There's an issue here at the moment
        {
            switch(a.name)
            {
                case "Gryf":
                    gryf = a.GetComponent<AudioSource>();
                    break;
                case "RabidDog":
                    rabidDog = a.GetComponent<AudioSource>();
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
        gryf.PlayOneShot(gryfGrowl);
    }

    public void GryfWhine()
    {
        gryf.PlayOneShot(gryfWhine);
    }

    //public void StrayGrowl()
    //{
    //    rabidDog.PlayOneShot(gryfGrowl);
    //}

    //public void StrayWhine()
    //{
    //    rabidDog.PlayOneShot(gryfWhine);
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        Lua.RegisterFunction(nameof(GryfGrowl), this, SymbolExtensions.GetMethodInfo(() => GryfGrowl()));
        Lua.RegisterFunction(nameof(GryfWhine), this, SymbolExtensions.GetMethodInfo(() => GryfWhine()));
        //Lua.RegisterFunction(nameof(StrayGrowl), this, SymbolExtensions.GetMethodInfo(() => StrayGrowl()));
        //Lua.RegisterFunction(nameof(StrayWhine), this, SymbolExtensions.GetMethodInfo(() => StrayWhine()));
    }
}
