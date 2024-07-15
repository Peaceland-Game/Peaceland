using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SidePanelToggle : MonoBehaviour
{
    public enum PanelState
    {
        Shown,
        Moving,
        Hidden
    };

    [Header("Panel Controls")]
    private PanelState currentState = PanelState.Hidden;
    private PanelState targetState;
    [SerializeField] GameObject tabs;
    public TextMeshProUGUI buttonText;

    private float moveSpeed = 300f; // Speed of the movement
    //private float shownPosition = 150f; // The 'shown' position in local space

    private Vector3 hiddenPosition = new(1070.481f, 0, 0);
    private Vector3 shownPosition = new(767, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"{transform.localPosition}");
        //Debug.Log($"{hiddenPosition}");
        //Debug.Log($"{shownPosition}");
    }

    public void TogglePanel()
    {
        if (currentState != PanelState.Moving)
        {
            targetState = (currentState == PanelState.Hidden) ? PanelState.Shown : PanelState.Hidden;
            currentState = PanelState.Moving;
            UpdateButtonText();
        }
    }

    private void MoveTabs()
    {
        Vector3 targetPosition = (targetState == PanelState.Shown) ? shownPosition : hiddenPosition;
        tabs.transform.localPosition = Vector3.MoveTowards(tabs.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(tabs.transform.localPosition, targetPosition) < 0.01f)
        {
            tabs.transform.localPosition = targetPosition;
            currentState = targetState;
        }
    }

    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = (targetState == PanelState.Shown) ? ">" : "<";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == PanelState.Moving)
        {
            MoveTabs();
        }
    }
}
