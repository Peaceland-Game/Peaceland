using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Tablet;

public class TabletSidePanelDropdown : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] List<GameObject> dropdownButtons;
    [SerializeField] Sprite upArrow;
    [SerializeField] Sprite downArrow;
    [SerializeField] Image currentArrow;
    [SerializeField] Image dropDownImage;
    bool currentlyActive;
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

    // Update is called once per frame
    void Update()
    {

    }

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
