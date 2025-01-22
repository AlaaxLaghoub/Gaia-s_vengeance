using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShield playerShield = other.GetComponent<PlayerShield>();

            if (playerShield != null && playerShield.IsShieldActive) // Use the property IsShieldActive
            {
                Debug.Log("Shield blocked the projectile!");
                Destroy(gameObject); // Destroy the projectile when shield is active
            }
            else
            {
                // Handle damage to the player
                PlayerMovement player = other.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.takeDamage(damage);
                }

                Destroy(gameObject); // Destroy the projectile after hitting the player
            }
        }
    }
}
