using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSpotting : MonoBehaviour
{
    [SerializeField] float fov = 360f;
    [SerializeField] GameObject player;
    [SerializeField] GameObject eye;
    [SerializeField] GameObject body;
    [SerializeField] float speed;
    private RaycastHit hit;

    public LayerMask playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("????????");
        if (LineOfSight(player))
        {
          //  Debug.Log("Spotted");
           body.transform.localRotation = Quaternion.LookRotation(player.transform.position - transform.position);
          // body.transform.LookAt(player.transform.position);    
            //MoveTowards(player.transform);
        }
    }

    public bool LineOfSight(GameObject player)
    {
        var pos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        var eyePos = new Vector3(eye.transform.forward.x, 0, eye.transform.forward.z);
        var angle = Vector3.Angle(pos, eyePos);
       Debug.Log(angle);
        if (angle <= fov)
        {
            if (Physics.Linecast(eye.transform.position, player.transform.position, out hit, playerLayerMask))
            {
               // Debug.Log(hit.collider.name);
                return hit.collider.transform == player.transform;
            }
        }

        return false;
    }

    public void MoveTowards(Transform target)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.LookAt(target);
    }
}
