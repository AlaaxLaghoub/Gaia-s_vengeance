using System.Collections;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject projectilePrefab; // The projectile prefab
    public Transform firePoint; // Where the projectile spawns
    public float attackInterval = 2f; // Time between attacks
    public float projectileSpeed = 5f; // Speed of the projectile

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    private bool isAttacking = false;

    private void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            if (!isAttacking)
            {
                ShootProjectile();
            }
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * projectileSpeed * Mathf.Sign(transform.localScale.x); // Shoot based on enemy's facing direction
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }
}
