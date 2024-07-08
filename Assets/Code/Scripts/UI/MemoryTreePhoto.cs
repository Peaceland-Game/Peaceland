using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a memory tree photo, holds a reference to the position to move the camera to
/// and to the description game object on the UI
/// </summary>
public class MemoryTreePhoto : MonoBehaviour
{

    public Transform cameraAnchor;
    public GameObject descriptionInterface;
    public bool IsComplete = false;
    
    /// <summary>
    /// Sets the description as active or not
    /// </summary>
    /// <param name="active">true to enable the description UI</param>
    public void ToggleDescription(bool active)
    {
        descriptionInterface.SetActive(active);
    }
}
