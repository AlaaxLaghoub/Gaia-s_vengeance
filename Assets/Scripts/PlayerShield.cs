using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private bool shieldActive = false; // Private backing field
    public float shieldDuration = 3f; // Duration for the shield
    public float shieldCooldown = 5f; // Cooldown for the shield

    private bool canUseShield = true; // If the shield can be activated

    // Public read-only property to expose shield state
    public bool IsShieldActive => shieldActive;

    void Update()
    {
        // Activate the shield when "E" is pressed, if it's not on cooldown
        if (Input.GetKeyDown(KeyCode.E) && canUseShield)
        {
            StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        shieldActive = true; // Set shield as active
        canUseShield = false;
        Debug.Log("Shield activated!");

        yield return new WaitForSeconds(shieldDuration); // Wait for shield duration

        shieldActive = false; // Deactivate shield
        Debug.Log("Shield deactivated!");

        yield return new WaitForSeconds(shieldCooldown); // Wait for cooldown

        canUseShield = true; // Allow shield to be used again
    }
}
