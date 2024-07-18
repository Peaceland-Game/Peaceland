using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ArtifactGroup : MonoBehaviour
{
    public TextMeshProUGUI title;
    public List<ArtifactImage> artifacts = new();

    void Start()
    {
        title = GetComponent<TextMeshProUGUI>();

        //get the artifact image components on the child objects
        artifacts = GetComponents<ArtifactImage>().ToList();
    }

    public void Show()
    {
        title.gameObject.SetActive(true);

        foreach(ArtifactImage ai in artifacts)
        {
            ai.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        title.gameObject.SetActive(false);

        foreach (ArtifactImage ai in artifacts)
        {
            ai.gameObject.SetActive(false);
        }
    }
}
