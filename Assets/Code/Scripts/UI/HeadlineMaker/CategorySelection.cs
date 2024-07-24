using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelection : MonoBehaviour
{
    [SerializeField] bool isTopic; // TODO: Find better automatic way for this 

    [Header("Animation")]
    [SerializeField] RectTransform highlight;
    [SerializeField] AnimationCurve highlightCurve;
    [SerializeField] float speed;

    float lerp = 0.0f;
    float targetWidth;


    HeadlineMaker hm { get; set; }


    void Start()
    {
        hm = this.GetComponentInParent<HeadlineMaker>();
        targetWidth = highlight.rect.width;
    }

    private void Update()
    {
        UpdateLerp();

        float l = highlightCurve.Evaluate(lerp);
        float width = Mathf.Lerp(0.0f, targetWidth, l);

        highlight.sizeDelta = new Vector2(width, highlight.sizeDelta.y);
        highlight.localPosition = highlight.right * (width / 2.0f - targetWidth / 2.0f);
    }

    /// <summary>
    /// Animates whether the highlight is appearing 
    /// or disspearing according to the idnex of 
    /// the currently selected topic/note 
    /// </summary>
    private void UpdateLerp()
    {
        if ((isTopic ? hm.SelectedTopic : hm.SelectedNote) == this.transform.GetSiblingIndex())
        {
            lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);
            return;
        }

        lerp = Mathf.Clamp01(lerp - Time.deltaTime * speed);
    }
}
