using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DebugCam : MonoBehaviour
{
    [SerializeField] float moveSpeedSensitivity; 
    [SerializeField] FreeCamera cam;


    // Update is called once per frame
    void Update()
    {
        print(Input.mouseScrollDelta.y);
        cam.m_MoveSpeed = Mathf.Max(cam.m_MoveSpeed + Input.mouseScrollDelta.y * moveSpeedSensitivity, 0.001f);
    }
}
