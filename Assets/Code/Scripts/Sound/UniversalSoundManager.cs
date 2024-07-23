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
        List<GameObject> gameObjects = FindObjectsOfType<GameObject>(true).ToList();
        allCharacters = new List<GameObject>();
        artifacts = new List<GameObject>();
        otherPickups = new List<GameObject>();
        streetLights = new List<GameObject>();

        //Debug.Log("Getting sound sources...");

        bool foundPlayer = false;

        foreach (GameObject a in gameObjects)
        {
            switch (a.tag)
            {
                case "Player":
                case "MainCamera":

                    if(!foundPlayer || a.tag == "Player")
                    {
                        player = a;
                        Debug.Log($"Got player: {player.name}");
                        foundPlayer = true;
                    }

                    break;
                case "NPC":

                    allCharacters.Add(a);

                    break;
                case "Artifacts":

                    artifacts.Add(a);

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
    /// <param name="source"> the game object to play a sound, which must have an audio source component with an audio clip </param>
    public void PlayLoopingSound(GameObject source)
    {
        AudioSource audioSource = source.GetComponent<AudioSource>();

        audioSource.Play();
    }

    /// <summary>
    /// Stops a game object's audio source from playing its sound
    /// </summary>
    /// <param name="source"> the game object to stop playing sound </param>
    public void StopSound(GameObject source)
    {
        AudioSource audioSource = source.GetComponent<AudioSource>();

        audioSource.Stop();
    }

    /// <summary>
    /// Plays footstep sounds
    /// </summary>
    public void Walk()
    {
        // I started working on putting an enum for this in the player controller, but it isn't
        // quite ready to use yet
    }

    /// <summary>
    /// Plays the sound effect for obtaining an artifact
    /// </summary>
    public void ArtifactGet()
    {
        PlaySound(player, artifactPickup);
    }

    /// <summary>
    /// Plays the sound effect for obtaining a theme
    /// </summary>
    public void ThemeGet()
    {
        PlaySound(player, themePickup);
    }

    /// <summary>
    /// Plays the sound effect for gaining money
    /// </summary>
    public void CoinGet()
    {
        PlaySound(player, coinPickup);
    }

    /// <summary>
    /// Plays the sound effect for picking up most objects
    /// </summary>
    public void Pickup()
    {
        PlaySound(player, genericPickup);
    }

    /// <summary>
    /// Plays the sound effect for selecting a dialogue option
    /// </summary>
    public void SelectDialogueOptionSound()
    {
        PlaySound(player, dialogueSelect);
    }

    /// <summary>
    /// Plays the sound effect for clicking a button on the main menu or settings menu, if in a scene with a player object
    /// </summary>
    public void SelectMenuOptionWithPlayer()
    {
        PlaySound(player, menuSelect);
    }

    /// <summary>
    /// Plays the sound effect for spending money
    /// </summary>
    public void MakePurchase()
    {
        PlaySound(player, purchase);
    }

    /// <summary>
    /// Plays the sound effect for clicking a tablet button
    /// </summary>
    public void SelectTabletOption()
    {
        PlaySound(player, tabletSelect);
    }

    /// <summary>
    /// Give Lua some functions
    /// </summary>
    protected virtual void OnEnable()
    {
        Lua.RegisterFunction(nameof(SelectDialogueOptionSound), this, SymbolExtensions.GetMethodInfo(() => SelectDialogueOptionSound()));
        Lua.RegisterFunction(nameof(Pickup), this, SymbolExtensions.GetMethodInfo(() => Pickup()));
    }
}
