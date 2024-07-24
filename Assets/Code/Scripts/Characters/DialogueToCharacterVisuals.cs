using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueToCharacterVisuals : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] List<CharacterVisualController> characters;

    [Header("LUA")]
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;

    public Dictionary<string, CharacterVisualController> nameToVisuals;


    void OnEnable()
    {
        // Make the functions available to Lua
        Lua.RegisterFunction(nameof(ChangeEmotionalState), this, SymbolExtensions.GetMethodInfo(() => ChangeEmotionalState(0, "")));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua
            Lua.UnregisterFunction(nameof(ChangeEmotionalState));
        }
    }

    private void Start()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            nameToVisuals.Add(characters[i].gameObject.name, characters[i]);
        }
    }

    /// <summary>
    /// Changes the given character's emotion state to the passed enum. 
    /// Make sure that next is a valid enum index  
    /// </summary>
    /// <param name="next"></param>
    public void ChangeEmotionalState(double next, string character)
    {
        //Holds the actual game object that has the same name as the character
        GameObject actor = GameObject.Find(character);
        //The visual controller to help change expressions for the actor
        CharacterVisualController visualController = null;

        //As long as the actor was found, assign its visual controller to the corresponding variable
        if (actor != null)
        {
            visualController = actor.GetComponent<CharacterVisualController>();
        }

        //If actor is null, log to console and return
        if (actor == null)
        {
            Debug.LogError("Actor is null for " + character);
            return;
        }
        //Else if the visualController is null, then log to console and return
        else if(visualController == null)
        {
            Debug.LogError("Unable to change character " + character);
            return;
        }
        //Otherwise change the emotional state in the actor's visual controller to the state given by the vraiable next
        else
        {
            CharacterVisualController.EmotionalState state = (CharacterVisualController.EmotionalState)(int)next;
            visualController.emotionalState = state;
        }

        /*DialogueToCharacterVisuals dialogueToCharacter = GameObject.FindObjectOfType<DialogueToCharacterVisuals>();
        print(dialogueToCharacter.gameObject.name);

        if (!dialogueToCharacter.nameToVisuals.ContainsKey(character))
        {
            Debug.LogError("Unable to chage character " + character + " becuase they are not included on this script");
            return;
        }
        
        CharacterVisualController controller = dialogueToCharacter.nameToVisuals[character];

        print(character);
        CharacterVisualController.EmotionalState state = (CharacterVisualController.EmotionalState)(int)next;
        print(state);
        controller.emotionalState = state;*/
    }
}
