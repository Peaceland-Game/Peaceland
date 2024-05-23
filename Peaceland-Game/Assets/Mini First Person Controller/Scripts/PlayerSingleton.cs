using DoorScript;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;

    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private float lockCameraOffsetDist = 1.5f;

    public List<GameObject> objects;

    public bool isLockpicking = false;
    private LockBehaviour activeLock;

    public InterfaceBehaviour uiScript;
    public FirstPersonLook firstPersonLookCamera;

    public float NoiseOutput;
    private Rigidbody rb;

    private Crouch crouchScript;
    public const float MAX_NOISE_OUTPUT = 9f;
    public Vector3 lockCamOffset;

    //-0.018 0.029

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
        crouchScript = GetComponent<Crouch>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        NoiseOutput = crouchScript.IsCrouched ? rb.velocity.magnitude / 4 : rb.velocity.magnitude;
        //Debug.Log(NoiseOutput);
        uiScript.UpdateNoiseBar(NoiseOutput, MAX_NOISE_OUTPUT);
    }

    public void StartLockPicking(Door door)
    {
        if (isLockpicking) return;

        transform.position = new Vector3(
            door.playerPos.position.x,
            transform.position.y,
            door.playerPos.position.z);
       // Debug.Log(transform.position);
       // Debug.Log(door.transform.position);
        var lookDir = new Vector3(door.transform.position.x - transform.position.x, 0, door.transform.position.z - transform.position.z);
        rb.velocity = Vector3.zero;



       // transform.localRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        //firstPersonLookCamera.transform.rotation = Quaternion.identity;

        isLockpicking = true;
        activeLock = door.lockObject;
        activeLock.transform.parent = firstPersonLookCamera.transform;
        activeLock.transform.localPosition = -Camera.main.transform.forward * lockCameraOffsetDist;
        activeLock.transform.localPosition += -lockCamOffset.x * transform.right;
        activeLock.transform.localPosition += lockCamOffset.y * -transform.up;
        uiScript.ShowLockPickRules();
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
        uiScript.HideLockPickRules();
        activeLock.CancelLockpick();
        activeLock = null;
        //  Debug.Log("End Lockpicking");
    }


}
