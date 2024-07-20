using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

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

    public void ChatLogDisplay(int memoryNumber)
    {
        Debug.Log(memoryNumber);
        overviewPage.SetActive(false);
        memoryPage.SetActive(false);    
        chatLogs[memoryNumber].SetActive(true);
        for(int i = 0; i < chatLogs.Count; i++)
        {
            if (i != memoryNumber)
                chatLogs[i].SetActive(false);
        }
    }

    public void AddChatEntry(int memoryNumber, string speaker, string dialogue) {
        var parent = chatLogContentPanes[memoryNumber].transform;
        currentChatItem = Instantiate(chatItemPrefab, parent).GetComponent<MemoryChatLog>();

        currentChatItem.SetEntryText(speaker, dialogue);
        Debug.Log("Chat entry added");  
    }
    /// <summary>
    /// Attempt to update the dialogue history text element using the conversation logger
    /// </summary>
    public void UpdateChatHistory() {
        if (conversationLogger == null) return;

        //var history = conversationLogger.GetDialogueHistory();
        //chatHistoryText.text = string.Join("\n", history.ToArray());

        var lastEntry = conversationLogger.GetLastEntry();
        if (lastEntry == "") return;

        var parts = lastEntry.Split(new[] { ':' }, 2);
        if (parts.Length < 2) return;

        var speaker = parts[0];
        var dialogue = parts[1].Trim();

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
