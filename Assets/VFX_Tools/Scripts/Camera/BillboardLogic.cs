using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardLogic : MonoBehaviour
{
    [SerializeField] bool lockToYAxis;

    private Transform cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        if (cam == null)
            Debug.LogError("No camera tagged as MainCamera in scene");
    }

    void Update()
    {
        if (lockToYAxis)
        {
            this.transform.LookAt(cam.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }
        else
        {
            this.transform.LookAt(cam.position);
        }
    }
}
