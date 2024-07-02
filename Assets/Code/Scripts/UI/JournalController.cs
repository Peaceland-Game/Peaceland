using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {
    private Animator animator;
    private int currentPage = 0;
    private readonly int totalPages = 5; // Updated to 5 pages
    public float animSpeed = 1f;
    public List<GameObject> pages = new List<GameObject>();
    public List<Transform> tabs = new List<Transform>();
    public float tabHoverOffset = 10f; // Adjust this value to control how much tabs pop out
    public float tabAnimationDuration = 0.2f;

    public ScreenshotCapture screenshotCapture;
    public List<Texture2D> journalEntries = new List<Texture2D>();
    private int currentEntryIndex = 0;
    public ArtifactJournalController artifactJournal;

    private static int SETTINGS_TAB = 4;

    void Start() {
        animator = GetComponent<Animator>();
      //  animator.SetFloat("Speed", animSpeed);
     //   ShowPage(currentPage);
    }

    public void HandleTabHoverEnter(int tabIndex) {
        HandleTabHover(tabIndex, true);
    }
    public void HandleTabHoverExit(int tabIndex) {
        HandleTabHover(tabIndex, false);
    }   
    private void HandleTabHover(int tabIndex, bool isEnter) {
        if (isEnter) {
            StartCoroutine(AnimateTabHover(tabIndex, tabs[tabIndex], true));
        }
        else {
            StartCoroutine(AnimateTabHover(tabIndex, tabs[tabIndex], false));
        }
    }

    private IEnumerator AnimateTabHover(int index, Transform tab, bool isEnter) {
        Vector3 startPos = tab.localPosition;
        //
        var tabDirection = isEnter ? Vector3.left : Vector3.right;
        tabDirection = index == SETTINGS_TAB ? -tabDirection : tabDirection;
        Vector3 endPos = startPos + tabDirection * tabHoverOffset;
        float elapsedTime = 0f;

        while (elapsedTime < tabAnimationDuration) {
            tab.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / tabAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tab.localPosition = endPos;
    }

    public void HandleTabClick(int tabNumber) {
        if (tabNumber < totalPages && tabNumber != currentPage) {
            pages[currentPage].SetActive(false);
            string animationTrigger = (tabNumber > currentPage) ? "MultiFwd" : "MultiBck";
            animator.SetTrigger(animationTrigger);
            currentPage = tabNumber;
            StartCoroutine(ActivatePageAfterAnimation(tabNumber));
        }
    }

    private IEnumerator ActivatePageAfterAnimation(int pageIndex) {
        // Wait for the animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        pages[pageIndex].SetActive(true);
    }

    public void AddArtifact(string name) {
        Debug.Log($"Adding {name} to artifacts");
        artifactJournal.RevealArtifact(name);
    }

    //private void ShowPage(int pageIndex) {
    //    string stateName = "Page" + (pageIndex + 1);
    //    Debug.Log($"set page: {currentPage}");
    //    animator.Play(stateName);
    //}
}