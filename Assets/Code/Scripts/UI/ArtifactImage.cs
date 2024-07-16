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
    [SerializeField] private Image artifactImage; //reference to the UI image game object for the artifact's appearance
    [SerializeField] private Sprite silhouetteImage; //"hidden" artifact silhoutte
    [SerializeField] private Sprite revealedImage; //the revealed artifact image

    [SerializeField] private Image textBox; //reference to the UI image game object for the artifact's title and description box
    [SerializeField] private Sprite darkTextBox; //the hidden (black) text box
    [SerializeField] private Sprite revealedTextBox; //the revealed (green) text box
    [SerializeField] private TextMeshProUGUI imageTitle; //reference to the GUI text object for the artifact's title
    [SerializeField] private TextMeshProUGUI imageDescription; //reference to the GUI text object for the artifact's description
    public string artifactName;
    public bool revealed = false; //bool indicating if the object is shown or not
    
    /// <summary>
    /// Set the sprite image to the correct state
    /// </summary>
    void Start()
    {
        if (revealed)
        {
            artifactImage.sprite = revealedImage;
            textBox.sprite = revealedTextBox;
            imageTitle.gameObject.SetActive(true);
            imageDescription.gameObject.SetActive(true);
        }
        else
        {
            artifactImage.sprite = silhouetteImage;
            textBox.sprite = darkTextBox;
            imageTitle.gameObject.SetActive(false);
            imageDescription.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reveal the artifact by setting the image and text box to shown and enabling the text
    /// </summary>
    public void RevealArtifact()
    {
        revealed = true;

        artifactImage.sprite = revealedImage;
        textBox.sprite = revealedTextBox;
        imageTitle.gameObject.SetActive(true);
        imageDescription.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hey, you never know
    /// </summary>
    public void HideArtifact()
    {
        revealed = false;

        artifactImage.sprite = silhouetteImage;
        textBox.sprite = darkTextBox;
        imageTitle.gameObject.SetActive(false);
        imageDescription.gameObject.SetActive(false);
    }

}
