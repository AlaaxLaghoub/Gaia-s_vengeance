using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damageAmount = 20; // Amount of health to deduct

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the spikes
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.takeDamage(damageAmount);
            }
        }
    }
}
