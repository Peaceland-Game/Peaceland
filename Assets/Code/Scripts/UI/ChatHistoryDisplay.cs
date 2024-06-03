using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;


public class ChatHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI chatHistoryText;
    public ConversationLogger conversationLogger;

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
    }

    private void OnDestroy()
    {
        if (conversationLogger != null)
        {
            conversationLogger.onDialogueUpdated.RemoveListener(UpdateChatHistory);
        }
    }
}
