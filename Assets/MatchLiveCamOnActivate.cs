using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLiveCamOnActivate : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        var camParent = GameObject.FindWithTag("Player").transform.GetChild(0);
        transform.parent = camParent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
