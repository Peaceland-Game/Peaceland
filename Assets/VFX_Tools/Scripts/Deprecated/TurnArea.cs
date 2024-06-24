using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnArea : MonoBehaviour
{
    [SerializeField] int angle;
    [SerializeField] bool altTurn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print(other.transform.eulerAngles.y);
            

            other.GetComponent<CamRotator>().SetTurn(angle, altTurn);
        }
    }
}
