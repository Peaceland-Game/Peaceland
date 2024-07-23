using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

/// <summary>
/// Manages the display of chat log entries in a memory or dialogue system,
/// allowing for setting and appending dialogue text.
/// </summary>
public class MemoryChatLog : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> entryTextMeshes = new();
    [SerializeField] private Image backgroundImage;

    /// <summary>
    /// Sets the text for a chat entry with a speaker and dialogue.
    /// </summary>
    /// <param name="speaker">The name of the speaker.</param>
    /// <param name="dialogue">The dialogue text.</param>
    public void SetEntryText(string speaker, string dialogue) {
        entryTextMeshes[0].text = speaker;
        entryTextMeshes[1].text = dialogue;
        LayoutRebuilder.ForceRebuildLayoutImmediate(entryTextMeshes[1].rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundImage.rectTransform);
    }
    /// <summary>
    /// Appends additional dialogue text to the existing entry.
    /// </summary>
    /// <param name="dialogue">The dialogue text to append.</param>
    public void AppendDialogue(string dialogue) {
        entryTextMeshes[1].text += dialogue;
        LayoutRebuilder.ForceRebuildLayoutImmediate(entryTextMeshes[1].rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundImage.rectTransform);
    }
}
