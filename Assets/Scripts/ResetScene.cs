using System.Collections;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    public float respawnDelay = 0.7f; // Delay before respawning the player
    public AudioClip Death2;
    private AudioSource deathAudio;

    private void Start()
    {
        deathAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object is the player
        {
            deathAudio.PlayOneShot(Death2, 1.0f);
            StartCoroutine(RespawnPlayerWithDelay(other.gameObject, respawnDelay));
        }
    }

    private IEnumerator RespawnPlayerWithDelay(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Check if RespawnManager is available
        if (RespawnManager.instance != null)
        {
            Debug.Log("Respawning player using RespawnManager...");
            RespawnManager.instance.RespawnPlayer(); // Respawn player at the last checkpoint
        }
        else
        {
            Debug.LogError("RespawnManager instance not found! Ensure it exists in the scene.");
        }
    }
}
