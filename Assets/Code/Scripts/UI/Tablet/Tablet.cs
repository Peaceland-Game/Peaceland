using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

/// <summary>
/// Manages the functionality of a tablet device in the game,
/// including sidebar navigation, artifact journal, and theme notes.
/// </summary>
public class Tablet : MonoBehaviour
{
    //state of the left sidebar
    public enum SidebarState
    {
        Shown,
        Moving,
        Hidden,
    }
    [Header("Tab Controls")]
    private SidebarState currentState = SidebarState.Hidden;
    private SidebarState targetState;
    public GameObject tabs;
    public TextMeshProUGUI buttonText;

    private List<Button> sideTabButtons = new();

    private const string INTRO_SCENE_NAME = "IntroSequence";

    private float moveSpeed = 300f; // Speed of the movement
    //private float shownPosition = 150f; // The 'shown' position in local space

    private Vector3 hiddenPosition = new(-1033, 529, 0);
    private Vector3 shownPosition = new(-878, 529, 0);

    [Header("Pages")]
    public List<GameObject> apps = new();
    public int currentPage = 0;
    private int totalPages = 0;

    public ArtifactJournalController artifactJournal;
    public GameObject artifactPopup;
    public TextMeshProUGUI artifactName;

    public ThemeNotesController themeNotesPage;

    private int numberOfArtifacts = 0;
    private bool playArtifactTutorial = false;
    public GameObject artifactTutorial;

    /// <summary>
    /// Initializes the tablet, setting up pages and buttons.
    /// </summary>
    void Start()
    {
        totalPages = apps.Count;
        
        for (int x = 1; x < apps.Count; x++)
        {
            apps[x].SetActive(false);
        }

        var introScene = SceneManager.GetActiveScene().name == INTRO_SCENE_NAME;
        if (!introScene)
            gameObject.SetActive(false);
       
        //get the tab buttons
        sideTabButtons = tabs.GetComponentsInChildren<Button>().ToList();

        //hardcode remove the tab swap button this is not a good idea and will break if the hide/show tab button is moved from being the first child
        //of the tabs parent object
        sideTabButtons.RemoveAt(0);
        ToggleTabButtons(false);

    }
    /// <summary>
    /// Updates the tablet state and handles artifact tutorial.
    /// </summary>
    void Update()
    {
        if (currentState == SidebarState.Moving)
        {
            MoveTabs();
        }
        if (playArtifactTutorial)
        {
            playArtifactTutorial = false;
            PlayArtifactTutorial();
        }
    }
    /// <summary>
    /// Toggles the sidebar between shown and hidden states.
    /// </summary>
    public void ToggleSidebar()
    {
        if (currentState != SidebarState.Moving)
        {
            targetState = (currentState == SidebarState.Hidden) ? SidebarState.Shown : SidebarState.Hidden;
            currentState = SidebarState.Moving;
            UpdateButtonText();

            if (targetState == SidebarState.Hidden)
            {
                ToggleTabButtons(false);
            }

        }
    }
    /// <summary>
    /// Toggles the tablet visibility.
    /// </summary>
    public void ToggleTablet()
    {
        PlayerSingleton.Instance.ToggleTablet();
    }
    /// <summary>
    /// Moves the tabs towards the target position based on the current state.
    /// </summary>
    private void MoveTabs()
    {
        Vector3 targetPosition = (targetState == SidebarState.Shown) ? shownPosition : hiddenPosition;
        tabs.transform.localPosition = Vector3.MoveTowards(tabs.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(tabs.transform.localPosition, targetPosition) < 0.01f)
        {
            tabs.transform.localPosition = targetPosition;
            currentState = targetState;

            //toggle buttons on or off if the tab is shown or not
            if (currentState == SidebarState.Shown)
            {
                ToggleTabButtons(true);
            }
        }
    }
    /// <summary>
    /// Enables or disables the tab buttons.
    /// </summary>
    /// <param name="enabled">Whether to enable or disable the buttons.</param>
    private void ToggleTabButtons(bool enabled)
    {
        sideTabButtons.ForEach(button => button.enabled = enabled);
    }
    /// <summary>
    /// Updates the button text based on the target state.
    /// </summary>
    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = (targetState == SidebarState.Shown) ? "<" : ">";
        }
    }

    /// <summary>
    /// Passes the name of the artifact to reveal to the artifact journal controller
    /// </summary>
    /// <param name="name">The name of the artifact to reveal</param>
    public void AddArtifact(string name, bool showPopup)
    {
       // Debug.Log($"Adding {name} to artifacts");
        //show popup if required
        if (!showPopup)
            artifactJournal.RevealArtifact(name);
        else
        {
            //show popup
            //ShowArtifactPopup(name);
            //then add
            artifactJournal.RevealArtifact(name);
        }

        numberOfArtifacts++;
        //if this is the first artifact, play the tutorial the next time the tablet is opened
        if (numberOfArtifacts == 1)
        {
            playArtifactTutorial = true;
        }
    }
    /// <summary>
    /// Adds a theme to the theme notes page.
    /// </summary>
    /// <param name="name">The name of the theme to add.</param>
    public void AddTheme(string name)
    {
        themeNotesPage.RevealTheme(name);
    }
    /// <summary>
    /// Waits for a short duration, used for popup display.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    public IEnumerator WaitPopup()
    {
        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// Handles tab click events, changing the current page.
    /// </summary>
    /// <param name="tabNumber">The index of the clicked tab.</param>
    /// <param name="fromHomePage">Whether the click originated from the home page.</param>
    private void HandleTabClick(int tabNumber, bool fromHomePage = false)
    {
        if (tabNumber < totalPages && tabNumber != currentPage)
        {
            //deactivate current page
            apps[currentPage].SetActive(false);

            //set current page and turn selected tab on
            currentPage = tabNumber;

            apps[currentPage].SetActive(true);

            if (!fromHomePage)
                ToggleSidebar();


        }
    }
    /// <summary>
    /// Handles home button click events.
    /// </summary>
    /// <param name="tabNumber">The index of the tab to switch to.</param>
    public void HandleHomeButtonClick(int tabNumber) {
        HandleTabClick(tabNumber, true);
    }
    /// <summary>
    /// Handles tab click events.
    /// </summary>
    /// <param name="tabNumber">The index of the clicked tab.</param>
    public void HandleTabClick(int tabNumber) {
        HandleTabClick(tabNumber, false);
    }
    /// <summary>
    /// Plays the artifact tutorial.
    /// </summary>
    private void PlayArtifactTutorial()
    {
        artifactTutorial.SetActive(true);
    }
    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    

}