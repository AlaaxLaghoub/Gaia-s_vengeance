using UnityEngine;

public class DarknessZone : MonoBehaviour
{
    public bool isPlayerInDarkness = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInDarkness = true;
            Debug.Log("Player is now in darkness.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInDarkness = false;
            Debug.Log("Player left the darkness.");
        }
    }

    private void Update()
    {
        // Log the state to confirm updates
        Debug.Log($"isPlayerInDarkness state: {isPlayerInDarkness}");
    }
}
