using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the behavior of a main button in a side panel UI,
/// controlling the visibility of associated UI pages.
/// </summary>
public class SidePanelMainButton : MonoBehaviour
{
    public GameObject pageToEnable;
    public List<GameObject> pagesToDisable;
    /// <summary>
    /// Handles the click event for this button.
    /// Enables the specified page and disables all other pages in the list.
    /// </summary>
    public void OnClickingThisButton()
    {
        pageToEnable.SetActive(true);
        for(int i = 0; i < pagesToDisable.Count; i++)
        {
            pagesToDisable[i].SetActive(false);
        }
    }
}
