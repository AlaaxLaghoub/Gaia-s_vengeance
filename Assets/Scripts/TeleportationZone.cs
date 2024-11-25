using UnityEngine;

public class TeleportationZone : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination; // Assign the teleport destination in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the teleportation zone
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && teleportDestination != null)
            {
                player.Teleport(teleportDestination.position); // Use the Teleport method from the PlayerMovement script
                Debug.Log("Player teleported to: " + teleportDestination.position);
            }
        }
    }
}
