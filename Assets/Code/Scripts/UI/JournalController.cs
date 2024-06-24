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
    public float animSpeed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        //remove:
        //Time.timeScale = 0;
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", animSpeed);
        ShowPage(currentPage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PageForward()
    {

        currentPage++;
        //  animator.SetFloat("Speed", animSpeed);
        //  animator.SetBool("fwd", true);
        if (currentPage > totalPages) currentPage = totalPages;
        Debug.Log($"{currentPage - 1} to {currentPage}");

        var trigger = GetTriggerName(currentPage - 1, currentPage);
        if (!string.IsNullOrEmpty(trigger))
        {
            Debug.Log($"trigger: {trigger}");
            animator.SetTrigger(trigger);
        }

    }
    public void PageBackward()
    {

        currentPage--;
        //  animator.SetBool("fwd", false);
        //  animator.SetFloat("Speed", -animSpeed);
        if (currentPage < 0) currentPage = 0;
        Debug.Log($"{currentPage + 1} to {currentPage}");
        var trigger = GetTriggerName(currentPage + 1, currentPage);
        if (!string.IsNullOrEmpty(trigger))
        {
            Debug.Log($"trigger: {trigger}");
            animator.SetTrigger(trigger);
        }
    }
    public void HandleTabClick(int tabNumber)
    {
        if (tabNumber < totalPages)
        {
            var trigger = GetTriggerName(currentPage, tabNumber);
            Debug.Log($"turning from {currentPage} to {tabNumber}");
            if (!string.IsNullOrEmpty(trigger))
            {
                currentPage = tabNumber;
                animator.SetTrigger(trigger);
            }
        }
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
}
