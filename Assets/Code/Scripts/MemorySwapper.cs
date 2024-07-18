using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

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
    public bool LoadMemoryOnStart = true;
    public int MemoryIndex = 0;
    [SerializeField] private UserInterface userInterface;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private bool controlLighting = false;
    [SerializeField] private DialogueSkipper skipper;
    

    void Start()
    {
        if (!Instance)
        {
            Instance = this;
            if (LoadMemoryOnStart)
                SwitchToMemory(MemoryIndex);
            if (!userInterface)
            {
                userInterface = GameObject.FindWithTag("UI").GetComponent<UserInterface>();
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public bool HasLevel(int index) {
        return index >= 0 && index < memoryObjects.Count;
    }

    public void SwitchToMemory(double index)
    {
        userInterface.EnableLoadScreen();
        playerController.enabled = false;

        var ind = (int)Mathf.Floor((float)index);
        if (ind < 0 || ind >= memoryObjects.Count)
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
        MemoryObjectPair selectedMemory = memoryObjects[ind];
        if (selectedMemory.memoryObject != null)
        {
            selectedMemory.memoryObject.SetActive(true);
            
        }
        if (controlLighting)
        {
            // Change the lighting profile
            if (lightingController != null)
            {
                lightingController.TransitionToProfile(selectedMemory.timeOfDay, 1f, userInterface, () => { playerController.enabled = true; }); // 1 second transition
            }
        }
        else
        {
#if (UNITY_EDITOR)
            StartCoroutine(WaitThen(1, skipper.Skip));
#endif
            playerController.enabled = true;
            userInterface.DisableLoadScreen();
        }
        
    }
    IEnumerator WaitThen(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(SwitchToMemory), this, SymbolExtensions.GetMethodInfo(() => SwitchToMemory((double)0)));
        // Lua.RegisterFunction(nameof(AddOne), this, SymbolExtensions.GetMethodInfo(() => AddOne((double)0)));
    }

    void OnDisable()
    {
        if (true)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(nameof(SwitchToMemory));
            //   Lua.UnregisterFunction(nameof(AddOne));
        }
    }

}
