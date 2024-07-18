using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System;


/// <summary>
/// Controls a single tab in the journal
/// </summary>
public class TabController : MonoBehaviour
{   
    
    [SerializeField] protected TabName tabName;
    protected int tabIndex;         //tab index [0-total journal pages)
    protected Tablet tablet;  //reference to the journal controller object

    /// <summary>
    /// set the tab index by the tab name, init transform and position
    /// </summary>
    protected virtual void Awake()
    {
        tabIndex = TabUtility.GetTabIndex(tabName);
        tablet = FindFirstObjectByType<Tablet>();
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
    [TabIndex(0)] Home,
    [TabIndex(1)] Notes,
    [TabIndex(2)] Graph,
    [TabIndex(3)] Artifacts,
    [TabIndex(4)] Lore,
    [TabIndex(5)] Map,
    [TabIndex(6)] Settings

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