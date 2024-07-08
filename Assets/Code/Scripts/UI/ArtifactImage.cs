using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a journal artifact image to control hiding/revealing the artifact when found
/// </summary>
public class ArtifactImage : MonoBehaviour
{
    [SerializeField] private Image artifactImage;   //reference to the UI image game object
    [SerializeField] private Sprite silhouetteImage;    //"hidden" artifact silhoutte
    [SerializeField] private Sprite revealedImage;      //the revealed artifact image
    [SerializeField] private TextMeshProUGUI imageText; //reference to the GUI text object
    public bool revealed = false;               //bool indicating if the object is shown or not
    
    /// <summary>
    /// Set the sprite image to hidden 
    /// </summary>
    void Start()
    {
        if (revealed)
        {
            artifactImage.sprite = revealedImage;
            imageText.text = gameObject.name;
        }
        else 
            artifactImage.sprite = silhouetteImage;
    }

    /// <summary>
    /// Reveal the artifact by setting the image to shown and setting the text to the game object's name
    /// this should match the name of the artifact
    /// </summary>
    public void RevealArtifact()
    {
        revealed = true;
        artifactImage.sprite = revealedImage;
        imageText.text = gameObject.name;
    }

}
