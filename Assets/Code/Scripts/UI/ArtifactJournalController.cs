using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls all of the artifact images in the journal to reveal them when found
/// This should be on the parent object of each of the artifact images
/// </summary>
public class ArtifactJournalController : MonoBehaviour
{
    
    public List<ArtifactImage> artifacts = new();   //list of all artifacts in the memory
    
    
    void Start()
    {
        //get the artifact image components on the child objects
        artifacts = GetComponentsInChildren<ArtifactImage>().ToList();
    }
    /// <summary>
    /// Attempts to reaveal the artifact by calling the child object's reveal function
    /// </summary>
    /// <param name="name">the name of the artifact to reveal this should match the game object's name in Unity</param>
    /// <exception cref="System.Exception">Thrown when the name of the artifact does not match the scene object</exception>
    public void RevealArtifact(string name)
    {
        var artifact = artifacts.FirstOrDefault(artifact => artifact.gameObject.name == name);

        if (!artifact)
        {
            throw new System.Exception($"Tried to reveal missing artifact {name}");
        }
        artifact.RevealArtifact();
    }

}
