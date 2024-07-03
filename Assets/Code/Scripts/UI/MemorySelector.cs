using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MemorySelector : MonoBehaviour
{
    public List<MemoryString> memoryStrings = new();
    private Camera mainCamera;
    private Transform cameraTransform;
    private GameObject player;
    public GameObject cameraParent;
    public float cameraMoveTime = 1.0f; // Duration of camera movement in seconds
    public MemoryString activeMemoryString;
    public MemoryTreePhoto currentActivePhoto;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSingleton.Instance.playerInMemorySelection)
        {
            HandlePlayerInput();
        }
    }
    void HandlePlayerInput()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            
            activeMemoryString.DeselectPhoto(currentActivePhoto);
            Debug.Log(cameraTransform, activeMemoryString.transform);
            StartCoroutine(MoveCameraSmooth(cameraParent.transform, activeMemoryString.transform, false));

        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            LoadMemory();
        }

    }
    void LoadMemory()
    {
        SceneManager.LoadScene("TerrainCreation");
    }

    public void ActivateMemoryString(MemoryString memoryString)
    {

        var activeMemory = memoryString.GetActivePhoto();
        if (activeMemory != null)
        {
            PlayerSingleton.Instance.SelectMemoryString();
            memoryString.SelectPhoto(activeMemory);
            currentActivePhoto = activeMemory;
            activeMemoryString = memoryString;
            StartCoroutine(MoveCameraSmooth(activeMemory.cameraAnchor, activeMemory.transform, true));
        }

    }

    private IEnumerator MoveCameraSmooth(Transform targetAnchor, Transform lookAtTarget, bool enableMemorySelect)
    {
        Transform cameraTransform = mainCamera.transform;
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < cameraMoveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraMoveTime;

            // Smoothly interpolate position
            cameraTransform.position = Vector3.Lerp(startPosition, targetAnchor.position, t);

            // Smoothly interpolate rotation
            Vector3 targetDirection = lookAtTarget.position - cameraTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Ensure final position and rotation are exact
        cameraTransform.position = targetAnchor.position;
        cameraTransform.LookAt(lookAtTarget);

        // Set the parent after reaching the final position
        cameraTransform.SetParent(targetAnchor);

        if (!enableMemorySelect)
        {
            PlayerSingleton.Instance.DeselectMemory();
        }
        
    }



}
