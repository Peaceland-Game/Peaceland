using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    private Animator animator;
    private int currentPage = 0;
    private readonly int totalPages = 4;
    // Start is called before the first frame update
    void Start()
    {
        //remove:
       // Time.timeScale = 0;
        animator = GetComponent<Animator>();
        ShowPage(currentPage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PageForward() {
        Debug.Log("page forward");
        currentPage++;
        if (currentPage >  totalPages) currentPage = totalPages; 

        var trigger = GetTriggerName(currentPage - 1, currentPage);
        if (!string.IsNullOrEmpty(trigger))
        {
            animator.SetTrigger(trigger);
        }

    }
    public void PageBackward() {
        Debug.Log("page back");
        currentPage--;
        if (currentPage < 0) currentPage = 0;
        var trigger = GetTriggerName(currentPage+ 1, currentPage);
        if (!string.IsNullOrEmpty(trigger))
        {
            animator.SetTrigger(trigger);
        }
    }

    private string GetTriggerName(int fromPage, int toPage)
    {
        if (fromPage == 0 && toPage == 1)
        {
            return "1To2";
        }
        else if (fromPage == 1 && toPage == 2)
        {
            return "2To3";
        }
        else if (fromPage == 2 && toPage == 3)
        {
            return "3To4";
        }
        else if (fromPage == 3 && toPage == 2)
        {
            return "4To3";
        }
        else if (fromPage == 2 && toPage == 1)
        {
            return "3To2";
        }
        else if (fromPage == 1 && toPage == 0)
        {
            return "2To1";
        }
        return "";
    }

    private void ShowPage(int pageIndex)
    {
        // Ensure the correct static page sprite is shown after animation
        string stateName = "Page" + (pageIndex + 1);
        animator.Play(stateName);
    }
}
