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
        if (Input.GetKeyDown(key))
        {
            if (PlayerSingleton.Instance.isLockpicking) return;

            foreach (GameObject obj in PlayerSingleton.Instance.objects)
            {
                if (obj.CompareTag("Item"))
                {
                    PlayerSingleton.Instance.objects.Remove(obj);
                    PlayerSingleton.Instance.uiScript.HideKeyIcon();
                    Destroy(obj);
                    return;
                }

                else if (obj.CompareTag("Door"))
                {
                    //obj.GetComponent<Door>().OpenDoor(); return;
                    obj.GetComponent<Door>().StartLockpicking();
                    PlayerSingleton.Instance.uiScript.HideKeyIcon();
                    return;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {

            PlayerSingleton.Instance.EndLockPiking();
        }
    }
}
