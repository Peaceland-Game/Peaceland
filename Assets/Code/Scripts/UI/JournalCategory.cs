using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JournalCategory : JournalElement
{
    List<JournalSubPage> subPages = new();


    public void PopulateSubPages()
    {
        subPages = GetComponentsInChildren<JournalSubPage>().ToList();
    }

    public List<JournalSubPage> GetSubPages() { return subPages; }
}