using UnityEngine;

public class FogController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public ParticleSystem fogParticleSystemPrefab; // Prefab of the fog particle system
    public Collider playableArea; // The collider that defines the playable area
    public float maxFogDensity = 100.0f; // Maximum emission rate for the fog
    public float fogIncreaseRate = 10.0f; // Rate of fog increase based on distance
    public float minFogLifetime = 0.5f; // Minimum lifetime of fog particles
    public float maxFogLifetime = 5.0f; // Maximum lifetime of fog particles
    public float turnAroundDistance = 5.0f; // Distance from the edge at which the player is turned around
    public int fogEmitterCount = 8; // Number of fog emitters around the edge

    private ParticleSystem[] fogEmitters;
    private Vector3 initialPlayerPosition;

    void Start()
    {
        initialPlayerPosition = player.position;

        // Initialize fog emitters around the edge of the playable area
        fogEmitters = new ParticleSystem[fogEmitterCount];
        for (int i = 0; i < fogEmitterCount; i++)
        {
            fogEmitters[i] = Instantiate(fogParticleSystemPrefab);
            fogEmitters[i].transform.position = GetEdgePosition(i);
            fogEmitters[i].transform.parent = null;
        }
    }

    void Update()
    {
        if (playableArea != null && player != null)
        {
            // Calculate distance from the edge
            float distanceToEdge = Vector3.Distance(player.position, playableArea.ClosestPoint(player.position));

            // Check if the player is within the playable area
            if (playableArea.bounds.Contains(player.position))
            {
                // Decrease the fog density and clear existing particles
                SetFogDensity(0);
                ClearFogEmitters();
            }
            else
            {
                // Increase fog density based on distance
                float fogDensity = Mathf.Clamp((distanceToEdge - turnAroundDistance) * fogIncreaseRate, 0, maxFogDensity);
                SetFogDensity(fogDensity);

                // Adjust fog particle lifetime based on distance to edge
                float fogLifetime = Mathf.Lerp(minFogLifetime, maxFogLifetime, distanceToEdge / (playableArea.bounds.extents.magnitude));
                SetFogLifetime(fogLifetime);

                // Turn the player around if too far
                if (distanceToEdge <= turnAroundDistance)
                {
                    Vector3 directionBack = initialPlayerPosition - player.position;
                    directionBack.y = 0; // Keep the player level
                    player.forward = Vector3.Lerp(player.forward, directionBack.normalized, Time.deltaTime * 2.0f);
                    player.Translate(Vector3.forward * Time.deltaTime * 5.0f); // Push the player back into the playable area
                }
            }
        }
    }

    void SetFogDensity(float density)
    {
        foreach (var emitter in fogEmitters)
        {
            var emission = emitter.emission;
            emission.rateOverTime = density;
        }
    }

    void SetFogLifetime(float lifetime)
    {
        foreach (var emitter in fogEmitters)
        {
            var main = emitter.main;
            main.startLifetime = lifetime;
        }
    }

    void ClearFogEmitters()
    {
        foreach (var emitter in fogEmitters)
        {
            emitter.Clear();
        }
    }

    Vector3 GetEdgePosition(int index)
    {
        Vector3 center = playableArea.bounds.center;
        Vector3 extents = playableArea.bounds.extents;
        float angle = index * Mathf.PI * 2 / fogEmitterCount;
        return new Vector3(center.x + Mathf.Cos(angle) * extents.x, center.y, center.z + Mathf.Sin(angle) * extents.z);
    }
}
