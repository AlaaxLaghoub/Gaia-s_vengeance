using System.Collections;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
    private bool isFrozen = false;
    private float originalSpeed;
    private PatrolEnemy patrolEnemy;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        patrolEnemy = GetComponent<PatrolEnemy>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (patrolEnemy != null)
        {
            originalSpeed = patrolEnemy.speed;
        }
    }

    public void ApplyFreeze(float freezeDuration)
    {
        if (isFrozen) return; // Avoid re-freezing an already frozen object

        isFrozen = true;

        // Stop patrol enemy movement
        if (patrolEnemy != null)
        {
            patrolEnemy.speed = 0;
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

        // Restore patrol enemy speed
        if (patrolEnemy != null)
        {
            patrolEnemy.speed = originalSpeed;
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
