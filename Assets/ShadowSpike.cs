using System.Collections;
using UnityEngine;

public class ShadowSpike : MonoBehaviour
{
    public int damage = 20;
    public float lifetime = 5f;
    public float riseDuration = 1f; // Time for the spike to rise
    public float riseHeight = 2f; // Distance the spike rises

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;

        // Start rising animation
        StartCoroutine(RiseUp());

        // Destroy the spike after its lifetime
        Destroy(gameObject, lifetime);
    }

    private IEnumerator RiseUp()
    {
        Vector3 targetPosition = initialPosition + Vector3.up * riseHeight;
        float elapsedTime = 0f;

        while (elapsedTime < riseDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure spike reaches exact target position
        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.takeDamage(damage);
                Debug.Log("Player hit by spike!");
            }

            // Optionally destroy the spike on impact
            Destroy(gameObject);
        }
    }
}
