using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonState
    {
        Default,
        Hover,
        Click
    }

    [System.Serializable]
    public class ButtonStateImages
    {
        public Sprite defaultImage;
        public Sprite hoverImage;
        public Sprite clickImage;
    }

    [SerializeField] private ButtonStateImages stateImages;
    [SerializeField] private UnityEvent onClick;

    private Image buttonImage;
    private ButtonState currentState = ButtonState.Default;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        switch (currentState)
        {
            case ButtonState.Default:
                buttonImage.sprite = stateImages.defaultImage;
                break;
            case ButtonState.Hover:
                buttonImage.sprite = stateImages.hoverImage;
                break;
            case ButtonState.Click:
                buttonImage.sprite = stateImages.clickImage;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentState = ButtonState.Hover;
        UpdateButtonState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentState = ButtonState.Default;
        UpdateButtonState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentState = ButtonState.Click;
        UpdateButtonState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentState = ButtonState.Hover;
        UpdateButtonState();

        // Trigger click event
        onClick.Invoke();
    }
}