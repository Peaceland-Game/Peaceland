using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniversalSoundManager : MonoBehaviour
{
    protected GameObject player;
    protected List<GameObject> allCharacters;

    protected List<GameObject> artifacts;
    protected List<GameObject> otherPickups;

    protected GameObject river;
    protected List<GameObject> streetLights;

    [Header("Universal Sounds")]

    [Space]

    [SerializeField] protected AudioClip gooberWalkCarpet;
    [SerializeField] protected AudioClip gooberWalkConcrete;
    [SerializeField] protected AudioClip gooberWalkGrass;
    [SerializeField] protected AudioClip gooberWalkWood;

    [Space]

    [SerializeField] protected AudioClip artifactPickup;
    [SerializeField] protected AudioClip themePickup;
    [SerializeField] protected AudioClip coinPickup;
    [SerializeField] protected AudioClip genericPickup;

    [Space]

    [SerializeField] protected AudioClip dialogueSelect;
    [SerializeField] protected AudioClip menuSelect;
    [SerializeField] protected AudioClip purchase;
    [SerializeField] protected AudioClip tabletSelect;

    [Space]

    [SerializeField] protected AudioClip ambienceRiver;
    [SerializeField] protected AudioClip ambienceStreetlight;

    /// <summary>
    /// Gets audio sources that are likely to be in multiple memories and the hub
    /// </summary>
    protected void GetUniversalSoundSources()
    {
        List<GameObject> gameObjects = FindObjectsOfType<GameObject>().ToList();
        allCharacters = new List<GameObject>();
        artifacts = new List<GameObject>();
        otherPickups = new List<GameObject>();
        streetLights = new List<GameObject>();

        //Debug.Log("Getting sound sources...");

        foreach (GameObject a in gameObjects)
        {
            switch(a.tag)
            {
                case "Player":

                    player = a;
                    //Debug.Log("Got the player...");

                    break;
                // NPCList and Artifacts cause issues right now, so be careful about uncommenting those
                case "NPCList":

                    //foreach (GameObject b in a.GetComponentsInChildren<GameObject>())
                    //{
                    //    allCharacters.Add(b);
                    //}

                    break;
                case "Artifacts":

                    //foreach (GameObject b in a.GetComponentsInChildren<GameObject>())
                    //{
                    //    artifacts.Add(b);
                    //}

                    break;
                case "Interactable":

                    otherPickups.Add(a);

                    break;
                case "Lamp":

                    streetLights.Add(a);

                    break;
                case "River":

                    river = a;

                    break;
            }
        }
    }

    /// <summary>
    /// Plays a sound clip
    /// </summary>
    /// <param name="source"> the object from which the sound plays </param>
    /// <param name="clip"> the sound clip to be played </param>
    public void PlaySound(GameObject source, AudioClip clip)
    {
        AudioSource audioSource = source.GetComponent<AudioSource>();

        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Makes a game object's audio source play a sound (if it has one assigned to it). 
    /// Note that you must enable looping in the audio source itself in order for it to play more than once.
    /// </summary>
    /// <param name="source"></param>
    public void PlayLoopingSound(GameObject source)
    {
        AudioSource audioSource = source.GetComponent<AudioSource>();

        audioSource.Play();
    }

    public void Walk()
    {
        // I started working on putting an enum for this in the player controller, but it isn't
        // quite ready to use yet
    }

    public void ArtifactGet()
    {
        PlaySound(player, artifactPickup);
    }

    public void ThemeGet()
    {
        PlaySound(player, themePickup);
    }

    public void CoinGet()
    {
        PlaySound(player, coinPickup);
    }

    public void Pickup()
    {
        PlaySound(player, genericPickup);
    }

    public void SelectDialogueOptionSound()
    {
        PlaySound(player, dialogueSelect);
    }

    public void SelectMenuOption()
    {
        PlaySound(player, menuSelect);
    }

    public void MakePurchase()
    {
        PlaySound(player, purchase);
    }

    public void SelectTabletOption()
    {
        PlaySound(player, tabletSelect);
    }

    protected virtual void OnEnable()
    {
        Lua.RegisterFunction(nameof(SelectDialogueOptionSound), this, SymbolExtensions.GetMethodInfo(() => SelectDialogueOptionSound()));
    }
}
