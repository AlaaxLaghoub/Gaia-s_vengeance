using UnityEngine;

public class BatDeath : MonoBehaviour
{
    [Header("Health")]
    public float health = 50f;
    private bool isDead = false;

    [Header("Audio")]
    public AudioClip deathClip;
    private AudioSource audioSource;

    [Header("Fall Settings")]
    public float gravityScaleOnDeath = 5f; // Make it fall faster

    private Rigidbody2D rb;
    private Collider2D[] colliders;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        if (rb == null)
            Debug.LogError("Missing Rigidbody2D on Bat!");
        if (audioSource == null)
            Debug.LogError("Missing AudioSource on Bat!");
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Enable gravity so the bat falls
        rb.gravityScale = gravityScaleOnDeath;
        rb.velocity = new Vector2(0, -1f); // Slight nudge downward

        // Optionally rotate or flip for effect
        rb.angularVelocity = 200f;

        // Play death sound
        if (deathClip != null)
            audioSource.PlayOneShot(deathClip);

        // Disable any extra scripts
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();
        foreach (var script in allScripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // Disable colliders after some time (e.g., so it doesn’t keep blocking stuff)
        Invoke(nameof(DisableColliders), 1f);

        // Destroy after a few seconds
        Destroy(gameObject, 3f);
    }

    void DisableColliders()
    {
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }

    // For test: press K to kill bat
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100f);
        }
    }
}
