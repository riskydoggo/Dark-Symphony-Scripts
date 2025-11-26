using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioDistortionFilter))]
public class DistortionController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioDistortionFilter distortionFilter;

    [Range(0f, 1f)]
    public float distortionLevel = 0.5f;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Get the AudioDistortionFilter component
        distortionFilter = GetComponent<AudioDistortionFilter>();

        // Set initial distortion level
        distortionFilter.distortionLevel = distortionLevel;

        // Play the audio
        audioSource.Play();
    }

    void Update()
    {
        // Example: Adjust the distortion level based on user input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            distortionLevel += 0.01f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            distortionLevel -= 0.01f;
        }

        // Clamp the distortion level to a valid range
        distortionLevel = Mathf.Clamp(distortionLevel, 0f, 1f);

        // Update the distortion filter's distortion level
        distortionFilter.distortionLevel = distortionLevel;
    }
}
