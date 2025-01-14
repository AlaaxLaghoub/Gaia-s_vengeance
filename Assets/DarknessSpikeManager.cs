using System.Collections;
using UnityEngine;

public class DarknessSpikeManager : MonoBehaviour
{
    public GameObject shadowSpikePrefab; // Spike prefab
    public Transform[] spawnPoints; // Predefined spike spawn locations
    public float spawnInterval = 2f; // Time between spike spawns

    private DarknessZone darknessZone;

    private void Start()
    {
        // Get the DarknessZone script in the scene
        darknessZone = FindObjectOfType<DarknessZone>();
        if (darknessZone == null)
        {
            Debug.LogError("No DarknessZone found in the scene!");
            return;
        }

        // Start spike spawning
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        while (true)
        {
            if (darknessZone.isPlayerInDarkness) // Check if the player is in darkness
            {
                SpawnSpike();
            }
            else
            {
                Debug.Log("Player is not in darkness. No spikes spawned.");
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSpike()
    {
        if (spawnPoints.Length == 0 || shadowSpikePrefab == null)
        {
            Debug.LogWarning("No spawn points or spike prefab assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(shadowSpikePrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Spawned spike at {spawnPoint.position}");
    }
}
