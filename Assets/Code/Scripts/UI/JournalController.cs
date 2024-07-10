using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the pages and animations of the journal
/// </summary>
public class JournalController : MonoBehaviour
{
    //private Animator animator;  //reference to the journal's animator component
    //private int currentPage = 4;    //current page number 
    //private readonly int totalPages = 5;    //total number of pages
    //public float animSpeed = 1f;    //how fast play the animations
    //public List<GameObject> pages = new List<GameObject>(); //list of each page game object
    //public List<TabController> tabs = new List<TabController>();    //list of the the tab game objects

    //public List<Texture2D> journalEntries = new List<Texture2D>();  
    //public ArtifactJournalController artifactJournal;   //reference to the artifact controller for the artifact page

    ///// <summary>
    ///// Gets the animator, sets its speed, and sets the tab as selected for inital current page
    ///// </summary>
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    animator.SetFloat("Speed", animSpeed);
    //    tabs[currentPage].SetSelected(true);

    //}

    ///// <summary>
    ///// Handle's the player clicking on a tab by changing the page
    ///// </summary>
    ///// <param name="tabNumber">The tab number that was clicked on [0-totalPages)</param>
    //public void HandleTabClick(int tabNumber)
    //{
    //    if (tabNumber < totalPages && tabNumber != currentPage)
    //    {
    //        //deactivate current page
    //        pages[currentPage].SetActive(false);
    //        tabs[currentPage].SetSelected(false);

    //        //trigger the animation depending on if tab clicked was before or after current page
    //        string animationTrigger = (tabNumber > currentPage) ? "MultiFwd" : "MultiBck";
    //        animator.SetTrigger(animationTrigger);

    //        //set current page and turn selected tab on
    //        currentPage = tabNumber;
    //        tabs[currentPage].SetSelected(true);


    //    }
    //}
    ///// <summary>
    ///// Actiavtes the new page
    ///// called by the animation on the final frame
    ///// </summary>
    //public void ActivatePage()
    //{
    //    pages[currentPage].SetActive(true);
    //}

    ///// <summary>
    ///// Passes the name of the artifact to reveal to the artifact journal controller
    ///// </summary>
    ///// <param name="name">The name of the artifact to reveal, should match the game object's name in Unity</param>
    //public void AddArtifact(string name)
    //{
    //    //Debug.Log($"Adding {name} to artifacts");
    //    artifactJournal.RevealArtifact(name);
    //}

}