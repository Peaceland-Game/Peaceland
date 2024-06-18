using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueUILayoutAdjuster : MonoBehaviour
{
    public RectTransform subtitlePanel;
    public RectTransform subtitlePanelText;
    public RectTransform speakerName;
  //  public RectTransform menuPanel;
    public Vector2 subtitlePanelExpandedSize;
    public Vector2 subtitlePanelDefaultSize;

    public Vector2 subtitlePanelTextExpandedSize;
    public Vector2 subtitlePanelTextDefaultSize;

    private DialogueSystemEvents dialogueSystemEvents;

    private void Start()
    {
        dialogueSystemEvents = FindObjectOfType<DialogueSystemEvents>();
        dialogueSystemEvents.conversationEvents.onConversationLine.AddListener(OnConversationLine);
        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener(OnConversationEnd);
    }

    private void OnConversationLine(Subtitle subtitle)
    {
        if (DialogueManager.currentConversationState.subtitle != null &&
            !DialogueManager.currentConversationState.hasPCResponses)
        {
            // No responses, expand subtitle panel
            Debug.Log("Expand subtitle panel");
            subtitlePanel.sizeDelta = subtitlePanelExpandedSize;
            subtitlePanelText.sizeDelta = subtitlePanelTextExpandedSize;
            
            //hard coding the values atm should probably figure out a way to not
            speakerName.anchoredPosition = new Vector2(653, -18);
            // menuPanel.gameObject.SetActive(false);
        }
        else
        {
            // Responses available, revert to default size
            Debug.Log("revert subtitle panel");
            subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
            subtitlePanelText.sizeDelta = subtitlePanelTextDefaultSize;

            speakerName.anchoredPosition = new Vector2(353, -18);
          //  menuPanel.gameObject.SetActive(true);
        }
    }

    private void OnConversationEnd(Transform actor)
    {
        // Revert to default size when conversation ends
        subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
        speakerName.anchoredPosition = new Vector2(353, -18);
    }
}
