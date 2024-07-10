using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{

    [SerializeField] DialogueState state;
    [SerializeField] RectTransform dialogueBox;
    [SerializeField] RectTransform choicesParent;

    [Header("Transition")]
    [SerializeField] float transitionTime;
    [SerializeField] AnimationCurve transitionPosCurve;
    [SerializeField] Vector3 dialogueBoxDefaultPos;
    [SerializeField] Vector3 dialogueBoxChoicePos;
    [SerializeField] AnimationCurve transitionScaCurve;
    [SerializeField] Vector3 dialogueBoxDefaultSca;
    [SerializeField] Vector3 dialogueBoxChoiceSca;

    private void Start()
    {
        StartCoroutine(Transition(true));
    }

    private IEnumerator Transition(bool toChoice)
    {
        float timer = 0.0f;
        while(timer < transitionTime)
        {
            // Lerp between having the dialogue box in the default
            // position and the choice 

            float lerp = timer / transitionTime;

            dialogueBox.localPosition = toChoice ?
                Vector3.LerpUnclamped(dialogueBoxDefaultPos, dialogueBoxChoicePos, transitionPosCurve.Evaluate(lerp)) :
                Vector3.LerpUnclamped(dialogueBoxChoicePos, dialogueBoxDefaultPos, transitionPosCurve.Evaluate(lerp));

            dialogueBox.localScale = toChoice ?
                Vector2.LerpUnclamped(dialogueBoxDefaultSca, dialogueBoxChoiceSca, transitionScaCurve.Evaluate(lerp)) :
                Vector2.LerpUnclamped(dialogueBoxChoiceSca, dialogueBoxDefaultSca, transitionScaCurve.Evaluate(lerp));

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private enum DialogueState
    {
        TYPING,     // Writing text to the dialogue box 
        IDLE,       // Waiting for player's next input 
        CHOOSING    // Player is choosing one of their givin options 
    }
}
