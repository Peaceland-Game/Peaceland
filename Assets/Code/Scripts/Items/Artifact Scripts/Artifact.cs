using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{

    [SerializeField]
    bool hasImageToDisplay;
    public string artifactName;

    //don't need this for artifact splash
    [SerializeField]
    Sprite artifactImageToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUse()
    {
        transform.parent.GetComponent<JournalPlayerRef>().AddArtifact(this, hasImageToDisplay);
    }
}
