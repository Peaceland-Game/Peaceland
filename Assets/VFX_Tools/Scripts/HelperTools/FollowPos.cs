using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPos : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;


    void Update()
    {
        this.transform.position = target.position + offset;
    }
}
