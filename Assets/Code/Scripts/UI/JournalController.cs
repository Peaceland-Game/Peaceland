using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    private Animator animator;
    private int currentPage = 0;
    private readonly int totalPages = 5;
    public float animSpeed = 1f;
    public List<GameObject> pages = new List<GameObject>();
    public List<TabController> tabs = new List<TabController>();

    public List<Texture2D> journalEntries = new List<Texture2D>();
    public ArtifactJournalController artifactJournal;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", animSpeed);
        tabs[currentPage].SetSelected(true);
        
    }

    public void HandleTabClick(int tabNumber)
    {
        if (tabNumber < totalPages && tabNumber != currentPage)
        {
            pages[currentPage].SetActive(false);
            tabs[currentPage].SetSelected(false);

            string animationTrigger = (tabNumber > currentPage) ? "MultiFwd" : "MultiBck";
            animator.SetTrigger(animationTrigger);

            currentPage = tabNumber;
            tabs[currentPage].SetSelected(true);

           // StartCoroutine(ActivatePageAfterAnimation(tabNumber));
        }
    }
    public void ActivatePage()
    {
        pages[currentPage].SetActive(true);
    }

    //private IEnumerator ActivatePageAfterAnimation(int pageIndex)
    //{
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    //    pages[pageIndex].SetActive(true);
    //}

    public void AddArtifact(string name)
    {
        Debug.Log($"Adding {name} to artifacts");
        artifactJournal.RevealArtifact(name);
    }

}