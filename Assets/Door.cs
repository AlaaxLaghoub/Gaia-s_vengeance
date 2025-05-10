using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
    public bool locked = true;
    [SerializeField] GameObject myDoor;
    public bool KeyPickedUp;

    [SerializeField] GlobalLightController lightController; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key") && KeyPickedUp)
        {
            Debug.Log("Key collided with door");
            locked = false;
            lightController.TurnLightBackOn(); 
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
