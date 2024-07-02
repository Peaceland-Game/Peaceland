using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject topTabPrefab;
    List<List<JournalElement>> journalElements = new List<List<JournalElement>>();
    public void SpawnCategories(JournalPageController journalPage)
    {
        journalPage.GetJournalCategories().ForEach(category =>
        {
            var tab = Instantiate(topTabPrefab, transform);
            tab.GetComponent<Image>().sprite = journalPage.tabSprite;
        });

    }
}
