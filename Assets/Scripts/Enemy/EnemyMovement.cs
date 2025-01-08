using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // Default movement speed
    private bool isFrozen = false; // Is the enemy currently frozen?

    private void Update()
    {
        if (!isFrozen)
        {
            // Example patrol movement logic
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    public IEnumerator FreezeMovement(float duration)
    {
        if (isFrozen) yield break; // Prevent stacking multiple freeze effects

        isFrozen = true;
        float originalSpeed = speed; // Save the current speed
        speed = 0; // Stop the enemy's movement

        // Optionally, change the enemy's color to indicate freezing
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan; // Change to icy color
        }

        yield return new WaitForSeconds(duration);

        // Restore the enemy's original speed
        speed = originalSpeed;
        isFrozen = false;

        // Restore the enemy's original color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Reset to original color
        }
    }
}
