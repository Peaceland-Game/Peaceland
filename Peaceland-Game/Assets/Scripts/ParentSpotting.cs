using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSpotting : MonoBehaviour
{
    [SerializeField] float fov = 60.0f;
    [SerializeField] GameObject player;
    [SerializeField] GameObject eye;
    [SerializeField] float speed;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LineOfSight(player))
        {
            Debug.Log("Spotted");
            //MoveTowards(player.transform);
        }
    }

    public bool LineOfSight(GameObject player)
    {
        if(Vector3.Angle(player.transform.position - eye.transform.position, eye.transform.forward) <= fov &&
            Physics.Linecast(eye.transform.position, player.transform.position, out hit) && 
            hit.collider.transform == player.transform)
        {
            return true;
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
