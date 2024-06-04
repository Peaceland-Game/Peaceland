using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;


public class ChatHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI chatHistoryText;
    public ConversationLogger conversationLogger;
    public ScrollToBottom scrollToBottom;  // Reference to the ScrollToBottom script

    void Start()
    {
        if (conversationLogger != null)
        {
            conversationLogger.onDialogueUpdated.AddListener(UpdateChatHistory);
        }
        UpdateChatHistory();
    }

    public void UpdateChatHistory()
    {
        if (conversationLogger == null || chatHistoryText == null) return;

        List<string> history = conversationLogger.GetDialogueHistory();
        chatHistoryText.text = string.Join("\n", history.ToArray());

        if (scrollToBottom != null)
        {
            scrollToBottom.ScrollToBottomInstant();
            Debug.Log("scrolling to bottom on text update");
        }
    }

    private void OnDestroy()
    {
        if (conversationLogger != null)
        {
            conversationLogger.onDialogueUpdated.RemoveListener(UpdateChatHistory);
        }
    }
}
