using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private GlobalLightController lightController; // Reference to the light controller
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Key to interact

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            lightController.TurnLightBackOn();
            gameObject.SetActive(false); // Disable the switch after interaction (optional)
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
