using UnityEngine;

public class FirePotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Unlock the fireball spell for the player
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.UnlockFireball();
            }

            // Destroy the potion after collection
            Destroy(gameObject);
        }
    }
}
