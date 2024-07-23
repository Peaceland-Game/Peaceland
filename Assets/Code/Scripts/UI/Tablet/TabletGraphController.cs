using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the graph functionality within the tablet interface,
/// including displaying and hiding popups for graph points.
/// </summary>
public class TabletGraphController : MonoBehaviour {
    public List<GraphPointButton> graphPoints = new();
    [SerializeField] private GameObject graphPointPrefab;
    [SerializeField] private GameObject GraphPopup;
    [SerializeField] private TextMeshProUGUI GraphPopupTitle;
    [SerializeField] private TextMeshProUGUI GraphPopupSummary;

    bool popupActive = false;
    public static TabletGraphController Instance;
    /// <summary>
    /// Initializes the TabletGraphController as a singleton instance.
    /// </summary>
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    /// <summary>
    /// Closes the graph point popup.
    /// </summary>
    public void ClosePopup() {
        popupActive = false;
        GraphPopup.SetActive(false);
    }
    /// <summary>
    /// Opens the graph point popup with information from the specified index.
    /// </summary>
    /// <param name="index">The index of the graph point to display.</param>
    public void OpenPopup(int index) {
        if (popupActive) {
            ClosePopup();
        }
        GraphPopupTitle.text = graphPoints[index].title;
        GraphPopupSummary.text = graphPoints[index].summary;
        GraphPopup.SetActive(true);
        popupActive = true;
    }
    /// <summary>
    /// Opens the graph point popup with information from the specified GraphPointButton.
    /// </summary>
    /// <param name="button">The GraphPointButton containing the information to display.</param>
    public void OpenPopup(GraphPointButton button) {
        if (popupActive) {
            ClosePopup();
        }
        GraphPopupTitle.text = button.title;
        GraphPopupSummary.text = button.summary;
        GraphPopup.SetActive(true);
        popupActive = true;
    }


}




