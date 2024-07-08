using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FogController : MonoBehaviour
{
    //reference to the scene player
    public Transform player;
    //reference to the prefab used to create the particle Emitters
    public GameObject fogEmitterPrefab;
    //how far away from the playable area the player must be before showing fog, 0 would be right at the edge of the collider
    public float fogStartDistance = 0f;
    //The farthest distance to show fog
    public float maxFogDistance = 20f;
    //How fast the fog density increases when leaving the playable area
    public float fogDensityIncreaseRate = 2.5f;
    //the maximum density of the fog
    public float maxFogDensity = 50f;
    //how far away from the playable area the player must reach before being teleported back, this value needs to be higher than fogStartDistance and lower than maxFogDistance
    public float teleportDistance = 10f;
    //How far away from the player to place the fog emitters
    public float emitterSpawnDistance = 3f;
    //how many fog emitters to spawn at one time
    public int emitterCount = 8;
    // Duration of the fade effect
    public float fadeDuration = 1f; 

    //List of colliders defining the playable area
    private List<Collider> playableArea = new();
    //list of particleSystem components of fog emitter objects
    private List<ParticleSystem> fogEmitters = new List<ParticleSystem>();
    //store the last valid rotation and position the player had when leaving the playable area to return them to that state if required
    private Quaternion lastValidRotation;
    private Vector3 lastValidPosition;
    //flag to check if player just stepped out of the playable zone
    private bool isOutsidePlayArea = false;
    //flag to check if the player was teleported already
    private bool teleportedPlayer = false;
    //holds a reference to the last container the player was in before they left the playable zone
    private Collider lastContainer = null;

    
    /// <summary>
    /// Initialize fog emitters around the player
    /// </summary>
    void Start()
    {

        playableArea = GetComponentsInChildren<Collider>().ToList();
        //Debug.Log(playableArea.Count);

        for (int i = 0; i < emitterCount; i++)
        {
            GameObject emitter = Instantiate(fogEmitterPrefab, Vector3.zero, Quaternion.identity);
            fogEmitters.Add(emitter.GetComponent<ParticleSystem>());
            emitter.SetActive(false);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastValidPosition, 1f);
    }
    /// <summary>
    /// Checks the player position against the playable area (defined by the playableArea collider list)
    /// </summary>
    /// <returns>true if the player is inside one of the bounding objects, false otherwise</returns>
    bool CheckPlayerBounds()
    {
        foreach (var coll in playableArea)
        {
            if (coll && coll.bounds.Contains(player.position))
            {
                lastContainer = coll;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Handles checking if the player is inside of the playable area and calls the appropriate functions depending on the player's state
    /// </summary>
    void HandlePlayerBounds()
    {
        //check if player is in bounds
        bool playerInBounds = CheckPlayerBounds();
        
        //if the player is inbounds, update valid areas, disable fog emitters
        if (playerInBounds)
        {
            isOutsidePlayArea = false;
            lastValidPosition = player.position;
            DisableFogEmitters();
        }
        else
        {   //player just stepped outside the playable area
            if (!isOutsidePlayArea)
            {
                //store last pos and rot
                isOutsidePlayArea = true;
                lastValidRotation = player.rotation;
                lastValidPosition = GetClosestPointOnColliders(player.position);
            }

            //how far the player is outside 
            float distanceOutside = Vector3.Distance(GetClosestPointOnColliders(player.position), player.position);

            //if needed, teleport player back inside
            if (distanceOutside > teleportDistance && !teleportedPlayer)
            {
                TeleportPlayerBack();
            }
            //start the fog emitters if they are outside of the play zone but not yet far enough to be teleported
            else if (distanceOutside > fogStartDistance)
            {
                UpdateFogEmitters();
                UpdateFogDensity(distanceOutside);
            }
            //player is very far outside of the playable zone, something is wrong
            else
            {
                DisableFogEmitters();
            }
        }
    }
    void Update()
    {
        HandlePlayerBounds();
    }
    /// <summary>
    /// Get the closest point to the player on the colliders in the playable area
    /// </summary>
    /// <param name="position">The position to test against, assume the player's position</param>
    /// <returns>A vector3 that is the closest point, on the closest collider to the passed in argument Vector3</returns>
    Vector3 GetClosestPointOnColliders(Vector3 position)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (var coll in playableArea)
        {
            Vector3 point = coll.ClosestPoint(position);
            float distance = Vector3.Distance(position, point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    /// <summary>
    /// Handles updating the fog emitters positions
    /// </summary>
    void UpdateFogEmitters()
    {
        if (!lastContainer) return;

        //get the direction from player to the last collider's center
        Vector3 directionFromCenter = (player.position - lastContainer.bounds.center).normalized;
        Vector3 spawnPosition = player.position + directionFromCenter * emitterSpawnDistance;

        //move the fog emitters to be around the player and set active
        for (int i = 0; i < fogEmitters.Count; i++)
        {
            fogEmitters[i].gameObject.SetActive(true);
            float angle = (360f / emitterCount) * i;
            Vector3 rotatedPosition = Quaternion.Euler(0, angle, 0) * (spawnPosition - player.position) + player.position;
            fogEmitters[i].transform.position = rotatedPosition;
        }
    }

    /// <summary>
    /// Update how dense the fog should be depending on the distance parameter
    /// </summary>
    /// <param name="distanceOutside">a float distance how far the player is outside of the playable area</param>
    void UpdateFogDensity(float distanceOutside)
    {
        //calculate a percentage density based on distance
        float fogPercentage = Mathf.Clamp01((distanceOutside - fogStartDistance) / (maxFogDistance - fogStartDistance));
        float targetDensity = fogPercentage * maxFogDensity;

        //smoothly update the alpha of the particles to increase density
        foreach (ParticleSystem emitter in fogEmitters)
        {
            var main = emitter.main;
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b,
                Mathf.MoveTowards(main.startColor.color.a, targetDensity, fogDensityIncreaseRate * Time.deltaTime));
        }
    }

    /// <summary>
    /// Disable all fog emitters in the scene
    /// </summary>
    void DisableFogEmitters()
    {
        fogEmitters.ForEach(em => em.gameObject.SetActive(false));
    }

    /// <summary>
    /// teleports the player back to the last valid position
    /// </summary>
    void TeleportPlayerBack()
    {
        //set flag
        teleportedPlayer = true;

        //disable player controls
        var playerMovementScript = player.GetComponent<FirstPersonController>();
        playerMovementScript.enabled = false;

        //calculate new position and rotation
        Vector3 forwardDirection = lastValidRotation * Vector3.forward;
        forwardDirection.y = 0;
        Vector3 reversedDirection = -forwardDirection.normalized;
        Quaternion newRotation = Quaternion.LookRotation(reversedDirection);

        //call Screen Fade script to show gray screen fade
        //second argument is an Action that updates player position, rotation, enables player controls, and clears teleport flag
        ScreenFader.instance.FadeAndTeleport(fadeDuration, () => {
            
            player.position = GetClosestPointOnColliders(player.position);
            player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, newRotation.eulerAngles.y, 0);
            playerMovementScript.enabled = true;
            teleportedPlayer = false;
        });
    }
}