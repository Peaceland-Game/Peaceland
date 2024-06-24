using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{

    [SerializeField] float turnTime;
    [SerializeField] AnimationCurve turnCurve;
    [SerializeField] float hopVert;
    [SerializeField] float hopRiseTime;
    [SerializeField] AnimationCurve riseCurve;
    [SerializeField] float hopFallTime;
    [SerializeField] AnimationCurve fallCurve;

    private bool isTurning;

    private void Start()
    {
        isTurning = false;  
    }

    // Update is called once per frame
    void Update()
    {
        // Testing controls 
        /*if(Input.GetKeyDown(KeyCode.E))
        {
            StartTurn(-90.0f);
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            StartTurn(90.0f);
        }*/
    }

    public void SetTurn(float yRot, bool altTurn = false)
    {
        if (isTurning)
            return;

        if (((int)this.transform.eulerAngles.y == (360 - yRot)) || ((int)this.transform.eulerAngles.y == yRot)) // TODO: change to be not bad :3
            return;

        isTurning = true;
        StartCoroutine(HopCo());
        StartCoroutine(RotateCo(yRot, altTurn));
    }

    public void StartTurn(float yRot, bool altTurn = false)
    {
        float rot = this.transform.eulerAngles.y + yRot;

        if (isTurning)
            return;

        isTurning = true;
        StartCoroutine(HopCo());
        StartCoroutine(RotateCo(rot, altTurn));
    }

    private IEnumerator HopCo()
    {
        float timer = 0.0f;
        float startY = this.transform.position.y;

        // Rising 

        while(timer <= hopRiseTime)
        {
            float y = Mathf.Lerp(startY, hopVert, riseCurve.Evaluate(timer / hopRiseTime));
            Vector3 target = this.transform.position;
            target.y = y;

            this.transform.position = target;

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;

        // Falling 

        while (timer <= hopFallTime)
        {
            float y = Mathf.Lerp(hopVert, startY, fallCurve.Evaluate(timer / hopFallTime));
            Vector3 target = this.transform.position;
            target.y = y;

            this.transform.position = target;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateCo(float yRot, bool alt = false)
    {
        float timer = 0.0f;
        float startY = this.transform.eulerAngles.y;

        while(timer <= 1.0f)
        {
            float lerp = timer / turnTime;
            float y;

            if(alt)
            {
                y = Mathf.LerpAngle(yRot, startY, turnCurve.Evaluate(lerp));
            }
            else
            {
                y = Mathf.LerpAngle(startY, yRot, turnCurve.Evaluate(lerp));
            }

            this.transform.eulerAngles = new Vector3(0.0f, y, 0.0f);

            timer += Time.deltaTime; 
            yield return null;
        }

        isTurning = false;
    }
}
