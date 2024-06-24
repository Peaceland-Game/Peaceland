using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(Transform source)
    {
        print(name + " has been interacted with");
    }
}
