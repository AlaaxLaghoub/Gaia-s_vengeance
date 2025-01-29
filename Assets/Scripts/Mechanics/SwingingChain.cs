using UnityEngine;

public class SwingingChain : MonoBehaviour
{
    public float swingSpeed = 2f; // Speed of the swing
    public float swingAngle = 45f; // Maximum angle to swing

    private float startAngle; // Initial angle

    private void Start()
    {
        // Store the starting rotation angle
        startAngle = transform.eulerAngles.z;
    }

    private void Update()
    {
        // Calculate the new angle using a sine wave for smooth swinging
        float angle = startAngle + Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.takeDamage(20); // Deal damage to the player
                Debug.Log("Player hit by the swinging chain!");
            }
        }
    }
}
