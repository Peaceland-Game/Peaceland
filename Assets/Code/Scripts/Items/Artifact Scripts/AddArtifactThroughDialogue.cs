using UnityEngine;
using PixelCrushers.DialogueSystem;

// Rename this class to the same name that you used for the script file.
// Add the script to your Dialogue Manager. You can optionally make this 
// a static class and remove the inheritance from MonoBehaviour, in which
// case you won't add it to the Dialogue Manager.
public class AddArtifactThroughDialogue : MonoBehaviour // Rename this class.
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;

    void OnEnable()
    {
        Lua.RegisterFunction(nameof(AddArtifact), this, SymbolExtensions.GetMethodInfo(() => AddArtifact(string.Empty)));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            Lua.UnregisterFunction(nameof(AddArtifact));
        }
    }

    public void AddArtifact(string name)
    {
        JournalPlayerRef journalPlayerRef = GameObject.FindWithTag("Artifacts").GetComponent<JournalPlayerRef>();

        journalPlayerRef.AddArtifact(name);
    }
}
