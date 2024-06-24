using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StringRendering: MonoBehaviour
{
    [SerializeField] Vector3 startStringOffset;
    [SerializeField] Vector3 endStringOffset;
    [SerializeField] LineRenderer stringRenderer;


    private void Update()
    {
        DangleAnim();
    }

    private void DangleAnim()
    {
        stringRenderer.SetPosition(0, this.transform.position + startStringOffset);
        stringRenderer.SetPosition(1, this.transform.position + endStringOffset);
    }
}
