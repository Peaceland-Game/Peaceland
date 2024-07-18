using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlePositioner : MonoBehaviour
{
    [SerializeField] Transform buttonsParent;

    [SerializeField] float speed = 1.0f;
    [SerializeField] AnimationCurve transitionPosCurve;
    [SerializeField] Vector3 dialogueBoxDefaultPos;
    [SerializeField] Vector3 dialogueBoxChoicePos;
    [SerializeField] AnimationCurve transitionScaCurve;
    [SerializeField] Vector3 dialogueBoxDefaultSca;
    [SerializeField] Vector3 dialogueBoxChoiceSca;

    private float lerp = 0.0f;


    void Update()
    {
        UpdateLerp();

        this.transform.localPosition = Vector3.LerpUnclamped(
            dialogueBoxDefaultPos, 
            dialogueBoxChoicePos, 
            transitionPosCurve.Evaluate(lerp));

        this.transform.localScale = Vector3.LerpUnclamped(
            dialogueBoxDefaultSca,
            dialogueBoxChoiceSca,
            transitionScaCurve.Evaluate(lerp));
    }

    /// <summary>
    /// Updates the lerp value based on whether there
    /// are buttons or not to transition caption UI 
    /// </summary>
    private void UpdateLerp()
    {
        // Are there any active buttons? 
        for (int i = 0; i < buttonsParent.childCount; i++)
        {
            if (buttonsParent.GetChild(i).gameObject.activeInHierarchy)
            {
                lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);
                return;
            }
        }

        lerp = Mathf.Clamp01(lerp - Time.deltaTime * speed);
    }
}
