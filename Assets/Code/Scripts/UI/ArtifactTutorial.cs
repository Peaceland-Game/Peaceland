using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script to run a tutorial of artifacts on checking the tablet after picking up an artifact.
/// </summary>
public class ArtifactTutorial : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tutorialText;           //reference to the text mesh in the scene
    [SerializeField]
    private RectTransform tutorialRect;             // the rect containing the tutorial text and buttons
    public GameObject continueButton;               //reference to the continue tutorial button in the scene

    /// <summary>
    /// begin the tutorial
    /// </summary>
    void Start()
    {
        tutorialRect.anchoredPosition = new Vector2(0, 100);
        tutorialText.text = "Artifact App\n" +
            "You picked up an artifact!  All of your collected artifacts can be viewed " +
            "by clicking the blue button below.";
    }

    /// <summary>
    /// Used to continuously check for player input to end the tutorial
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndTutorial();
        }
    }

    /// <summary>
    /// Disables the tutorial window.
    /// </summary>
    public void EndTutorial()
    {
        gameObject.SetActive(false);
    }



}
