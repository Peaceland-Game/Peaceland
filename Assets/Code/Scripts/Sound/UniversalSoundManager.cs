using System.Collections;
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

    [SerializeField] protected AudioClip GooberWalkCarpet;
    [SerializeField] protected AudioClip GooberWalkConcrete;
    [SerializeField] protected AudioClip GooberWalkGrass;
    [SerializeField] protected AudioClip GooberWalkWood;

    [Space]

    [SerializeField] protected AudioClip ArtifactPickup;
    [SerializeField] protected AudioClip CoinPickup;
    [SerializeField] protected AudioClip GenericPickup;

    [Space]

    [SerializeField] protected AudioClip DialogueSelect;
    [SerializeField] protected AudioClip MenuSelect;
    [SerializeField] protected AudioClip Purchase;
    [SerializeField] protected AudioClip TabletSelect;

    [Space]

    [SerializeField] protected AudioClip AmbienceRiver;
    [SerializeField] protected AudioClip AmbienceStreetlight;

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

        Debug.Log("Getting sound sources...");

        foreach (GameObject a in gameObjects)
        {
            switch(a.tag)
            {
                case "Player":

                    player = a;
                    Debug.Log("Got the player");

                    break;
                case "NPCList":

                    //foreach(GameObject b in a.GetComponentsInChildren<GameObject>())
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
        PlaySound(player, ArtifactPickup);
    }

    public void CoinGet()
    {
        PlaySound(player, CoinPickup);
    }

    public void Pickup()
    {
        PlaySound(player, GenericPickup);
    }

    public void SelectDialogueOption()
    {
        PlaySound(player, DialogueSelect);
    }

    public void SelectMenuOption()
    {
        PlaySound(player, MenuSelect);
    }

    public void MakePurchase()
    {
        PlaySound(player, Purchase);
    }

    public void SelectTabletOption()
    {
        PlaySound(player, TabletSelect);
    }
}
