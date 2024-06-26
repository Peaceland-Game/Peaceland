using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    private Animator animator;
    private int currentPage = 0;
    private readonly int totalPages = 4;
    public float animSpeed = 1f;
    public List<GameObject> pages = new();
    // Start is called before the first frame update
    public ScreenshotCapture screenshotCapture;
    public List<Texture2D> journalEntries = new List<Texture2D>();
    public UnityEngine.UI.Image displayImage;
    private int currentEntryIndex = 0;
    void Start()
    {
        //remove:
        //Time.timeScale = 0;
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", animSpeed);
        ShowPage(currentPage);
    }
    public void AddJournalEntry()
    {
        Texture2D screenshot = screenshotCapture.CaptureScreenshot();
        journalEntries.Add(screenshot);
        //screenshotCapture.SaveScreenshot(screenshot, "JournalEntry_" + journalEntries.Count);
    }
    public void HandleTabClick(int tabNumber)
    {
        if (tabNumber < totalPages)
        {
            var trigger = GetTriggerName(currentPage, tabNumber);
            Debug.Log($"turning from {currentPage} to {tabNumber}");
            if (!string.IsNullOrEmpty(trigger))
            {
                pages[currentPage].SetActive(false);
                currentPage = tabNumber;
                animator.SetTrigger(trigger);
            }
        }
    }
    public void AddArtifact(string name)
    {
        Debug.Log($"Adding {name} to artifacts");
    }

    private string GetTriggerName(int fromPage, int toPage)
    {
        if (fromPage == 0 && toPage == 1)
            return "1To2";
        else if (fromPage == 1 && toPage == 2)
            return "2To3";
        else if (fromPage == 2 && toPage == 3)
            return "3To4";
        else if (fromPage == 1 && toPage == 0)
            return "2To1";
        else if (fromPage == 2 && toPage == 1)
            return "3To2";
        else if (fromPage == 3 && toPage == 2)
            return "4To3";
        else if (fromPage == 0 && toPage == 2)
            return "1To3";
        else if (fromPage == 0 && toPage == 3)
            return "1To4";
        else if (fromPage == 1 && toPage == 3)
            return "2To4";
        else if (fromPage == 2 && toPage == 0)
            return "3To1";
        else if (fromPage == 3 && toPage == 1)
            return "4To2";
        else if (fromPage == 3 && toPage == 0)
            return "4To1";

        return "";
    }

    private void ShowPage(int pageIndex)
    {
        // Ensure the correct static page sprite is shown after animation
        string stateName = "Page" + (pageIndex + 1);
        Debug.Log($"set page: {currentPage}");
        animator.Play(stateName);
    }
    public void ActivatePageDisplay(int pageIndex)
    {
        pages[pageIndex].SetActive(true);
    }
    public void DisplayNextEntry()
    {
        currentEntryIndex = (currentEntryIndex + 1) % journalEntries.Count;
        DisplayCurrentEntry();
    }

    public void DisplayPreviousEntry()
    {
        currentEntryIndex = (currentEntryIndex - 1 + journalEntries.Count) % journalEntries.Count;
        DisplayCurrentEntry();
    }

    private void DisplayCurrentEntry()
    {
        if (journalEntries.Count > 0)
        {
            displayImage.sprite = Sprite.Create(journalEntries[currentEntryIndex],
                new Rect(0, 0, journalEntries[currentEntryIndex].width, journalEntries[currentEntryIndex].height),
                new Vector2(0.5f, 0.5f));
        }
    }
}
