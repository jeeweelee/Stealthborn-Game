using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMesh operations

public class AISpawner : MonoBehaviour
{
    public int AICount = 5;          // Number of AI to spawn
    public GameObject AIGameObject; // AI prefab to spawn
    public GameObject ground;       // Ground object for reference
    public float spawnRadius = 20f; // Radius within which AI can spawn
    public float positionCheckRadius = 1f; // Size of the overlap check box

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ground.transform.position;

        for (int i = 0; i < AICount; i++)
        {
            var position = GetAIPosition();

            if (position.HasValue)
            {
                Instantiate(AIGameObject, position.Value, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid position for AI.");
            }
        }
    }

    private Vector3? GetAIPosition()
    {
        for (int attempts = 0; attempts < 100; attempts++) // Limit attempts to prevent infinite loops
        {
            var randomPosition = new Vector3(
                Random.Range(-spawnRadius, spawnRadius) + ground.transform.position.x,
                ground.transform.position.y,
                Random.Range(-spawnRadius, spawnRadius) + ground.transform.position.z
            );

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, positionCheckRadius, NavMesh.AllAreas))
            {
                var colliders = Physics.OverlapBox(hit.position, new Vector3(positionCheckRadius, 0.5f, positionCheckRadius));
                if (colliders.Length == 0)
                {
                    return hit.position;
                }
            }
        }

        return null;
    }
}
