using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Depricated, was used to display the chat history, verbatim in the journal, replaced by notes system
/// use as reference for logging dialogue 
/// </summary>
public class ChatHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI chatHistoryText; //reference to the chat history GUI text object
    public ConversationLogger conversationLogger;   //reference to the COnversationLogger component inthe scene 
    public ScrollToBottom scrollToBottom;  // Reference to the ScrollToBottom script

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

        if (scrollToBottom != null)
        {

            Debug.Log("scrolling to bottom on text update");
        }
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
