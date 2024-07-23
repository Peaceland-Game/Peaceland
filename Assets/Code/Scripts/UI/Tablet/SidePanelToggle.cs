using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the behavior of a toggleable side panel in the UI,
/// controlling its visibility and movement.
/// </summary>
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

    [SerializeField] private float moveSpeed = 600f; // Speed of the movement
    //private float shownPosition = 150f; // The 'shown' position in local space

    private Vector3 hiddenPosition = new(1070.481f, 0, 0);
    private Vector3 shownPosition = new(767, 0, 0);


    /// <summary>
    /// Toggles the panel between shown and hidden states.
    /// </summary>
    public void TogglePanel()
    {
        if (currentState != PanelState.Moving)
        {
            targetState = (currentState == PanelState.Hidden) ? PanelState.Shown : PanelState.Hidden;
            currentState = PanelState.Moving;
            UpdateButtonText();
        }
    }
    /// <summary>
    /// Moves the tabs towards the target position based on the current state.
    /// </summary>
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
    /// <summary>
    /// Updates the button text based on the target state.
    /// </summary>
    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = (targetState == PanelState.Shown) ? ">" : "<";
        }
    }

    /// <summary>
    /// Updates the panel movement each frame when in the Moving state.
    /// </summary>
    void Update()
    {
        if (currentState == PanelState.Moving)
        {
            MoveTabs();
        }
    }
}
