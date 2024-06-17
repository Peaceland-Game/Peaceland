using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueUILayoutAdjuster : MonoBehaviour
{
    public RectTransform subtitlePanel;
  //  public RectTransform menuPanel;
    public Vector2 subtitlePanelExpandedSize;
    public Vector2 subtitlePanelDefaultSize;

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
           // menuPanel.gameObject.SetActive(false);
        }
        else
        {
            // Responses available, revert to default size
            Debug.Log("revert subtitle panel");
            subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
          //  menuPanel.gameObject.SetActive(true);
        }
    }

    private void OnConversationEnd(Transform actor)
    {
        // Revert to default size when conversation ends
        subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
    }
}
