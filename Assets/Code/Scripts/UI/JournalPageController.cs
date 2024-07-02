using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JournalPageController : MonoBehaviour
{
    public TabName pageName;
    public Sprite tabSprite;

    public List<JournalCategory> journalCategories = new List<JournalCategory>();
    private TabManager journalTabManager;

    private void Start()
    {
        journalTabManager = GameObject.FindGameObjectWithTag("JournalTabManager").GetComponent<TabManager>();
        journalCategories = GetComponentsInChildren<JournalCategory>().ToList();
        journalCategories.ForEach(catTab => catTab.PopulateSubPages());
        Debug.Log(journalCategories);
        journalTabManager.SpawnCategories(this);

    }

    public List<JournalCategory> GetJournalCategories() { return journalCategories; }
}