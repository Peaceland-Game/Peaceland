using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] Camera mainCam;
    [SerializeField] FirstPersonController firstPersonController;

    [Header("UI Elements")]
    [SerializeField] Slider FOVSlider;
    [SerializeField] TextMeshProUGUI FOVTextMesh;
    [SerializeField] Slider mouseSpeedSlider;
    [SerializeField] TextMeshProUGUI mouseSpeedTextMesh;

    [Header("Values")]
    [Range(30.0f, 160.0f)]
    [SerializeField] float FOV = 60.0f;
    [Range(0.1f, 10.0f)]
    [SerializeField] float mouseSpeed = 4.0f;



    void Update()
    {
        // Store internally 
        FOV = FOVSlider.value;
        mouseSpeed = mouseSpeedSlider.value;

        // Send to ingame objects 
        mainCam.fieldOfView = FOV;
        firstPersonController.mouseSensitivity = mouseSpeed;

        // Write to text meshes 
        FOVTextMesh.text = ((int)FOV).ToString();
        mouseSpeedTextMesh.text = ((int)mouseSpeed).ToString();
    }
}
