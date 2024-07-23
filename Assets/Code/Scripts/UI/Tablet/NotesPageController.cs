using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Controller for the notes page on the tablet
/// </summary>
public class NotesPageController : MonoBehaviour
{
    //memoryButton[0] would correspond to chatLog[0]
    public List<Button> memoryButtons;
    public List<GameObject> chatLogs;
    public List<GameObject> chatLogContentPanes;
    public GameObject overviewPage;
    public GameObject memoryPage;
    public GameObject chatItemPrefab;

    [SerializeField] private ConversationLogger conversationLogger;
    private string currentSpeaker;
    private MemoryChatLog currentChatItem;

    private Dictionary<string, string> lastLoggedDialogue = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        //if (conversationLogger != null) {
        //    conversationLogger.onDialogueUpdated.AddListener(UpdateChatHistory);
        //}
        UpdateChatHistory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// used by the buttons to select which memory chat log to display
    /// </summary>
    /// <param name="memoryNumber">int memory number to display</param>
    public void ChatLogDisplay(int memoryNumber)
    {
       // Debug.Log(memoryNumber);
        overviewPage.SetActive(false);
        memoryPage.SetActive(false);    
        chatLogs[memoryNumber].SetActive(true);
        for(int i = 0; i < chatLogs.Count; i++)
        {
            if (i != memoryNumber)
                chatLogs[i].SetActive(false);
        }
    }
    /// <summary>
    /// Adds a chat log entry by creating a new chat log item and setting the text
    /// </summary>
    /// <param name="memoryNumber">Which memory to add the chat log to</param>
    /// <param name="speaker">string name for the speaker of the line of dialogue</param>
    /// <param name="dialogue">the string line of dialogue</param>
    public void AddChatEntry(int memoryNumber, string speaker, string dialogue) {
        var parent = chatLogContentPanes[memoryNumber].transform;
        currentChatItem = Instantiate(chatItemPrefab, parent).GetComponent<MemoryChatLog>();

        currentChatItem.SetEntryText(speaker, dialogue);
       // Debug.Log("Chat entry added");  
    }
    /// <summary>
    /// Attempt to update the dialogue history text element using the conversation logger
    /// </summary>
    public void UpdateChatHistory() {
        if (conversationLogger == null) {
            Debug.LogError("Missing conversation logger on chat history update");
            return;
        }
        var lastEntry = conversationLogger.GetLastEntry();
        //Debug.Log("Last entry: " + lastEntry);
        if (lastEntry == "") return;

        var parts = lastEntry.Split(new[] { ':' }, 2);
        if (parts.Length < 2) return;

        var speaker = parts[0];
        var dialogue = parts[1].Trim();

        // Check if this dialogue is a duplicate for this speaker
        if (lastLoggedDialogue.TryGetValue(speaker, out string lastDialogue)) {
            if (lastDialogue == dialogue) {
                // This is a duplicate, so we ignore it
                return;
            }
        }

        // Update the last logged dialogue for this speaker
        lastLoggedDialogue[speaker] = dialogue;

        //if the speaker is the same as the previous line,
        //append the dialogue to the current chat item instead of creating a new one
        if (speaker == currentSpeaker) {
            currentChatItem.AppendDialogue(dialogue);
        }
        else {
            currentSpeaker = speaker;
            var memoryNumber = 0;
            AddChatEntry(memoryNumber, speaker, dialogue);
        }
    }

    /// <summary>
    /// Remove event listener
    /// </summary>
    private void OnDestroy() {
        //if (conversationLogger != null) {
        //    conversationLogger.onDialogueUpdated.RemoveListener(UpdateChatHistory);
        //}
    }


}
