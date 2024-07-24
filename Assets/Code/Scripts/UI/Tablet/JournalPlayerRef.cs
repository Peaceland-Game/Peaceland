using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple passthrough reference object for the artifacts to access the player
/// should be on the artifact's parent object in Unity so you don't have to assign the player for each artifact
/// only once per scene
/// </summary>
public class JournalPlayerRef : MonoBehaviour
{
    public List<Artifact> artifacts = new List<Artifact>();
    public Tablet player;
   // public GameObject artifactPopupPrefab;
    public GameObject imagePopupPrefab;
    public UniversalSoundManager soundManager;

    private void Start()
    {
        artifacts = GetComponentsInChildren<Artifact>().ToList();
    }

    /// <summary>
    /// pass the artifact's name to the player journal controller
    /// </summary>
    /// <param name="artifact">The game object artifact, name should match the artifact name in the journal</param>
    //public void AddArtifact(Artifact artifact, bool showPopup = true) 
    //{
    //    //  Debug.Log("Add Artifact method called");
    //    //var popup = Instantiate(artifactPopupPrefab).GetComponent<PopupNotification>();
    //    //if (popup.HasText)
    //    //    popup.UpdateArtifactName(artifact.artifactName);
    //    //NotificationManager.Instance.QueueNotification(artifactPopupPrefab, artifact.artifactName);
    //    NotificationManager.Instance.QueueNotification(NotificationType.ArtifactPopup, artifact.artifactName);
    //    soundManager.ArtifactGet();
    //    player.AddArtifact(artifact.artifactName, showPopup);
    //}

    public void AddArtifact(string name)
    {
        // Debug.Log("Add Artifact string method called");
        NotificationManager.Instance.QueueNotification(NotificationType.ArtifactPopup, name);
        soundManager.ArtifactGet();
        player.AddArtifact(name, true);
        //var artifact = transform.GetComponentsInChildren<Artifact>().ToList().FirstOrDefault(artifact => artifact.artifactName == name);
        //Destroy(artifact.gameObject);
    }

    public void AddArtifactWithImage(string name)
    {
        AddArtifact(name);
        var imagePopup = Instantiate(imagePopupPrefab);
        var artifact = artifacts.FirstOrDefault(artifact => artifact.artifactName == name);
        imagePopup.GetComponentInChildren<Image>().sprite = artifact.artifactImageToDisplay;
    }

    private void OnEnable() {
        Lua.RegisterFunction(nameof(AddArtifact), this, SymbolExtensions.GetMethodInfo(() => AddArtifact("")));
    }
}
