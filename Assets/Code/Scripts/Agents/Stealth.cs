using Language.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Stealth : MonoBehaviour
{
    //the player gameObject that the agent wants to seek
    public GameObject player;
    //the number of rays being cast
    public int numberOfRays;
    //the field of view that the rays cover
    public float fieldOfViewInDegrees;
    
    //the maximum distance the agaent can see the player from
    public float detectionDistance;
    
    //when true, the agent detects the player when they get too close
    public bool detectsPlayerWhenClose;
    public float closeDetectDistance;

    //agent uses a larger detecttion distance when they can hear the player
    public float largerDetectionDistance;
    //how long the agent stays alerted when they hear the player
    public float secondsAgentStaysAlerted;
    private float currentAlertTime;

    //how long the agent will persue the player after they lose sight of them
    public float secondsAgentSeeks;

    //wether or not the player was detected
    [HideInInspector] public bool detectedPlayer;
    //wether or not the agent can hear the player
    [HideInInspector] public bool heardPlayer;

    //TEMPORARY indicators and materials
    public GameObject detectionIndicator;
    public Material detectMaterial;
    public GameObject alertIndicator;
    public Material alertMaterial;

    //contains every ray
    private Ray[] rays;
    //contains every ray angle
    private float[] rayAngles;

    //used for collisions
    private RaycastHit hit;
    
    private void Start()
    {
        rays = new Ray[numberOfRays];
        rayAngles = new float[numberOfRays];
        currentAlertTime = secondsAgentStaysAlerted;

        //find the distance between each ray in radians
        float distanceBetweenRays = fieldOfViewInDegrees / numberOfRays * (Mathf.PI / 180);
        float currentAngle = 0;

        for (int i = 0; i < numberOfRays; i++)
        {
            //create a new ray
            rays[i] = new Ray(transform.position, transform.forward);

            //set forward ray angle to 0
            if (i == 0) { rayAngles[0] = 0; }

            //set a positive angle if the ray being created is positive and vice versa
            else if (currentAngle >= 0)
            {
                currentAngle += distanceBetweenRays;
                rayAngles[i] = currentAngle;
                currentAngle *= -1;
            }
            else
            {
                rayAngles[i] = currentAngle;
                currentAngle *= -1;
            }
        }
    }

    private void Update()
    {
        //reset states
        detectedPlayer = false;
        heardPlayer = false;
        alertMaterial.color = Color.green;

        //the angle made by the forward and the axis
        float angleOfForward = CalculateRayAngle(transform.forward.z, transform.forward.x);
        
        //TEMPORARY set the indicator above the agent's head
        detectionIndicator.transform.position = transform.position + new Vector3(0, 2, 0);
        alertIndicator.transform.position = transform.position + new Vector3(0, 3, 0);


        //check if the agent can hear the player
        if (GetDistance(transform.position, player.transform.position) <= player.GetComponent<PlayerSound>().getCurrentSoundFootprint())
        {
            currentAlertTime = 0;
        }
        //agent is alerted until time runs out
        if (currentAlertTime < secondsAgentStaysAlerted)
        {
            alertMaterial.color = Color.red;
            currentAlertTime += Time.deltaTime;
            heardPlayer = true;
        }


        //for each ray
        for (int i = 0; i < numberOfRays; i++)
        {
            //set the ray's new origin and direction
            rays[i].origin = transform.position;
            rays[i].direction = new Vector3(Mathf.Cos(angleOfForward + rayAngles[i]), transform.forward.y, Mathf.Sin(angleOfForward + rayAngles[i])).normalized;
            if (i == 0)
            {
                rays[i].direction = transform.forward;
            }

            //detect for any collisions, no more collisions are needed once the player is detected
            if(!detectedPlayer) 
            {
                //use smaller detection range if they do not hear the player
                if (!heardPlayer)
                {
                    if (IsCollidingWithObject(rays[i], player, detectionDistance) == "detected" || IsCollidingWithObject(rays[i], player, detectionDistance) == "out of range")
                    {
                        detectedPlayer = true;
                    }
                }
                //use large if they hear the player
                else
                {
                    if (IsCollidingWithObject(rays[i], player, largerDetectionDistance) == "detected" || IsCollidingWithObject(rays[i], player, largerDetectionDistance) == "out of range")
                    {
                        detectedPlayer = true;
                    }
                }
            }
        }

        //player is detected when they get too close to the agent
        if (GetDistance(transform.position, player.transform.position) <= closeDetectDistance)
        {
            //Debug.Log("too close! agent detected player");
            detectMaterial.color = Color.blue;
        }
    }

    /// <summary>
    /// calculates the angle of the forward and the x/z axis in radians
    /// </summary>
    /// <param name="forwardZ"></param>
    /// <param name="forwardX"></param>
    /// <returns>a float with the angle</returns>
    private float CalculateRayAngle(float forwardZ, float forwardX)
    {
        float angle = 0;
        if (forwardX <= 0 && forwardZ > 0)
        {
            angle = MathF.Atan(transform.forward.z / transform.forward.x) + MathF.PI;
        }
        else if (forwardX <= 0 && forwardZ <= 0)
        {
            angle = MathF.Atan(transform.forward.z / transform.forward.x) + MathF.PI;
        }
        else if (forwardX > 0 && forwardZ <= 0)
        {
            angle = MathF.Atan(transform.forward.z / transform.forward.x) + 2 * MathF.PI;
        }
        else if (forwardX > 0 && forwardZ > 0)
        {
            angle = MathF.Atan(transform.forward.z / transform.forward.x) + 2 * MathF.PI;
        }
        return angle;
    }

    /// <summary>
    /// Checks if a ray collides with a valid object
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="player"></param>
    /// <param name="maxDistance"></param>
    /// <returns>a string shpwing what object the ray collided with</returns>
    private string IsCollidingWithObject(Ray ray, GameObject player, float maxDistance)
    {
        if (Physics.Raycast(ray, out hit))
        {
            //the agent sees a wall
            if (hit.collider.gameObject.layer == 0)
            {
                //Debug.Log("wall");
                detectMaterial.color = Color.grey;
                return "wall";
            }

            //rays collided with the player
            if (hit.collider.gameObject.layer == 6)
            {
                //they were in range, agent sees player
                if (GetDistance(transform.position, player.transform.position) <= maxDistance)
                {
                    //Debug.Log("player was seen");
                    detectMaterial.color = Color.red;
                    return "detected";
                }
                //they were out of range, agent does not see player
                else if (GetDistance(transform.position, player.transform.position) > maxDistance)
                {
                    //Debug.Log("ray collided with player, but player is out of range");
                    detectMaterial.color = Color.yellow;
                    return "out of range";
                }
            }
        }
        else
        {
            detectMaterial.color = Color.gray;
        }
        return "undetected";
    }

    /// <summary>
    /// calculates the distance between 2 objects
    /// </summary>
    /// <param name="object1Pos"></param>
    /// <param name="object2Pos"></param>
    /// <returns></returns>
    private float GetDistance(Vector3 object1Pos, Vector3 object2Pos)
    {
        return Mathf.Sqrt(Mathf.Pow(object2Pos.x - object1Pos.x, 2) + Mathf.Pow(object2Pos.z - object1Pos.z, 2));
    }

    private void OnDrawGizmos()
    {
        //draw each ray
        for (int i = 0; i < numberOfRays; i++)
        {
            //make the visual refect their current range
            if (!heardPlayer)
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * detectionDistance, Color.cyan);
            }
            else
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * largerDetectionDistance, Color.cyan);
            }
        }
    }
}
