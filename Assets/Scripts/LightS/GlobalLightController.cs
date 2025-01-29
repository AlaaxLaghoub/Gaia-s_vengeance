using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for 2D Lights
using System.Collections;
public class GlobalLightController : MonoBehaviour
{
    public Light2D globalLight; // Assign the Global Light 2D component
    public float lightOffIntensity = 0f; // Intensity when the light is "off"
    public float lightOnIntensity = 1f; // Intensity when the light is "on"
    public float transitionDuration = 1f; // Time for intensity transition

    private bool isLightOff = false; // Track if the light is currently off

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLightOff)
        {
            StartCoroutine(ChangeLightIntensity(lightOffIntensity));
            isLightOff = true; // Mark the light as off
        }
    }

    public void TurnLightBackOn()
    {
        if (isLightOff)
        {
            StartCoroutine(ChangeLightIntensity(lightOnIntensity));
            isLightOff = false; // Mark the light as on
        }
    }

    private IEnumerator ChangeLightIntensity(float targetIntensity)
    {
        float startIntensity = globalLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        globalLight.intensity = targetIntensity;
    }
}