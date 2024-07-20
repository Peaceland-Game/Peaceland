using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

public class MemoryChatLog : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> entryTextMeshes = new();
    [SerializeField] private Image backgroundImage;

    public void SetEntryText(string speaker, string dialogue) {
        entryTextMeshes[0].text = speaker;
        entryTextMeshes[1].text = dialogue;
        LayoutRebuilder.ForceRebuildLayoutImmediate(entryTextMeshes[1].rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundImage.rectTransform);
    }
    public void AppendDialogue(string dialogue) {
        entryTextMeshes[1].text += dialogue;
        LayoutRebuilder.ForceRebuildLayoutImmediate(entryTextMeshes[1].rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundImage.rectTransform);
    }
}
