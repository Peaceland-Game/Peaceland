using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurnGetter : MonoBehaviour
{
    public Transform ObjectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        if (!ObjectToLookAt) Debug.LogError($"Missing Object for head turner on {name}");
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

}
