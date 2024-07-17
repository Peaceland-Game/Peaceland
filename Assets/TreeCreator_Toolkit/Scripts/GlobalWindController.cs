using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalWindController : MonoBehaviour
{
    [Range(0, 5)]
    public float speed = 2.41f;

    [Range(0, 0.05f)]
    public float worldFrequency = 0.03f;
    [Range(0, 0.1f)]
    public float bendAmount = 0.047f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("WindSpeed", speed);
        Shader.SetGlobalFloat("WorldFrequency", worldFrequency);
        Shader.SetGlobalFloat("BendAmount", bendAmount);
    }
}
