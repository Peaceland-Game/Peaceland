using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR)
[CustomEditor(typeof(HeadlineMaker)), CanEditMultipleObjects]
public class HeadlineMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HeadlineMaker myScript = (HeadlineMaker)target;
        if (GUILayout.Button("Generate Topics"))
        {
            myScript.GenerateTopics();
        }
        if (GUILayout.Button("Clear Topics"))
        {
            myScript.ClearTopics();
        }

        if (GUILayout.Button("Generate Notes"))
        {
            myScript.GenerateNotes();
        }
        if (GUILayout.Button("Clear Notes"))
        {
            myScript.ClearNotes();
        }
    }
}
#endif