using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TabletTutorial : MonoBehaviour {
    //public GameObject tablet;
    //public GameObject tutorialPrefab;
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    [SerializeField]
    private List<Transform> homePageButtons;
    private bool waitForPlayer = false;
    private GameObject tutorialInstance;
    private int tutorialStep = -1;
    public GameObject skipButton;
    public GameObject contButon;
    private const string LOAD_SCENE_NAME = "HubWorld2";
    // Start is called before the first frame update
    void Start()
    {
        ProgressTutorial();

        
      //  homePageButtons.ForEach(homePageButtons => homePageButtons.gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForPlayer) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene(LOAD_SCENE_NAME);
            }
        }
    }
    public void ProgressTutorial() {
        tutorialStep++;

        UpdateTutorialObjects();
        UpdateTutorialText();


    }
    public void UpdateTutorialObjects() {

        switch (tutorialStep) {
            case 0:
                
                
                break;
            case 1:
                homePageButtons[0].gameObject.SetActive(true);
                skipButton.SetActive(false);
                break;
            case 2:
                homePageButtons[0].gameObject.SetActive(false);
                homePageButtons[1].gameObject.SetActive(true);
                break;
            case 3:
                homePageButtons[1].gameObject.SetActive(false);
                homePageButtons[2].gameObject.SetActive(true);
                contButon.SetActive(false);
                waitForPlayer = true;
                break;
        }

    }
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
    public void StopWaitForPlayer() {
        waitForPlayer = false;
    }
    public void SkipTutorial()
    {
        SceneManager.LoadScene(LOAD_SCENE_NAME);
    }



}


