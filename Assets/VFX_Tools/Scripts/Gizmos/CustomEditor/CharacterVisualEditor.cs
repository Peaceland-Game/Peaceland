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
        if (GUILayout.Button("Update Eyes"))
        {
            myScript.UpdateEyes();
        } 
    }
}
