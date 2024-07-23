using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Theme : MonoBehaviour
{
    [SerializeField] private Image lockImage; //reference to the UI image game object for the lock
    [SerializeField] private Sprite lockDark; //"hidden" lock silhoutte
    [SerializeField] private Sprite revealedUnlock; //the revealed unlocked lock

    [SerializeField] private Image textBox; //reference to the UI image game object for the theme
    [SerializeField] private Sprite darkTextBox; //the hidden (black) text box
    [SerializeField] private Sprite revealedTextBox; //the revealed (green) text box
    [SerializeField] private Sprite hoverTextBox; //the (light green) text box that shows on hover
    [SerializeField] private Image hoverPopup; //the text box that shows up when hovered over a revealed theme
    [SerializeField] private TextMeshProUGUI title; //reference to the GUI text object for name of the theme
    [SerializeField] private TextMeshProUGUI description; //reference to the GUI text object for the theme description which shows up on the popup
    public string themeName;
    public bool revealed = false; //bool indicating if the object is shown or not
    // Start is called before the first frame update
    void Start()
    {
        textBox.sprite = revealedTextBox;
        hoverPopup.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        if (revealed)
        {
            textBox.sprite = revealedTextBox;
            lockImage.sprite = revealedUnlock;
            title.gameObject.SetActive(true);

        }
        else 
        {
            textBox.sprite = darkTextBox;
            lockImage.sprite = lockDark;
            title.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Currently commented out code that when uncommented will show a popup when hovering over a theme
    /// </summary>
    public void ShowPopup()
    {
        //uncomment below when we have an idea of what to do with the popup
        //if(revealed)
        //{
        //    textBox.sprite = hoverTextBox;
        //    hoverPopup.gameObject.SetActive(true);
        //    description.gameObject.SetActive(true);
        //}
    }

    /// <summary>
    /// Code meant to hide the popup that is currently not in use since ShowPopup() is commented out
    /// </summary>
    public void HidePopup()
    {
        if (revealed)
        {
            textBox.sprite = revealedTextBox;
            hoverPopup.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reveal the theme by enabling the text, and changing the sprites of the textbox and lock to 
    /// show that it is now unlocked.
    /// </summary>
    public void RevealTheme()
    {
        revealed = true;

        textBox.sprite = revealedTextBox;
        lockImage.sprite = revealedUnlock;
        title.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the theme by disabling the text, and changing the sprites of the textbox and lock to
    /// show that it is now locked.
    /// </summary>
    public void HideTheme()
    {
        revealed = false;

        textBox.sprite = darkTextBox;
        lockImage.sprite = lockDark;
        title.gameObject.SetActive(false);
    }
}
