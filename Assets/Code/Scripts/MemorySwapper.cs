using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeOfDay
{
    Day = 0,
    Evening = 1,
    Night = 2
}
[System.Serializable]
public struct MemoryObjectPair
{
    public GameObject memoryObject;
    public TimeOfDay timeOfDay;
}
public class MemorySwapper : MonoBehaviour
{

    public static MemorySwapper Instance;
    public DynamicLightingController lightingController;
    public List<MemoryObjectPair> memoryObjects = new List<MemoryObjectPair>();

    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SwitchToMemory(int index)
    {
        if (index < 0 || index >= memoryObjects.Count)
        {
            Debug.LogError("Invalid memory index");
            return;
        }

        // Disable all memory objects
        foreach (var memoryPair in memoryObjects)
        {
            if (memoryPair.memoryObject != null)
            {
                memoryPair.memoryObject.SetActive(false);
            }
        }

        // Enable the selected memory object
        MemoryObjectPair selectedMemory = memoryObjects[index];
        if (selectedMemory.memoryObject != null)
        {
            selectedMemory.memoryObject.SetActive(true);
        }

        // Change the lighting profile
        if (lightingController != null)
        {
            lightingController.TransitionToProfile(selectedMemory.timeOfDay, 2f); // 2 second transition
        }
    }


}
