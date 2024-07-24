using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Tablet;

/// <summary>
/// UI element that controls the dropdown of the side panel present in the notes page.
/// </summary>
public class TabletSidePanelDropdown : MonoBehaviour
{
    [SerializeField] Button button; //the button that should be clicked to toggle the dropdown
    [SerializeField] List<GameObject> dropdownButtons; //the buttons featured in the dropdown itself
    [SerializeField] Sprite upArrow; //the sprite present when the dropdown is toggled on
    [SerializeField] Sprite downArrow; //the sprite present when the dropdown is toggled off
    [SerializeField] Image currentArrow; //the current sprite
    [SerializeField] Image dropDownImage; //the dark background present as part of the dropdown
    bool currentlyActive; //whether the dropdown is currently toggled on or not
    // Start is called before the first frame update
    void Start()
    {
        currentlyActive = false;
        dropDownImage.enabled = currentlyActive;
        for (int i = 0; i < dropdownButtons.Count; i++)
        {
            dropdownButtons[i].SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the memory button visibility when called
    /// </summary>
    public void ToggleDropDownButtons()
    {
        currentlyActive = !currentlyActive;
        if (currentlyActive)
        {
            currentArrow.sprite = upArrow;
        }
        else currentArrow.sprite = downArrow;
        for(int i = 0; i < dropdownButtons.Count; i++) 
        {
            dropdownButtons[i].SetActive(currentlyActive);
        }
        dropDownImage.enabled = currentlyActive;
    }
}
