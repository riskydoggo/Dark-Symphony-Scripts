using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        if (lightSource == null)
        {
            Debug.LogError("LightFlicker script must be attached to a GameObject with a Light component.");
        }
    }

    private void Update()
    {
        if (lightSource != null)
        {
            lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f));
        }
    }
}
