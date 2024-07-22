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
        HidePopup();
        if(revealed)
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

    public void ShowPopup()
    {
        if(revealed)
        {
            textBox.sprite = hoverTextBox;
            hoverPopup.gameObject.SetActive(true);
            description.gameObject.SetActive(true);
        }
    }

    public void HidePopup()
    {
        if (revealed)
        {
            textBox.sprite = revealedTextBox;
            hoverPopup.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
        }
    }

    public void RevealTheme()
    {
        revealed = true;

        textBox.sprite = revealedTextBox;
        lockImage.sprite = revealedUnlock;
        title.gameObject.SetActive(true);
    }

    public void HideTheme()
    {
        revealed = false;

        textBox.sprite = darkTextBox;
        lockImage.sprite = lockDark;
        title.gameObject.SetActive(false);
    }
}
