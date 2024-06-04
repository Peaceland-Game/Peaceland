using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;  // Assign the Scroll Rect component in the Inspector

    void Start()
    {
        // Optionally, scroll to the bottom at the start
        ScrollToBottomInstant();
    }

    public void ScrollToBottomInstant()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ScrollToBottomSmooth()
    {
        StartCoroutine(SmoothScrollToBottom());
    }

    private IEnumerator SmoothScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        float elapsedTime = 0.0f;
        float duration = 0.3f;  // Adjust duration as needed

        while (elapsedTime < duration)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = 0f;
    }
}
