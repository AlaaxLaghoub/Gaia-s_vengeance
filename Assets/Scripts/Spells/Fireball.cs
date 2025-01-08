using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after 3 seconds
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy fireball on impact
        }
    }
}
