using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactImage : MonoBehaviour
{
    [SerializeField] private Image artifactImage;
    [SerializeField] private Sprite silhouetteImage;
    [SerializeField] private Sprite revealedImage;
    [SerializeField] private TextMeshProUGUI imageText;
    public bool revealed = false;
    // Start is called before the first frame update
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

    public void RevealArtifact()
    {
        revealed = true;
        artifactImage.sprite = revealedImage;
        imageText.text = gameObject.name;
    }

}
