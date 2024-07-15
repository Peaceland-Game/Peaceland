using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple passthrough reference object for the artifacts to access the player
/// should be on the artifact's parent object in Unity so you don't have to assign the player for each artifact
/// only once per scene
/// </summary>
public class JournalPlayerRef : MonoBehaviour
{
    public Tablet player;

    /// <summary>
    /// pass the artifact's name to the player journal controller
    /// </summary>
    /// <param name="artifact">The game object artifact, name should match the artifact name in the journal</param>
    public void AddArtifact(Artifact artifact, bool showPopup = true) 
    {
        player.AddArtifact(artifact.name, showPopup);
    }
}
