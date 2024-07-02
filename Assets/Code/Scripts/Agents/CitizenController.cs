using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script to create and initialize a large amount of citizen AIs to populate the city
/// </summary>
public class CitizenController : MonoBehaviour
{

    [Range(20, 1000)]
    public int numberOfCitizens = 100;  //the number of citizens to spawn
    public GameObject citizenPrefab;    //reference to citizen prefab
    public GameObject WaypointParent;   //The parent waypoint object to pass to the citizens
    private Transform[] waypoints;      //Array of waypoint transforms used to spawn the citizens

    [Header("Citizen Properties")]
    //[Range(1f, 1.6f)]
    //public float citizenMinHeight = 1.3f;
    //[Range(1.6f, 2.3f)]
    //public float citizenMaxHeight = 2.0f;

    [Range(1f,2f)]
    public float citizenMinSpeed = 1.5f;    //citizen minimum speed
    [Range(2f, 4f)]
    public float citizenMaxSpeed = 3f;      //citizen maximum speed

    private List<GameObject> citizens = new();  //holds a reference to each created citizen

    private int prevNumCitizens = 100;          //holds the previous number of spawned citizens, used to spawn/remove citizens during runtime if changing inspector slider


    // Start is called before the first frame update
    void Start()
    {
        waypoints = WaypointParent.GetComponentsInChildren<Transform>();
        AddNumberOfCitizens(numberOfCitizens);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (prevNumCitizens != numberOfCitizens) {
            if (prevNumCitizens > numberOfCitizens) {
                RemoveNumberOfCitizens(prevNumCitizens - numberOfCitizens); 
            }
            else {
                AddNumberOfCitizens(numberOfCitizens - prevNumCitizens);
            }
        }
    }
    void AddNumberOfCitizens(int num) {
        for (int i = 0; i < num; i++) {
            Vector3 spawnPosition = GetRandomWaypointLocation();
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                GameObject citizen = Instantiate(citizenPrefab, hit.position, Quaternion.identity);
              //  citizen.transform.localScale = new Vector3(1, Random.Range(citizenMinHeight, citizenMaxHeight), 1);
                citizen.GetComponent<NavMeshAgent>().speed = Random.Range(citizenMinSpeed, citizenMaxSpeed);
                citizen.GetComponent<CitizenAI>().SetWaypoints(WaypointParent, true);
                citizens.Add(citizen);
            }
            else {
                Debug.LogWarning("Failed to place citizen on NavMesh.");
            }
        }
        prevNumCitizens = numberOfCitizens;
    }
    void RemoveNumberOfCitizens(int num) {
        if (num >= citizens.Count) {
            foreach (GameObject citizen in citizens) {
                Destroy(citizen);
            }
            citizens.Clear();
            return;
        }
        for (int i = 0; i < num; i++) {
            GameObject citizenToRemove = citizens[^1];
            citizens.RemoveAt(citizens.Count - 1);
            Destroy(citizenToRemove);
        }
        prevNumCitizens = numberOfCitizens;
    }
    public Vector3 GetRandomWaypointLocation()
    {
        return waypoints[Random.Range(0, waypoints.Length)].position;
    }



}
