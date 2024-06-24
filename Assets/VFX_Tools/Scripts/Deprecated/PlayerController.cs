using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TerrainUtils;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{

    [Header("Controls")]
    [SerializeField] float speed;
    [SerializeField] float checkRadius;
    [SerializeField] float checkDis;
    [SerializeField] LayerMask terrainMask;

    [Header("Animation")]
    [SerializeField] Transform visualMesh;
    [SerializeField] float bobSpeed;
    [SerializeField] Vector3 bobScaleMin;
    [SerializeField] Vector3 bobScaleMax;
    [SerializeField] float bobInecreaseRate;
    [SerializeField] float bobDecreaseRate;
    [SerializeField] AnimationCurve bobXCurve;
    [SerializeField] AnimationCurve bobYCurve;

    private float bobLerp;
    private float interpolateIdleToBob;
    private Vector3 startVisualScale;
    private float startVisualLocalYPos;

    void Start()
    {
        startVisualScale = visualMesh.localScale;
        startVisualLocalYPos = visualMesh.localPosition.y;

        interpolateIdleToBob = 0.0f;
        bobLerp = 0.5f;
    }

    void Update()
    {
        Movement();
        Visual();
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = this.transform.right * horizontal + this.transform.forward * vertical; // (new Vector3(horizontal, 0, vertical)).normalized;
        if(direction.sqrMagnitude <= 0.01f) // Lessen bob influence 
        {
            interpolateIdleToBob = Mathf.Clamp01(interpolateIdleToBob - bobDecreaseRate * Time.deltaTime);
        }

        if (Physics.CheckSphere(this.transform.position + direction * checkDis, checkRadius, terrainMask))
        {
            return;
        }

        this.transform.position += direction * speed * Time.deltaTime;
        interpolateIdleToBob = Mathf.Clamp01(interpolateIdleToBob + bobInecreaseRate * Time.deltaTime);

       /* if (direction == Vector3.zero)
        {
            vertical = -1;
        }
        if (manager.TimeLeft <= (0.25f * startTime))
        {
            SetSprite(vertical, horizontal, front75, back75, right75, left75);
        }
        else if (manager.TimeLeft <= (0.50f * startTime))
        {
            SetSprite(vertical, horizontal, front50, back50, right50, left50);
        }
        else if (manager.TimeLeft <= (0.75f * startTime))
        {
            SetSprite(vertical, horizontal, front25, back25, right25, left25);
        }
        else
        {
            SetSprite(vertical, horizontal, front0, back0, right0, left0);
        }*/
    }

    private void Visual()
    {
        bobLerp = Mathf.InverseLerp(-1.0f, 1.0f, Mathf.Sin(Time.time * bobSpeed));
        Vector3 bobScale = new Vector3(
            Mathf.Lerp(bobScaleMin.x, bobScaleMax.x, bobXCurve.Evaluate(bobLerp)),
            Mathf.Lerp(bobScaleMin.y, bobScaleMax.y, bobYCurve.Evaluate(bobLerp)),
            1.0f);


        Vector3 finalScale = Vector3.Lerp(startVisualScale, bobScale, interpolateIdleToBob);
        visualMesh.transform.localScale = finalScale;
        visualMesh.transform.localPosition = new Vector3(
            visualMesh.localPosition.x, 
            startVisualLocalYPos + (finalScale - startVisualScale).y * 0.5f, 
            visualMesh.localPosition.z);
    }

    private void OnDrawGizmosSelected()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = (new Vector3(horizontal, 0, vertical)).normalized;
        Gizmos.DrawWireSphere((this.transform.position + direction * checkDis), checkRadius);
    }
}
