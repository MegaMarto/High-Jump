using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign the prefab of the object you want to spawn in the inspector
    public Transform spawnLocation; // The location where the object will be spawned
    public float detectionRadius = 5f; // The radius within which the player will trigger the spawn
    public Transform player; // The player's Transform

    private bool hasSpawned = false; // To ensure the object only spawns once

    private void Update()
    {
        // Check if the player is within the detection radius
        if (!hasSpawned && Vector3.Distance(player.position, transform.position) <= detectionRadius)
        {
            SpawnObject();
            hasSpawned = true;
        }
    }

    private void SpawnObject()
    {
        Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
    }

    // Draw the detection radius in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
