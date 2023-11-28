using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject objectToDuplicate; // The object you want to duplicate.
    public int InitialCreatureCount = 100; // Number of duplicates to create.
    public Vector2 mapSize = new Vector2(60f, 40f); // The size of the designated map area.

    void Start()
    {
        // Create duplicates
        for (int i = 0; i < InitialCreatureCount; i++)
        {
            // Generate random position within the map bounds
            Vector3 randomPosition = new Vector3(
                Random.Range(-mapSize.x / 2, mapSize.x / 2),
                Random.Range(-mapSize.y / 2, mapSize.y / 2), 0f
            );

            // Create a new instance of the objectToDuplicate at the random position
            Instantiate(objectToDuplicate, randomPosition, Quaternion.identity);
        }
    }
}
