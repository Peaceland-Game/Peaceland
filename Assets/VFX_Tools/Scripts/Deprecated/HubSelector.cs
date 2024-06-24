using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class HubSelector : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask memoryLayer;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, memoryLayer))
            {
                SelectableMemory objectHit = hit.transform.GetComponent<SelectableMemory>();

                if (objectHit == null)
                    return;

                objectHit.SelectMemory();
            }
        }
    }

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition );
        //Ray fromScreenRay = new Ray(cam.transform.position, point - cam.transform.position);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
