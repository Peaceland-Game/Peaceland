using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTreePhoto : MonoBehaviour
{

    public Transform cameraAnchor;
    public GameObject descriptionInterface;
    public bool IsComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDescription(bool active)
    {
        descriptionInterface.SetActive(active);
    }
}
