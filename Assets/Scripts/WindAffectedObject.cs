using UnityEngine;

public class WindAffectedObject : MonoBehaviour
{
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (WindManager.Instance != null)
        {
            float sway = Mathf.Sin(Time.time * WindManager.Instance.swaySpeed) * WindManager.Instance.windStrength;
            transform.localPosition = initialPosition + new Vector3(sway, 0, 0);
        }
        else
        {
            Debug.LogWarning("WindManager instance is not set.");
        }
    }
}
