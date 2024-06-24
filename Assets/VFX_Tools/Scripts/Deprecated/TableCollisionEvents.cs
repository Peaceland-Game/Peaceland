using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCollisionEvents : MonoBehaviour
{
    //the number for the table layer
    private int playerLayer = 1 << 0;

    public GameObject player;

    private float force = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckBox(transform.position, transform.localScale, new Quaternion(), playerLayer, new QueryTriggerInteraction()))
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == player.GetComponent<Rigidbody>())
        {
            Debug.Log(gameObject.transform.position - player.transform.position);
            gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - player.transform.position) * force);
        }
    }
}
