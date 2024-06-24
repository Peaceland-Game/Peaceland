using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    [SerializeField] Renderer obj;
    [SerializeField] AnimationCurve curve;
    [SerializeField] bool startState;
    private bool isVisible;
    private float alpha = 1.0f;

    private Vector3 tint;

    // Start is called before the first frame update
    void Start()
    {
        isVisible = startState;

        tint = (Vector4)obj.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(isVisible)
        {
            alpha = Mathf.Clamp01(alpha + Time.deltaTime);
        }
        else
        {
            alpha = Mathf.Clamp01(alpha - Time.deltaTime);
        }

        obj.material.color = new Vector4(tint.x, tint.y, tint.z, curve.Evaluate(alpha));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isVisible = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isVisible = true;
        }
    }
}
