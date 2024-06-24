using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialgoueInteractable : Interactable
{
    [SerializeField] List<string> text;

    [SerializeField] GameObject dialogueObject;
    [SerializeField] TextMeshPro textMesh;
    [Tooltip("If player walks away duringa conversation at the given distance, dialgoue will close")]
    [SerializeField] float endDialogueRange;

    [SerializeField][Range(0.0f, 1.0f)] float showTextThreshold;
    [Space]
    [SerializeField] Transform dialogueTrans;
    [SerializeField] float dialogueIncreaseRate;
    [SerializeField] float dialogueDecreaseRate;
    [SerializeField] Vector3 dialogueStartScale;
    [SerializeField] Vector3 dialogueTargetScale;
    [SerializeField] AnimationCurve dialogueCurveScaleX;
    [SerializeField] AnimationCurve dialogueCurveScaleY;



    private int currentText = 0;
    private float dialogueLerp;

    private Transform listener;

    private NPCState state;

    private void Start()
    {
        currentText = 0;
        dialogueLerp = 0.0f;
    }

    public override void Interact(Transform source)
    {
        base.Interact(source);

        this.listener = source;

        // Startup dialogue 
        state = NPCState.TALKING;
    }

    private void Update()
    {
        DialogueStateMachine(); // Logic
        DialogueVisual();       // Dialogue box visual 
    }

    private void DialogueStateMachine()
    {
        switch (state)
        {
            case NPCState.IDLE: // Wait for interact call 
                break;
            case NPCState.TALKING:
                DialogueLogic();
                break;
            case NPCState.END_CONVO:
                EndConversation();
                break;
        }
    }

    /// <summary>
    /// Manages what is happening during a conversation 
    /// </summary>
    private void DialogueLogic()
    {
        // Make sure player is within distance
        if(listener)
        {
            if (Vector3.Distance(listener.transform.position, this.transform.position) >= endDialogueRange)
            {
                ResetDialogueText();
                state = NPCState.END_CONVO;
            }
        }

        if (dialogueLerp < showTextThreshold)
            return;
        
        textMesh.text = text[currentText];

        if (Input.GetMouseButtonDown(0))
        {
            // Close dialogue if end reached
            if (currentText + 1 >= text.Count)
            {
                ResetDialogueText();
                state = NPCState.END_CONVO;

                return;
            }

            currentText++;
        }
    }

    /// <summary>
    /// Ends conversation and does not allow to start a new dialogue until close
    /// dialogue box has gone past threshold 
    /// </summary>
    private void EndConversation()
    {
        if(dialogueLerp <= showTextThreshold)
        {
            state = NPCState.IDLE;
            print("Hello");
        }
    }

    private void ResetDialogueText()
    {
        currentText = 0;
    }

    /// <summary>
    /// Animates the dialogue box 
    /// </summary>
    private void DialogueVisual()
    {
        if (state != NPCState.TALKING)
        {
            dialogueLerp = Mathf.Clamp01(dialogueLerp - dialogueDecreaseRate * Time.deltaTime);
            textMesh.text = "";
        }
        else
        {
            dialogueLerp = Mathf.Clamp01(dialogueLerp + dialogueIncreaseRate * Time.deltaTime);
        }

        float scaleX = Mathf.LerpUnclamped(dialogueStartScale.x, dialogueTargetScale.x, dialogueCurveScaleX.Evaluate(dialogueLerp));
        float scaleY = Mathf.LerpUnclamped(dialogueStartScale.y, dialogueTargetScale.y, dialogueCurveScaleY.Evaluate(dialogueLerp));
        dialogueTrans.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }

    private enum NPCState
    {
        IDLE,
        TALKING,
        END_CONVO
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, endDialogueRange);
    }
}
