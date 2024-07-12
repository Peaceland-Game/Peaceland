using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletGraphController : MonoBehaviour {
    public List<GraphPointButton> graphPoints = new();
    [SerializeField] private GameObject graphPointPrefab;
    [SerializeField] private GameObject GraphPopup;
    [SerializeField] private TextMeshProUGUI GraphPopupTitle;
    [SerializeField] private TextMeshProUGUI GraphPopupSummary;

    bool popupActive = false;
    public static TabletGraphController Instance;
    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {

    }


    public void ClosePopup() {
        popupActive = false;
        GraphPopup.SetActive(false);
    }

    public void OpenPopup(int index) {
        if (popupActive) {
            ClosePopup();
        }
        GraphPopupTitle.text = graphPoints[index].title;
        GraphPopupSummary.text = graphPoints[index].summary;
        GraphPopup.SetActive(true);
        popupActive = true;
    }

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




