using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterVisualController)), CanEditMultipleObjects]
public class CharacterVisualEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterVisualController myScript = (CharacterVisualController)target;
        if(GUILayout.Button("Update All"))
        {
            myScript.UpdateEyes();
            myScript.UpdateMouth();
            myScript.UpdatePattern();
        }
        // TODO: Figure out how to add a folder / dropdown thing for these buttons
        if (GUILayout.Button("Update Eyes"))
        {
            myScript.UpdateEyes();
        } 
        if(GUILayout.Button("Update Mouth"))
        {
            myScript.UpdateMouth();
        }
        if(GUILayout.Button("Update Pattern"))
        {
            myScript.UpdatePattern();
        }
    }
}
