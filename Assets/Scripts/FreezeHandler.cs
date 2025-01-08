using System.Collections;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
    private bool isFrozen = false;
    private float originalSpeed;
    private PatrolEnemy patrolEnemy;
    private SpiderEnemyAI spiderEnemyAI;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Collider2D attackCollider; // To disable attack detection

    private void Awake()
    {
        patrolEnemy = GetComponent<PatrolEnemy>();
        spiderEnemyAI = GetComponent<SpiderEnemyAI>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider = GetComponent<Collider2D>();

        if (patrolEnemy != null)
        {
            originalSpeed = patrolEnemy.speed;
        }
        else if (spiderEnemyAI != null)
        {
            originalSpeed = spiderEnemyAI.speed;
        }
    }

    public void ApplyFreeze(float freezeDuration)
    {
        if (isFrozen) return; // Avoid re-freezing an already frozen object

        isFrozen = true;

        // Stop enemy movement
        if (patrolEnemy != null)
        {
            patrolEnemy.speed = 0;
        }
        else if (spiderEnemyAI != null)
        {
            spiderEnemyAI.speed = 0;
        }

        // Disable attack capabilities
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        // Completely disable the Animator to stop animations
        if (anim != null)
        {
            anim.enabled = false;
        }

        // Change color to indicate freezing (optional)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan;
        }

        // Start unfreezing process
        StartCoroutine(UnfreezeAfterDelay(freezeDuration));
    }

    private IEnumerator UnfreezeAfterDelay(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);

        // Restore enemy speed
        if (patrolEnemy != null)
        {
            patrolEnemy.speed = originalSpeed;
        }
        else if (spiderEnemyAI != null)
        {
            spiderEnemyAI.speed = originalSpeed;
        }

        // Re-enable attack capabilities
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        // Re-enable animations
        if (anim != null)
        {
            anim.enabled = true;
        }

        // Restore original color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        isFrozen = false; // Mark as unfrozen
        Debug.Log($"{gameObject.name} is now unfrozen.");
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}
