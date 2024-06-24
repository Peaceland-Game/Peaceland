using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

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

    public GameObject npcSubtitlePanel;
    public GameObject npcSubtitlePanelLong;

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
            //subtitlePanel.sizeDelta = subtitlePanelExpandedSize;
            //subtitlePanelText.sizeDelta = subtitlePanelTextExpandedSize;
            npcSubtitlePanel.SetActive(false);
            npcSubtitlePanelLong.SetActive(true);
            // menuPanel.gameObject.SetActive(false);
        }
        else
        {
            // Responses available, revert to default size
            Debug.Log("revert subtitle panel");
            //subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
            //subtitlePanelText.sizeDelta = subtitlePanelTextDefaultSize;
            npcSubtitlePanel.SetActive(true);
            npcSubtitlePanelLong.SetActive(false);
            //  menuPanel.gameObject.SetActive(true);
        }
    }

    private void OnConversationEnd(Transform actor)
    {
        // Revert to default size when conversation ends
        //subtitlePanel.sizeDelta = subtitlePanelDefaultSize;
        //subtitlePanelText.sizeDelta = subtitlePanelTextDefaultSize;
        npcSubtitlePanel.SetActive(true);
        npcSubtitlePanelLong.SetActive(false); ;
        //speakerName.anchoredPosition = new Vector2(75, 43);
    }
}
