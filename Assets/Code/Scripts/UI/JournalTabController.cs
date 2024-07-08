using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System;


/// <summary>
/// Controls a single tab in the journal
/// </summary>
public class TabController : MonoBehaviour
{   
    /// <summary>
    /// Three states that a tab can be in used to determine movement
    /// </summary>
    public enum TabState { Default, Hovered, Selected }

    [SerializeField] protected TabName tabName; //the reference to the tab name instance
    protected float hoverOffset = 25f;  //how far to move the tab on hover/select
    protected float moveSpeed = 100f;   //how fast to move the tab on hover

    protected int tabIndex;         //tab index [0-total journal pages)
    protected RectTransform rectTransform;  //reference to the transform of the ui component
    protected Vector2 defaultPosition;      //unselected position
    protected Vector2 hoveredPosition;      //selected/hover position
    protected Vector2 targetPosition;       //position the tab is attempting to move towards
    protected TabState currentState = TabState.Default; //holds current states of the tab

    protected JournalController journalController;  //reference to the journal controller object

    /// <summary>
    /// set the tab index by the tab name, init transform and position
    /// </summary>
    protected virtual void Awake()
    {
        tabIndex = TabUtility.GetTabIndex(tabName);
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;
        hoveredPosition = defaultPosition + Vector2.left * (hoverOffset * (tabIndex == 4 ? -1 : 1));
        
        targetPosition = defaultPosition;

        journalController = FindFirstObjectByType<JournalController>();
    }

    /// <summary>
    /// moves the tab towards the target position over time
    /// </summary>
    protected virtual void Update()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
    /// <summary>
    /// On mouseover event, sets the tab to hovered state if it is not the selected tab
    /// </summary>
    protected virtual void OnPointerEnter()
    {
        if (currentState != TabState.Selected)
        {
            SetState(TabState.Hovered);
        }
    }
    /// <summary>
    /// pointer exit event, on hovers the tab if it is not the selected tab
    /// </summary>
    protected virtual void OnPointerExit()
    {
        if (currentState != TabState.Selected)
        {
            SetState(TabState.Default);
        }
    }

    /// <summary>
    /// Pass the tab index to the journal controller's tab click handler to change the page 
    /// </summary>
    protected virtual void OnPointerClick()
    {
        journalController.HandleTabClick(tabIndex);
    }

    /// <summary>
    /// Set the state of the tab
    /// </summary>
    /// <param name="newState">the tabstate name to set the state to</param>
    public void SetState(TabState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case TabState.Default:
                targetPosition = defaultPosition;
                break;
            case TabState.Hovered:
            case TabState.Selected:
                targetPosition = hoveredPosition;
                break;
        }
    }

    /// <summary>
    /// Set the tab to selected
    /// </summary>
    /// <param name="isSelected">If true, set the tab as selected, othewise set default</param>
    public void SetSelected(bool isSelected)
    {
        SetState(isSelected ? TabState.Selected : TabState.Default);
    }
}

/// <summary>
/// Custom attribute to help creating the tab name drop down in Unity inspector
/// </summary>
public class TabIndexAttribute : Attribute
{
    public int Index { get; }

    public TabIndexAttribute(int index)
    {
        Index = index;
    }
}
/// <summary>
/// Enum representing the tab name using the custon index attribute to map tab index to a tab name
/// </summary>
public enum TabName
{
    [TabIndex(0)] Notes,
    [TabIndex(1)] Graph,
    [TabIndex(2)] Artifacts,
    [TabIndex(3)] Lore,
    [TabIndex(4)] Settings
}
/// <summary>
/// Utility class to convert tab name to an index
/// </summary>
public static class TabUtility
{   
    /// <summary>
    /// Converts the tab name to the tab index
    /// </summary>
    /// <param name="tabName">the tab name enum</param>
    /// <returns>an integer index matching the tab name otherwise returns -1 if not found</returns>
    public static int GetTabIndex(TabName tabName)
    {
        var memberInfo = typeof(TabName).GetMember(tabName.ToString())[0];
        var attribute = (TabIndexAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(TabIndexAttribute));
        return attribute?.Index ?? -1;
    }
}