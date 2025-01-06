using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Cinemachine.CinemachineImpulseSource impulseSource;

    /// <summary>
    /// Triggers a camera shake using the Cinemachine Impulse system.
    /// </summary>
    public void Shake()
    {
        if (impulseSource != null)
        {
            Debug.Log("Generating impulse for shake...");
            impulseSource.GenerateImpulse(); // Generates the shake impulse
        }
        else
        {
            Debug.LogError("Impulse Source is not assigned!");
        }
    }
}
