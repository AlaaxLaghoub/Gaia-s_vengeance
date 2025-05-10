using UnityEngine;

public class BatHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took damage: " + amount);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Enable gravity so the bat falls
        if (rb != null)
        {
            rb.gravityScale = 1f; // Make sure gravity is on
            rb.velocity = Vector2.zero; // Stop any movement
        }

        // Optional: disable AI script so it stops chasing
        MonoBehaviour aiScript = GetComponent<SpiderEnemyAI>();
        if (aiScript != null)
        {
            aiScript.enabled = false;
        }

        // Optional: change layer or tag so it's no longer treated as an enemy
        gameObject.tag = "Untagged";

        // Optionally destroy after delay
        Destroy(gameObject, 3f);
    }
}
