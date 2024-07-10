using PixelCrushers.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class UsableWithOutline : Usable
{
    private Outline outline;

    public override void Start()
    {
        base.Start();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public override void OnSelectUsable()
    {
        base.OnSelectUsable();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public override void OnDeselectUsable()
    {
        base.OnDeselectUsable();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    public override void OnUseUsable()
    {
        
        Destroy(gameObject);
    }
}