using System.Collections;
using UnityEngine;
using TMPro;

public class PopupNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] float popupDuration = 2f;
    [SerializeField] float animationDuration = .75f;
    [SerializeField] float xOffsetPercentage = 0.175f;
    [SerializeField] RectTransform popup;
    [SerializeField] bool hasText = true;
    public bool HasText { get { return hasText; } }

    private Vector2 startPos;
    private Vector2 endPos;

    void Start()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("PopupNotification must be child of a Canvas");
            return;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;

        startPos = popup.anchoredPosition;
        endPos = new Vector2(canvasWidth * xOffsetPercentage - canvasWidth / 2, startPos.y);

        //endPos = new Vector2(startPos.x + 400, startPos.y);
        StartCoroutine(AnimatePopup());
        
    }
    //private void Update()
    //{
    //    popup.anchoredPosition = new Vector2(popup.anchoredPosition.x +1, popup.anchoredPosition.y);
    //    if (popup.anchoredPosition.x > 1000)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void UpdateArtifactName(string newName)
    {
        if (artifactName != null)
            artifactName.text = newName;
    }

    public float GetTotalNotifcationTime()
    {
        return animationDuration * 2 + popupDuration;
    }

    private IEnumerator AnimatePopup()
    {
        yield return StartCoroutine(AnimatePopupMovement(startPos, endPos));
        yield return new WaitForSeconds(popupDuration);
        yield return StartCoroutine(AnimatePopupMovement(endPos, startPos));
        Destroy(gameObject);
    }

    private IEnumerator AnimatePopupMovement(Vector2 start, Vector2 end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float easedT = 1 - Mathf.Pow(1 - t, 3); // Ease-out cubic

            popup.anchoredPosition = Vector2.Lerp(start, end, easedT);
            yield return null;
        }
        popup.anchoredPosition = end;
    }
}