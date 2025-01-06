using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomDuration = 0.5f;
    private float originalSize; // Store the original orthographic size

    private void Start()
    {
        if (virtualCamera != null)
        {
            originalSize = virtualCamera.m_Lens.OrthographicSize; // Initialize the original size
        }
        else
        {
            Debug.LogError("CinemachineZoom: Virtual Camera is not assigned!");
        }
    }

    public void Zoom(float targetSize)
    {
        StartCoroutine(ZoomCoroutine(targetSize));
    }

    public void ResetZoom()
    {
        StartCoroutine(ZoomCoroutine(originalSize)); // Reset to the original size
    }

    private IEnumerator ZoomCoroutine(float targetSize)
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineZoom: Virtual Camera is not assigned!");
            yield break;
        }

        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsed = 0f;

        Debug.Log($"Starting zoom from {startSize} to {targetSize}");

        while (elapsed < zoomDuration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
        Debug.Log($"Zoom completed: Final size = {targetSize}");
    }
}
