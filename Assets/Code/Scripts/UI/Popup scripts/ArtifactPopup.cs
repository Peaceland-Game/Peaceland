using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] float popupDuration = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitPopup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateArtifactName(string newName)
    {
        artifactName.text = newName;
    }

    public IEnumerator WaitPopup()
    {
        yield return new WaitForSeconds(popupDuration);
        Destroy(gameObject);
    }
}
