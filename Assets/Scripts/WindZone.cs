using UnityEngine;

public class WindZone : MonoBehaviour
{
    [Header("Wind Settings")]
    public float windForce = 10f; // The strength of the wind
    public Vector2 windDirection = Vector2.left; // Direction of the wind (default: left)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.EnterWindZone(windDirection.normalized * windForce);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ExitWindZone();
            }
        }
    }
}
