using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    private float moveSpeed = 300f; // Speed of the movement
    //private float shownPosition = 150f; // The 'shown' position in local space

    private Vector3 hiddenPosition = new(-1033, 529, 0);
    private Vector3 shownPosition = new(-878, 529, 0);

    [Header("Pages")]
    public List<GameObject> apps = new();
    public int currentPage = 0;
    private int totalPages = 0;

    public ArtifactJournalController artifactJournal;

    


    void Start()
    {
        totalPages = apps.Count;
        
       

    }

    void Update()
    {
        if (currentState == SidebarState.Moving)
        {
            MoveTabs();
        }
    }

    public void ToggleSidebar()
    {
        if (currentState != SidebarState.Moving)
        {
            targetState = (currentState == SidebarState.Hidden) ? SidebarState.Shown : SidebarState.Hidden;
            currentState = SidebarState.Moving;
            UpdateButtonText();
        }
    }

    private void MoveTabs()
    {
        Vector3 targetPosition = (targetState == SidebarState.Shown) ? shownPosition : hiddenPosition;
        tabs.transform.localPosition = Vector3.MoveTowards(tabs.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(tabs.transform.localPosition, targetPosition) < 0.01f)
        {
            tabs.transform.localPosition = targetPosition;
            currentState = targetState;
        }
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
    /// <param name="name">The name of the artifact to reveal, should match the game object's name in Unity</param>
    public void AddArtifact(string name)
    {
        //Debug.Log($"Adding {name} to artifacts");
        artifactJournal.RevealArtifact(name);
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
    
    

    public void ExitGame()
    {
        Application.Quit();
    }

}