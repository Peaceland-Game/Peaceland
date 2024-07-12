using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPointButton : MonoBehaviour {
    public string title;
    public string summary;

    private void Start() {
        // Add this button to the list of graph points
        TabletGraphController.Instance.graphPoints.Add(this);

        // Add a listener to the button
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            TabletGraphController.Instance.OpenPopup(this);
        });
    }

}
