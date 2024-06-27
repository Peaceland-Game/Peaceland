using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;

    private CinemachineVirtualCamera curretCam;

    private void Start()
    {
        curretCam = mainCam;
    }

    /// <summary>
    /// Change camera to a desierd cam 
    /// </summary>
    /// <param name="nextCam"></param>
    public void SwapCamera(CinemachineVirtualCamera nextCam)
    {
        if(nextCam == null) return;

        if (curretCam != null)
            curretCam.gameObject.SetActive(false);

        nextCam.gameObject.SetActive(true);
        curretCam = nextCam;
    }

    /// <summary>
    /// Reset current cam back to main camera 
    /// </summary>
    public void SwapToMainCam()
    {
        SwapCamera(mainCam);
    }
}
