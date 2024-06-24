using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShaderPropertyEdit))]
public class ShaderPropertiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShaderPropertyEdit myScript = (ShaderPropertyEdit)target;
        if (GUILayout.Button("Load Properties Blueprint"))
        {
            myScript.LoadProperties();
        }
        if (GUILayout.Button("Override Properties Ranges"))
        {
            myScript.OverrideRanges();
        }
        if (GUILayout.Button("Generate Random Properties"))
        {
            myScript.GenerateRandomProperties();
        }
        if (GUILayout.Button("Load into Target Materials"))
        {
            myScript.LoadIntoTargetMaterials();  
        }
    }
}
