using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlowCloud : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] Color color;
    [SerializeField] Vector2 intensityRange;
    [SerializeField] Vector2 disRange;
    [SerializeField] AnimationCurve intensityRamp;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float disSqr = Vector3.SqrMagnitude(this.transform.position - cam.transform.position);
        float lerp = Mathf.InverseLerp(disRange.x, disRange.y, disSqr);

        renderer.material.SetColor("_Color", color * Mathf.Lerp(intensityRange.x, intensityRange.y, lerp));
    }
}
