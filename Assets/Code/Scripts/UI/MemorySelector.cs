using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySelector : MonoBehaviour
{
    List<MemoryString> memoryStrings = new();
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMemoryString(MemoryString memoryString)
    {
        Debug.Log($"selected {memoryString.name}");
    }

    

}
