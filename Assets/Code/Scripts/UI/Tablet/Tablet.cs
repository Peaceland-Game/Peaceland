using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class Tablet : MonoBehaviour
{
    
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
    public void ToggleTablet()
    {
        PlayerSingleton.Instance.ToggleJournal();
    }

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

    private void ToggleTabButtons(bool enabled)
    {
        sideTabButtons.ForEach(button => button.enabled = enabled);
    }

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

    public void AddTheme(string name)
    {
        themeNotesPage.RevealTheme(name);
    }

    //public void ShowArtifactPopup(string name)
    //{
    //    artifactName.text = name;
    //    artifactPopup.SetActive(true);
    //    StartCoroutine(WaitPopup());
    //    artifactPopup.SetActive(false);
    //}

    public IEnumerator WaitPopup()
    {
        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// Handle's the player clicking on a tab by changing the page
    /// </summary>
    /// <param name="tabNumber">The tab number that was clicked on [0-totalPages)</param>
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
    public void HandleHomeButtonClick(int tabNumber) {
        HandleTabClick(tabNumber, true);
    }
    public void HandleTabClick(int tabNumber) {
        HandleTabClick(tabNumber, false);
    }
    
    private void PlayArtifactTutorial()
    {
        artifactTutorial.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    

}