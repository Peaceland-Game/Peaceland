using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CustomButton : Button
{
    public enum ButtonState
    {
        Default,
        Hover,
        Click
    }

    [SerializeField]
    private List<Sprite> stateImages = new List<Sprite>(3);

    private ButtonState currentState = ButtonState.Default;
    private Image buttonImage;

    protected override void Awake()
    {
        base.Awake();
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("CustomButton requires an Image component");
        }
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (buttonImage == null || stateImages.Count < 3) return;

        switch (currentState)
        {
            case ButtonState.Default:
                buttonImage.sprite = stateImages[0];
                break;
            case ButtonState.Hover:
                buttonImage.sprite = stateImages[1];
                break;
            case ButtonState.Click:
                buttonImage.sprite = stateImages[2];
                break;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        currentState = ButtonState.Hover;
        UpdateButtonState();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        currentState = ButtonState.Default;
        UpdateButtonState();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        currentState = ButtonState.Click;
        UpdateButtonState();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        currentState = interactable ? ButtonState.Hover : ButtonState.Default;
        UpdateButtonState();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        switch (state)
        {
            case SelectionState.Normal:
                currentState = ButtonState.Default;
                break;
            case SelectionState.Highlighted:
                currentState = ButtonState.Hover;
                break;
            case SelectionState.Pressed:
                currentState = ButtonState.Click;
                break;
            case SelectionState.Selected:
                currentState = ButtonState.Hover;
                break;
            case SelectionState.Disabled:
                currentState = ButtonState.Default;
                break;
        }

        UpdateButtonState();
    }
}