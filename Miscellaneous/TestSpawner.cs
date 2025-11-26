using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Assign the 6 different objects in the inspector
    public Transform[] spawnLocations; // Assign the 10 locations in the inspector

    private bool[] spawnLocationUsed; // Array to track used spawn locations

    private void Start()
    {
        if (objectsToSpawn.Length != 6)
        {
            Debug.LogError("Please assign exactly 6 different objects.");
            return;
        }

        if (spawnLocations.Length != 10)
        {
            Debug.LogError("Please assign exactly 10 locations.");
            return;
        }

        spawnLocationUsed = new bool[spawnLocations.Length]; // Initialize array to track used locations
        InstantiateObjects();
    }

    private void InstantiateObjects()
    {
        // Shuffle the locations array to get random positions
        ShuffleArray(spawnLocations);

        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            // Find an unused spawn location
            int spawnIndex = FindUnusedSpawnLocation();
            if (spawnIndex != -1)
            {
                Instantiate(objectsToSpawn[i], spawnLocations[spawnIndex].position, Quaternion.identity);
                spawnLocationUsed[spawnIndex] = true; // Mark the location as used
            }
            else
            {
                Debug.LogWarning("No available spawn locations.");
                break; // Stop spawning if no available locations
            }
        }
    }

    private int FindUnusedSpawnLocation()
    {
        for (int i = 0; i < spawnLocationUsed.Length; i++)
        {
            if (!spawnLocationUsed[i])
            {
                return i; // Return the index of the first unused location
            }
        }
        return -1; // Return -1 if all locations are used
    }

    // Fisher-Yates shuffle algorithm
    private void ShuffleArray(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
