using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;

    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private float lockCameraOffsetDist = 1;

    public List<GameObject> objects;

    public bool isLockpicking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in objects)
        {
            
        }
    }
    public void StartLockPicking(LockBehaviour lockO)
    {
        if (isLockpicking) return;
        isLockpicking = true;
        //var lockPos = transform.position;// + Camera.main.transform.forward * lockCameraOffsetDist;
        //var lockRotation = Quaternion.Euler(-90, 0, 0);
        //Debug.Log(lockPos +"\n"+ lockRotation);
        //var activeLock = Instantiate(lockPrefab, lockPos, lockRotation);
    
        
       // Debug.Log("Start Lockpicking");

    }
    public void EndLockPiking()
    {
        if (!isLockpicking) return;
        isLockpicking = false;
      //  Debug.Log("End Lockpicking");
    }
}
