using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager Instance { get; private set; }

    public float windStrength = 1f;
    public Vector2 windDirection = Vector2.right;

    // Add swaySpeed if it was missing
    public float swaySpeed = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of WindManager found! Destroying duplicate.");
            Destroy(gameObject);
        }
    }
}
