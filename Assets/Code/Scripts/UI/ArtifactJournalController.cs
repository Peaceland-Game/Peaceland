using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtifactJournalController : MonoBehaviour
{
    public List<ArtifactImage> artifacts = new();
    // Start is called before the first frame update
    void Start()
    {
        artifacts = GetComponentsInChildren<ArtifactImage>().ToList();
        //artifacts.ForEach(a => Debug.Log(a.name));
    }
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
