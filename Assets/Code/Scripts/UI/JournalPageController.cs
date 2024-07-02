using System.Collections.Generic;
using UnityEngine;

public class JournalPageController : MonoBehaviour
{
    private List<JournalCategory> categories = new List<JournalCategory>();
    private List<JournalSubPage> currentSubPages = new List<JournalSubPage>();
    public List<Sprite> tabSprites = new();

    private void Awake()
    {
        // Populate categories
        categories.AddRange(GetComponentsInChildren<JournalCategory>());
    }

    public void SelectCategory(int categoryIndex)
    {
        if (categoryIndex >= 0 && categoryIndex < categories.Count)
        {
            JournalCategory selectedCategory = categories[categoryIndex];
            currentSubPages.Clear();
            currentSubPages.AddRange(selectedCategory.GetComponentsInChildren<JournalSubPage>());

            // Here you would instantiate or update your UI for the sub-pages
            UpdateSubPageTabs();
        }
    }

    private void UpdateSubPageTabs()
    {
        // Implement your logic to create or update sub-page tabs
        // This might involve instantiating prefabs, updating UI elements, etc.
    }

    public void SelectSubPage(int subPageIndex)
    {
        if (subPageIndex >= 0 && subPageIndex < currentSubPages.Count)
        {
            JournalSubPage selectedSubPage = currentSubPages[subPageIndex];

            // Implement your logic to show the selected sub-page
            // This might involve playing animations, enabling/disabling GameObjects, etc.
        }
    }
}