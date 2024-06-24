using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public override void Interact(Transform source)
    {
        base.Interact(source);
        source.GetComponent<Inventory>().hasKey = true;
        Destroy(this.gameObject);
    }
}
