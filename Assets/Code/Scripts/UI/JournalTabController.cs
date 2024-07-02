using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System;


public class TabController : MonoBehaviour
{
    public enum TabState { Default, Hovered, Selected }

    [SerializeField] protected TabName tabName;
    protected float hoverOffset = 35f;
    protected float moveSpeed = 100f;

    protected int tabIndex;
    protected RectTransform rectTransform;
    protected Vector2 defaultPosition;
    protected Vector2 hoveredPosition;
    protected Vector2 targetPosition;
    protected TabState currentState = TabState.Default;

    protected JournalController journalController;

    protected virtual void Awake()
    {
        tabIndex = TabUtility.GetTabIndex(tabName);
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;
        hoveredPosition = defaultPosition + Vector2.left * hoverOffset;
        targetPosition = defaultPosition;

        journalController = GetComponentInParent<JournalController>();
    }

    protected virtual void Update()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    protected virtual void OnPointerEnter()
    {
        if (currentState != TabState.Selected)
        {
            SetState(TabState.Hovered);
        }
    }

    protected virtual void OnPointerExit()
    {
        if (currentState != TabState.Selected)
        {
            SetState(TabState.Default);
        }
    }

    protected virtual void OnPointerClick()
    {
        journalController.HandleTabClick(tabIndex);
    }

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

    public void SetSelected(bool isSelected)
    {
        SetState(isSelected ? TabState.Selected : TabState.Default);
    }
}

public class TabIndexAttribute : Attribute
{
    public int Index { get; }

    public TabIndexAttribute(int index)
    {
        Index = index;
    }
}
public enum TabName
{
    [TabIndex(0)] Notes,
    [TabIndex(1)] Graph,
    [TabIndex(2)] Artifacts,
    [TabIndex(3)] Lore,
    [TabIndex(4)] Settings
}
public static class TabUtility
{
    public static int GetTabIndex(TabName tabName)
    {
        var memberInfo = typeof(TabName).GetMember(tabName.ToString())[0];
        var attribute = (TabIndexAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(TabIndexAttribute));
        return attribute?.Index ?? -1;
    }
}