using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MushroomTool)), CanEditMultipleObjects]
public class MushroomToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MushroomTool myScript = (MushroomTool)target;
        if (GUILayout.Button("Redistribute Mushrooms"))
        {
            myScript.RedistributeMushrooms();
        }

        if (GUILayout.Button("Randomize Attributes"))
        {
            myScript.RandomizeAttributes();
        }

        if (GUILayout.Button("Spawn Random Mushroom Type"))
        {
            myScript.SpawnRandomMushroomType();
        }

        if (GUILayout.Button("Clear"))
        {
            myScript.Clear();
        }
    }
}
