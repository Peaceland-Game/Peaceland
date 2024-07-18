using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactTutorial : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tutorialText;           //reference to the text mesh in the scene
    [SerializeField]
    private List<Transform> homePageButtons;        //list of the home button graphics to turn on or off during the tutorial
    [SerializeField]
    private RectTransform tutorialRect;             // the rect containing the tutorial text and buttons
    private bool waitForPlayer = false;             //if we are at a point where we need to wait for the player set flag here
    public GameObject continueButton;               //reference to the continue tutorial button in the scene

    /// <summary>
    /// begin the tutorial
    /// </summary>
    void Start()
    {
        ProgressTutorial();
    }

    /// <summary>
    /// Used to continuously check for player input to end the tutorial
    /// </summary>
    void Update()
    {
        if (waitForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndTutorial();
            }
        }
    }
    /// <summary>
    /// Add one to tutrial step and update objects and text
    /// </summary>
    public void ProgressTutorial()
    {
        UpdateTutorialObjects();
        UpdateTutorialText();
    }
    /// <summary>
    /// Updates the tutorial game objects based on which step of the tutorial the player is on
    /// </summary>
    public void UpdateTutorialObjects()
    {
        tutorialRect.anchoredPosition = new Vector2(0, 100);
        waitForPlayer = true;
    }
    /// <summary>
    /// Updates the tutorial text based on which step the player is on
    /// </summary>
    public void UpdateTutorialText()
    {
        tutorialText.text = "Artifact App\n" +
            "You picked up an artifact!  All of your collected artifacts can be viewed " +
            "by clicking the blue button below.";
    }

    public void EndTutorial()
    {
        gameObject.SetActive(false);
    }



}
