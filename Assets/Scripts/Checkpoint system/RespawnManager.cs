using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public GameObject player; // The player GameObject
    public CinemachineZoom cinemachineZoom; // Reference to CinemachineZoom script
    public CameraShake cameraShake; // Reference to CameraShake script
    public Vector3 respawnOffset = new Vector3(-1f, 0f, 0f); // Offset before the checkpoint
    private Vector3 lastCheckpointPosition; // Stores the last active checkpoint position
    private bool hasCheckpoint = false; // Tracks if a checkpoint has been activated

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

    /// <summary>
    /// Sets the active checkpoint position.
    /// </summary>
    /// <param name="checkpointPosition">Position of the checkpoint.</param>
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        hasCheckpoint = true;
        Debug.Log($"Checkpoint set at position: {checkpointPosition}");
    }

    /// <summary>
    /// Respawns the player at the last active checkpoint.
    /// </summary>
    public void RespawnPlayer()
    {
        if (hasCheckpoint)
        {
            StartCoroutine(RespawnWithEffects(lastCheckpointPosition));
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Unable to respawn.");
        }
    }

    private IEnumerator RespawnWithEffects(Vector3 checkpointPosition)
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
        yield return new WaitForSeconds(0.2f);

        // Calculate the respawn position with offset
        Vector3 respawnPoint = checkpointPosition + respawnOffset;

        // Teleport player
        if (player != null)
        {
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                Debug.Log($"Teleporting player to respawn point: {respawnPoint}");
                playerScript.Teleport(respawnPoint); // Teleport to offset respawn point
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
            Debug.Log("Starting zoom in...");
            cinemachineZoom.Zoom(3f); // Adjust zoom level as needed
            yield return new WaitForSeconds(cinemachineZoom.zoomDuration); // Wait for zoom to complete

            Debug.Log("Resetting zoom to original size...");
            cinemachineZoom.ResetZoom(); // Smoothly reset to the original size
        }
        else
        {
            Debug.LogWarning("CinemachineZoom script is not assigned.");
        }
    }
}
