using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopicClick : ClickExample
{
    private HeadlineMaker hm;

    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        hm = this.GetComponentInParent<HeadlineMaker>();
    }

    public override void TaskOnClick()
    {
        if (hm == null)
            return;

        hm.GenerateNotes(this.transform.GetSiblingIndex());
    }
}