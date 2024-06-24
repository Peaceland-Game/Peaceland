using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopEuler : MonoBehaviour
{
    [SerializeField] Vector3 offsetA;
    [SerializeField] Vector3 offsetB;
    [SerializeField] AnimationCurve rotCurve;
    [SerializeField] float rotSpeed;

    private Vector3 rotA;
    private Vector3 rotB;
    private bool aToB = true;
    private float rotLerp;

    // Start is called before the first frame update
    void Start()
    {
        rotA = this.transform.localEulerAngles + offsetA;
        rotB = this.transform.localEulerAngles + offsetB;

        rotLerp = Random.Range(0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (aToB)
        {
            this.transform.localEulerAngles = Vector3.Lerp(rotA, rotB, rotCurve.Evaluate(rotLerp));
        }
        else
        {
            this.transform.localEulerAngles = Vector3.Lerp(rotB, rotA, rotCurve.Evaluate(rotLerp));
        }

        rotLerp += Time.deltaTime * rotSpeed;

        if (rotLerp >= 1.0)
        {
            rotLerp = 0.0f;
            aToB = !aToB;
        }
    }
}
