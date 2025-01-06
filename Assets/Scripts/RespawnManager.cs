using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public GameObject player; // The player GameObject
    public CinemachineZoom cinemachineZoom; // Reference to CinemachineZoom script
    public CameraShake cameraShake; // Reference to CameraShake script

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RespawnPlayer(Vector3 respawnPoint)
    {
        StartCoroutine(RespawnWithEffects(respawnPoint));
    }

    private IEnumerator RespawnWithEffects(Vector3 respawnPoint)
    {
        // Trigger camera shake
        if (cameraShake != null)
        {
            Debug.Log("Triggering camera shake...");
            cameraShake.Shake();
        }
        else
        {
            Debug.LogWarning("CameraShake script is not assigned.");
        }

        // Wait for shake duration
        yield return new WaitForSeconds(0.5f);

        // Teleport player
        if (player != null)
        {
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                Debug.Log("Teleporting player to respawn point...");
                playerScript.Teleport(respawnPoint);
                playerScript.Heal(playerScript.maxHealth); // Restore health
                playerScript.ResetAnimationState(); // Reset animations
            }
            else
            {
                Debug.LogError("PlayerMovement script is not attached to the player.");
            }
        }

        // Trigger zoom effect
        if (cinemachineZoom != null)
        {
            Debug.Log("Starting zoom effect...");
            cinemachineZoom.Zoom(0.2f); // Zoom in
        }
        else
        {
            Debug.LogWarning("CinemachineZoom script is not assigned.");
        }

        // Wait for zoom duration
        yield return new WaitForSeconds(1f);

        // Reset zoom to default
        if (cinemachineZoom != null)
        {
            Debug.Log("Resetting zoom to default...");
            cinemachineZoom.Zoom(5f); // Reset zoom
        }
    }
}
