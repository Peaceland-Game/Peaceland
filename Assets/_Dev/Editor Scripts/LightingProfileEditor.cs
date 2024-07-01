#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(LightingProfile))]
public class LightingProfileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LightingProfile profile = (LightingProfile)target;

        if (profile.useKelvin)
        {
            EditorGUI.BeginChangeCheck();
            float newKelvin = EditorGUILayout.Slider("Kelvin", profile.kelvin, 1000f, 20000f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(profile, "Change Kelvin");
                profile.kelvin = newKelvin;
                EditorUtility.SetDirty(profile);

                // Update the gradient preview
                float t = Mathf.InverseLerp(1000f, 20000f, newKelvin);
                Color sunColor = profile.kelvinGradient.Evaluate(t);
                EditorGUILayout.ColorField("Sun Color", sunColor);
            }
        }
    }
}
#endif