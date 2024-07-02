using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTabController : TabController
{
    protected override void Awake()
    {
        //tabIndex = TabUtility.GetTabIndex(tabName);
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;
        hoveredPosition = defaultPosition + Vector2.up * hoverOffset;
        targetPosition = defaultPosition;

        journalController = GetComponentInParent<JournalController>();
    }
}
