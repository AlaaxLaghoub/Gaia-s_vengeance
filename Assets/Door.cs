using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = true;
    [SerializeField] GameObject myDoor;
    public bool KeyPickedUp;

    [SerializeField] GlobalLightController lightController; // 🔆 Add reference to light controller

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key") && KeyPickedUp)
        {
            Debug.Log("Key collided with door");
            locked = false;
            lightController.TurnLightBackOn(); // 🔆 Turn on the light here
            Destroy(myDoor, 0.6f);
            other.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Key") && KeyPickedUp)
        {
            locked = true;
        }
    }
}
