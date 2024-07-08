using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Reflection;


public class DialogueSkipper : MonoBehaviour
{
    public bool MotherPermission = false;
    public bool FatherPermission = false;

    

    private FieldInfo[] fields;

#if (UNITY_EDITOR)
    public void Skip()
    {
       
        fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            // Get the value of the field
            object value = field.GetValue(this);

            // Set the Lua variable with the same name as the field
            DialogueLua.SetVariable(field.Name, value);
            Debug.Log($"Setting lua variable {field.Name} to {value}");
        }
    }
#endif
}
