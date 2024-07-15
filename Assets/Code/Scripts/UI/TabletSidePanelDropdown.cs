using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletSidePanelDropdown : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button button;
    [SerializeField] List<GameObject> dropdownButtons;
    [SerializeField] TextMeshProUGUI arrowText;
    [SerializeField] Image dropDownImage;
    bool currentlyActive;
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
            arrowText.text = "▲";
        }
        else arrowText.text = "▼";
        for(int i = 0; i < dropdownButtons.Count; i++) 
        {
            dropdownButtons[i].SetActive(currentlyActive);
        }
        dropDownImage.enabled = currentlyActive;
    }
}
