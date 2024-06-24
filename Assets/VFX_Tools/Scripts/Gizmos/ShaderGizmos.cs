using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderGizmos : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] List<ShaderGizmosSettings> shaderGizmos;

    [Serializable]
    private class ShaderGizmosSettings
    {
        [SerializeField] public string varName;
        [SerializeField] public GizmosType gizmoType;
        [SerializeField] public Space space;

        public enum GizmosType
        {
            SPHERE
        }

        public enum Space
        {
            WORLD,
            OBJECT
        }
    }
}
