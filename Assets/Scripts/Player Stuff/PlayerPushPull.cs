using UnityEngine;

public class PlayerPushPull : MonoBehaviour
{
    [SerializeField] private float pushStrength = 10f; // Strength of pushing

    private Rigidbody2D playerRb;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovableBlock"))
        {
            Rigidbody2D blockRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (blockRb != null)
            {
                Vector2 pushDirection = collision.GetContact(0).normal * -1; // Opposite of contact point normal
                blockRb.AddForce(pushDirection * pushStrength, ForceMode2D.Impulse);
            }
        }
    }
}
