using UnityEngine;

public class IceSpell : MonoBehaviour
{
    [Header("Ice Spell Settings")]
    public float freezeDuration = 1f; // Duration to freeze the enemy
    public float lifetime = 2f; // Lifetime of the spell before it's destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the spell after its lifetime
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            FreezeHandler freezeHandler = other.GetComponent<FreezeHandler>();
            if (freezeHandler != null)
            {
                Debug.Log($"Freezing {other.name} for {freezeDuration} seconds.");
                freezeHandler.ApplyFreeze(freezeDuration);
            }

            Destroy(gameObject); // Destroy the ice spell on impact
        }
    }
}
