using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteClick : ClickExample
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

        hm.GenerateHeadline(this.transform.GetSiblingIndex());
    }
}
