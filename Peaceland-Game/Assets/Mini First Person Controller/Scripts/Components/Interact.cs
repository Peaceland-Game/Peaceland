using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorScript;

public class Interact : MonoBehaviour
{
    public KeyCode key = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(key)) 
        {
            foreach (GameObject obj in PlayerSingleton.instance.objects) 
            {
                if (obj.tag == "Item")
                {
                    PlayerSingleton.instance.objects.Remove(obj);
                    Destroy(obj);
                }

                else if (obj.tag == "Door") 
                {
                    obj.GetComponent<Door>().OpenDoor();
                }
            }
        }
    }
}
