using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocusInteractable : Interactable
{
    [Tooltip("Unfocuses from this interactable if player goes out of range")]
    [SerializeField] float range;
    [SerializeField] CinemachineVirtualCamera cam;

    private CameraManager manager;
    private Transform source;
    private bool focused;

    public override void Interact(Transform source)
    {
        print("test");
        if (manager == null)
            return;

        manager.SwapCamera(cam);
        focused = true;

        this.source = source;

        base.Interact(source);
    }

    public void ResetThisFocus()
    {
        manager.SwapToMainCam();
    }

    private void Awake()
    {
        manager = GameObject.FindObjectOfType<CameraManager>();
        if (manager == null)
            Debug.LogError("Camera manager not in scene");

        if (cam == null)
            Debug.LogError("Cam not attached to this script");

        focused = false;
    }

    private void Update()
    {
        if(focused)
        {
            if (Vector3.Distance(this.transform.position, source.position) >= range)
            {
                manager.SwapToMainCam();
                focused = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
