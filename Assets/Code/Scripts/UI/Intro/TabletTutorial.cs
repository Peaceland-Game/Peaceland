using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the tablet onboarding experience
/// </summary>
public class TabletTutorial : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI tutorialText;           //reference to the text mesh in the scene
    [SerializeField]
    private List<Transform> homePageButtons;        //list of the home button graphics to turn on or off during the tutorial
    [SerializeField]
    private RectTransform tutorialRect;             // the rect containing the tutorial text and buttons
    private bool waitForPlayer = false;             //if we are at a point where we need to wait for the player set flag here
    private int tutorialStep = -1;                  //holds the current tutorial step
    public GameObject skipButton;                   //reference to the skip tutorial button in the scene
    public GameObject contButon;                    //reference to the continue button
    private const string LOAD_SCENE_NAME = "HubWorld2";     //the scene that will load after the tutorial is finished 
    private GameObject controller;
    
    /// <summary>
    /// begin the tutorial
    /// </summary>
    void Start()
    {
        controller = GameObject.Find("IntroController");
        ProgressTutorial();
    }
    
    /// <summary>
    /// Used to continuously check for player input to end the tutorial
    /// </summary>
    void Update()
    {
        if (waitForPlayer) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StartCoroutine(controller.GetComponent<IntroController>().FadeToBlack());
            }
        }
    }
    /// <summary>
    /// Add one to tutrial step and update objects and text
    /// </summary>
    public void ProgressTutorial() {
        tutorialStep++;

        UpdateTutorialObjects();
        UpdateTutorialText();
    }
    /// <summary>
    /// Updates the tutorial game objects based on which step of the tutorial the player is on
    /// </summary>
    public void UpdateTutorialObjects() {

        switch (tutorialStep) {
            case 0:
                
                
                break;
            case 1:
                homePageButtons[0].gameObject.SetActive(true);
                tutorialRect.anchoredPosition = new Vector2(-425, 30);
                skipButton.SetActive(false);
                break;
            case 2:
                homePageButtons[0].gameObject.SetActive(false);
                homePageButtons[1].gameObject.SetActive(true);
                break;
            case 3:
                homePageButtons[1].gameObject.SetActive(false);
                homePageButtons[2].gameObject.SetActive(true);
                tutorialRect.anchoredPosition = new Vector2(100, 30);
                contButon.SetActive(false);
                waitForPlayer = true;
                break;
        }

    }
    /// <summary>
    /// Updates the tutorial text based on which step the player is on
    /// </summary>
    public void UpdateTutorialText() {
        string text = "";
        switch (tutorialStep) {

            case 0:
                text = "Journalist's Tablet\n" +
                    "This tablet is your tool to keep track of the information you obtain and records " +
                    "the choices you make. This will help you to write your stories.";
                break;
            case 1:
                text = "Notes App\n" +
                    "As you experience memories, the actions you take and the choices you make will be recorded here.";
                break;
            case 2:
                text = "Peace Graph App\n" +
                    "This app shows a graph that visually represents the impact your articles will have. " +
                    "Make sure to keep an eye on it after finishing a story.";
                break;
            case 3:
                text = "Library App\n" +
                    "As you play the game, any information or documents you find will be stored " +
                    "here for future reference when writing your stories.\n" +
                    "Press Escape to Exit Tablet";
                break;


        }

        tutorialText.text = text;
    }
    /// <summary>
    /// clear player waiting flag
    /// </summary>
    public void StopWaitForPlayer() {
        waitForPlayer = false;
    }
    /// <summary>
    /// force load of the next scene to skip the tutorial
    /// </summary>
    public void SkipTutorial()
    {
        StartCoroutine(controller.GetComponent<IntroController>().FadeToBlack());
    }



}


