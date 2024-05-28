using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            PlayerSingleton.Instance.objects.Add(other.gameObject);
            PlayerSingleton.Instance.uiScript.ShowKeyIcon();
            PlayerSingleton.Instance.uiScript.PositionKeyIcon(other.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 3) 
        {
            PlayerSingleton.Instance.objects.Remove(other.gameObject);
            PlayerSingleton.Instance.uiScript.HideKeyIcon();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
