using UnityEngine;

public class IcePotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Unlock the ice shard spell for the player
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.UnlockIceShard(); // Unlock ice shard spell
            }

            // Destroy the potion after collection
            Destroy(gameObject);
        }
    }
}
