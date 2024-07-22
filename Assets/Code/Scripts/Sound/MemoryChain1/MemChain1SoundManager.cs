using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemChain1SoundManager : UniversalSoundManager
{
    private GameObject gryf;

    [Header("Memory Chain 1 Sounds")]

    [Space]

    [SerializeField] AudioClip GryfGrowl;
    [SerializeField] AudioClip GryfWhine;

    // Start is called before the first frame update
    void Start()
    {
        GetUniversalSoundSources();

        foreach(GameObject a in allCharacters)
        {
            if(a.name == "Gryf")
            {
                gryf = a;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
