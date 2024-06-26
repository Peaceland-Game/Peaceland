using UnityEngine;
using System.Collections.Generic;

public class FogController : MonoBehaviour
{
    public Transform player;
    public GameObject fogEmitterPrefab;
    public float fogStartDistance = 0f;
    public float maxFogDistance = 20f;
    public float fogDensityIncreaseRate = 2.5f;
    public float maxFogDensity = 50f;
    public float teleportDistance = 10f;
    public float emitterSpawnDistance = 3f;
    public int emitterCount = 8;
    public float fadeDuration = 1f; // Duration of the fade effect


    private Collider playableArea;
    private List<ParticleSystem> fogEmitters = new List<ParticleSystem>();
    private Quaternion lastValidRotation;
    private Vector3 lastValidPosition;
    private bool isOutsidePlayArea = false;
    private bool teleportedPlayer = false;
    

    void Start()
    {
        
        playableArea = GetComponent<SphereCollider>();
        

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

    void Update()
    {
        if (!playableArea.bounds.Contains(player.position))
        {
            if (!isOutsidePlayArea)
            {
                // Player just exited the play area
                isOutsidePlayArea = true;
                lastValidRotation = player.rotation;
                lastValidPosition = playableArea.ClosestPoint(player.position);
            }

            float distanceOutside = Vector3.Distance(playableArea.ClosestPoint(player.position), player.position);

            if (distanceOutside > teleportDistance && !teleportedPlayer)
            {
                TeleportPlayerBack();
            }
            else if (distanceOutside > fogStartDistance)
            {
                UpdateFogEmitters();
                UpdateFogDensity(distanceOutside);
            }
            else
            {
                DisableFogEmitters();
            }
        }
        else
        {
            isOutsidePlayArea = false;
            lastValidPosition = player.position;
            DisableFogEmitters();
        }

    }

    void UpdateFogEmitters()
    {
        
        Vector3 directionFromCenter = (player.position - playableArea.bounds.center).normalized;
        Vector3 spawnPosition = player.position + directionFromCenter * emitterSpawnDistance;

        for (int i = 0; i < fogEmitters.Count; i++)
        {
            fogEmitters[i].gameObject.SetActive(true);
            float angle = (360f / emitterCount) * i;
            Vector3 rotatedPosition = Quaternion.Euler(0, angle, 0) * (spawnPosition - player.position) + player.position;
            fogEmitters[i].transform.position = rotatedPosition;
        }
    }

    void UpdateFogDensity(float distanceOutside)
    {
        float fogPercentage = Mathf.Clamp01((distanceOutside - fogStartDistance) / (maxFogDistance - fogStartDistance));
        float targetDensity = fogPercentage * maxFogDensity;

        foreach (ParticleSystem emitter in fogEmitters)
        {
            var main = emitter.main;
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b,
                Mathf.MoveTowards(main.startColor.color.a, targetDensity, fogDensityIncreaseRate * Time.deltaTime));
        }
    }

    void DisableFogEmitters()
    {
        foreach (ParticleSystem emitter in fogEmitters)
        {
            emitter.gameObject.SetActive(false);
        }
    }

    void TeleportPlayerBack()
    {
        teleportedPlayer = true;
        var playerMovementScript = player.GetComponent<FirstPersonController>();
        playerMovementScript.enabled = false;
        // Calculate the reversed direction
        Vector3 forwardDirection = lastValidRotation * Vector3.forward;
        forwardDirection.y = 0; // Keep only the horizontal component
        Vector3 reversedDirection = -forwardDirection.normalized;

        // Create a rotation facing the reversed direction
        Quaternion newRotation = Quaternion.LookRotation(reversedDirection);

        // Use the ScreenFader to fade, teleport, and then fade back
        ScreenFader.instance.FadeAndTeleport(fadeDuration, () => {
            // This code runs when the screen is fully faded (black)
            player.position = lastValidPosition;
            player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, newRotation.eulerAngles.y, 0);
            playerMovementScript.enabled = true;
            teleportedPlayer = false;
        });
    }
}