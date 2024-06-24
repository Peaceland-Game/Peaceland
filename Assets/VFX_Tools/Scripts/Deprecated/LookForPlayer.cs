using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class LookForPlayer : MonoBehaviour
{
    // Establishing the area that the detector sees
    public Camera viewCamera;
    Plane[] planes;

    public Light viewLight;

    // Establishing the dimensions of the player
    public GameObject player;
    Collider playerCollider;

    // Variables for determining when the detector can see the player
    bool lookingUp;
    bool lookEnabled;
    double lookTimer;
    public double lookThreshold;

    bool playerSeen; // test
    public UnityEvent Detected;

    // Start is called before the first frame update
    void Start()
    {
        // Calculating the bounds of the camera view
        planes = GeometryUtility.CalculateFrustumPlanes(viewCamera);

        // Getting the dimensions of the player collider
        playerCollider = player.GetComponent<Collider>();

        // Starting state for whether the detector can see
        lookingUp = false;
        lookEnabled = true;
        GetComponent<Renderer>().material.color = Color.green;
        viewLight.enabled = false;
        lookTimer = 0;

        playerSeen = false;

        if(Detected == null)
        {
            Detected = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the look timer
        if (lookEnabled)
        {
            lookTimer += Time.deltaTime;
        }

        // If the look timer exceeds the look threshold,
        // change whether the detector is looking, and reset the look timer
        if(lookTimer >= lookThreshold)
        {
            lookTimer = 0;
            lookingUp = !lookingUp;

            if(lookingUp)
            {
                Debug.Log(name + " is looking up");
                GetComponent<Renderer>().material.color = Color.red;
                viewLight.enabled = true;
            }
            else
            {
                Debug.Log(name + " is looking down");
                GetComponent<Renderer>().material.color = Color.green;
                viewLight.enabled = false;
            }
        }

        // Check if the player is seen by the detector
        if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds) && lookingUp && lookEnabled)
        {
            playerSeen = true;
            Debug.Log("Player seen by " + name);

            lookEnabled = false; // Stop checking for the player after they are initially detected
            Detected.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            playerSeen = false;
        }
    }

    public void reEnableLooking()
    {
        lookEnabled = true;
    }
}
