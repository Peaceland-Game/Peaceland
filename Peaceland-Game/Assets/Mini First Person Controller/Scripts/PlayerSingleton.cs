using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton instance;

    public List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject obj in objects) 
        {

        }
    }
}
