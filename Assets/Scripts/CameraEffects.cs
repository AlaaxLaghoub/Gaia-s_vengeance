using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.5f; // Duration of the shake effect
    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect

    [Header("Zoom Settings")]
    public float defaultZoom = 5f; // Default orthographic size for the camera
    private Camera mainCamera; // Reference to the camera
    private Vector3 originalPosition; // Original position of the camera for shake reset

    private void Awake()
    {
        // Automatically find the main camera if not explicitly assigned
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Assign it manually or ensure there is a Main Camera tagged 'MainCamera'.");
        }

        // Save the camera's initial position
        if (mainCamera != null)
        {
            originalPosition = mainCamera.transform.position;
        }
    }

    /// <summary>
    /// Shakes the camera for the configured duration and magnitude.
    /// </summary>
    public IEnumerator Shake()
    {
        if (mainCamera == null)
        {
            Debug.LogError("CameraEffects: No camera assigned for shake.");
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.position = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset the camera's position
        mainCamera.transform.position = originalPosition;
        Debug.Log("CameraEffects: Shake completed.");
    }

    /// <summary>
    /// Smoothly zooms the camera to a target orthographic size over the specified duration.
    /// </summary>
    /// <param name="targetSize">The target orthographic size.</param>
    /// <param name="duration">The duration of the zoom.</param>
    public IEnumerator Zoom(float targetSize, float duration)
    {
        if (mainCamera == null)
        {
            Debug.LogError("CameraEffects: No camera assigned for zoom.");
            yield break;
        }

        float startSize = mainCamera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        Debug.Log($"CameraEffects: Zoom completed. Final size: {targetSize}");
    }

    /// <summary>
    /// Resets the camera to its default zoom size.
    /// </summary>
    public void ResetZoom()
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = defaultZoom;
            Debug.Log($"CameraEffects: Zoom reset to default size: {defaultZoom}");
        }
        else
        {
            Debug.LogError("CameraEffects: No camera assigned for resetting zoom.");
        }
    }
}
