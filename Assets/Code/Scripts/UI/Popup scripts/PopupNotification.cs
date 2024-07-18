using System.Collections;
using UnityEngine;
using TMPro;

public class PopupNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] float popupDuration = 1.5f;
    [SerializeField] float animationDuration = 0.5f;
    float xOffsetPercentage = 0.2f; 
    float xStopLocation;
    [SerializeField] RectTransform popup;
    [SerializeField] bool hasText = true;
    public bool HasText { get { return hasText; } }

    private Vector2 startPos;

    void Start()
    {
        //Debug.Log(xOffsetPercentage);

        xStopLocation = -Screen.width / 2f + Screen.width * xOffsetPercentage;

        startPos = popup.anchoredPosition;
        StartCoroutine(AnimatePopup());
    }

    public void UpdateArtifactName(string newName)
    {
        artifactName.text = newName;
    }

    public float GetTotalNotifcationTime()
    {
        return animationDuration * 2 + popupDuration;
    }

    private IEnumerator AnimatePopup()
    {
        Vector2 startPosition = startPos;
        Vector2 endPosition = new Vector2(xStopLocation, startPosition.y);
     //   Debug.Log($"Start Position: {startPosition}, End Position: {endPosition}");
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // Apply easing function (ease-out cubic)
            float easedT = 1 - Mathf.Pow(1 - t, 3);

            popup.anchoredPosition = Vector3.Lerp(startPosition, endPosition, easedT);
            yield return null;
        }

        // Ensure the popup ends exactly at the target position
        popup.anchoredPosition = endPosition;

        // Wait for the popup duration
        yield return new WaitForSeconds(popupDuration);

        // Animate out (optional)
        yield return StartCoroutine(AnimatePopupOut());

        // Destroy the popup
        Destroy(gameObject);
    }

    private IEnumerator AnimatePopupOut()
    {
        Vector2 startPosition = popup.anchoredPosition;
        Vector2 endPosition = startPos;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // Apply easing function (ease-in cubic)
            float easedT = t * t * t;

            popup.anchoredPosition = Vector3.Lerp(startPosition, endPosition, easedT);
            yield return null;
        }

        
        Destroy(gameObject);
    }
}