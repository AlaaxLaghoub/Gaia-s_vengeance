using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomDuration = 1f;

    public void Zoom(float targetSize)
    {
        StartCoroutine(ZoomCoroutine(targetSize));
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

        while (elapsed < zoomDuration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}
