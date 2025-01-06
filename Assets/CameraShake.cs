using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Cinemachine.CinemachineImpulseSource impulseSource;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Press 'I' to test the shake
        {
            CameraShake shake = GetComponent<CameraShake>();
            if (shake != null)
            {
                shake.Shake();
                Debug.Log("Shake triggered!");
            }
        }
    }


    public void Shake()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogError("Impulse Source is not assigned!");
        }
    }
    
}
