using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDownstairs : Interactable
{
    [SerializeField] Transform target;

    public override void Interact(Transform source)
    {
        source.transform.position = target.position;
    }
}
