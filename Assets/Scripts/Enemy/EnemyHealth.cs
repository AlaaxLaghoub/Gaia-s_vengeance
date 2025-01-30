using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f; // Enemy's health
    private Animator anim; // Reference to the Animator
    private bool isDead = false; // To track if the enemy is already dead

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("No Animator component found on this object!");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent further damage after death

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Prevent multiple calls to Die()

        isDead = true; // Set the enemy's state to dead
        anim.SetTrigger("Die"); // Trigger the death animation

        // Disable all colliders on the enemy
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        // Disable movement and attack scripts
        PatrolEnemy patrolEnemy = GetComponent<PatrolEnemy>();
        if (patrolEnemy != null)
        {
            patrolEnemy.enabled = false;
        }

        // Optionally disable other scripts or behaviors
        FreezeHandler freezeHandler = GetComponent<FreezeHandler>();
        if (freezeHandler != null)
        {
            freezeHandler.enabled = false;
        }

        Debug.Log($"{gameObject.name} is now dead, colliders and movement disabled.");
    }
}
