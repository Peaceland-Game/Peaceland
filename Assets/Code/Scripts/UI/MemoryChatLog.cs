using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class MemoryChatLog : MonoBehaviour
{
    public TextMeshProUGUI chatHistoryText; //reference to the chat history GUI text object
    public ConversationLogger conversationLogger;   //reference to the ConversationLogger component inthe scene 

    /// <summary>
    /// Adds the conversation dialogue update listener on Start
    /// </summary>
    void Start()
    {
        if (conversationLogger != null)
        {
            conversationLogger.onDialogueUpdated.AddListener(UpdateChatHistory);
        }
        UpdateChatHistory();
    }

    /// <summary>
    /// Attempt to update the dialogue history text element using the conversation logger
    /// </summary>
    public void UpdateChatHistory()
    {
        if (conversationLogger == null || chatHistoryText == null) return;

        List<string> history = conversationLogger.GetDialogueHistory();
        chatHistoryText.text = string.Join("\n", history.ToArray());
    }
    /// <summary>
    /// Remove event listener
    /// </summary>
    private void OnDestroy()
    {
        if (conversationLogger != null)
        {
            conversationLogger.onDialogueUpdated.RemoveListener(UpdateChatHistory);
        }
    }
}
