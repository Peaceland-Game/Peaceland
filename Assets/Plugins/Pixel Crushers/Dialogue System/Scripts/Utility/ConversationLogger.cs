// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// When you attach this script to an actor, conversations involving that actor will be
    /// logged to the console.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ConversationLogger : MonoBehaviour
    {

        public List<string> dialogueHistory = new();
        public UnityEvent onDialogueUpdated;

        [Tooltip("Log player lines in this color.")]
        public Color playerColor = Color.blue;

        [Tooltip("Log NPC lines in this color.")]
        public Color npcColor = Color.red;

        public void OnConversationStart(Transform actor)
        {
            Debug.Log(string.Format("{0}: Starting conversation with {1}", new object[] { name, GetActorName(actor) }));
        }

        public void OnConversationLine(Subtitle subtitle)
        {
            if (subtitle == null || subtitle.formattedText == null || string.IsNullOrEmpty(subtitle.formattedText.text)) return;

            string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            string dialogueLine = string.Format("{0}: {1}", speakerName, subtitle.formattedText.text);
            dialogueHistory.Add(dialogueLine);
            onDialogueUpdated.Invoke();
            Debug.Log(string.Format("<color={0}>{1}</color>", GetActorColor(subtitle), dialogueLine));



        }
        private void Awake()
        {
            if (onDialogueUpdated == null)
                onDialogueUpdated = new UnityEvent();
        }

        public List<string> GetDialogueHistory()
        {
            return dialogueHistory;
        }
        public void OnConversationEnd(Transform actor)
        {
            Debug.Log(string.Format("{0}: Ending conversation with {1}", name, GetActorName(actor)));
        }

        private string GetActorName(Transform actor)
        {
            return (actor != null) ? actor.name : "(null transform)";
        }

        private string GetActorColor(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.speakerInfo == null) return "white";
            return Tools.ToWebColor(subtitle.speakerInfo.isPlayer ? playerColor : npcColor);
        }


        public void OnPrepareConversationLine(DialogueEntry entry)
        {
            if (entry == null) return;
            Debug.Log(string.Format("Preparing line {0}", entry.currentDialogueText));
        }

        public void OnConversationLineCancelled(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            Debug.Log(string.Format("<color={0}>Line cancelled - {1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
        }

        public void OnConversationLineEnd(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            Debug.Log(string.Format("<color={0}>Line ended - {1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
        }

        public void OnConversationResponseMenu(Response[] responses)
        {
            Debug.Log("Showing conversation response menu.");
        }

        public void OnConversationTimeout()
        {
            Debug.Log("Conversation timed out.");
        }

        public void OnLinkedConversationStart(Transform actor)
        {
            Debug.Log("Starting linked conversation.");
        }

    }

}
