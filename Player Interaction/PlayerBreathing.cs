using UnityEngine;

public class PlayerBreathing : MonoBehaviour
{
    // Reference to the player controller for stamina
    public FirstPersonController controller;
    public SpeedrunManager speedrunManager;

    private AudioSource breathingSound;  // Reference to the audio source playing breath sounds

    // Breathing intensity settings
    public float maxBreathingPitch = 1.5f; // Calm breathing pitch (at full stamina)
    public float minBreathingPitch = 2f;   // Heavy breathing pitch (when out of stamina)

    public float volumeMax = 1f;  // Max volume when fresh (full stamina)
    public float volumeMin = 0.5f;  // Min volume when out of breath (empty stamina)

    void Start()
    {
        // Get the AudioSource component (this assumes it's attached to the same GameObject)
        breathingSound = GetComponent<AudioSource>();

        // Start the breathing sound at a random point
        //StartBreathingAtRandomPoint();
    }

    void Update()
    {
        // Debug the pitch and volume to check if they are updating
        //Debug.Log("Sprint Remaining: " + controller.sprintRemaining);
        //Debug.Log("Pitch: " + breathingSound.pitch);
        //Debug.Log("Volume: " + breathingSound.volume);

        // Update breathing sound based on current sprintRemaining
        UpdateBreathingSounds();

        // Check if the current clip has looped and update the random start point
        if (!breathingSound.isPlaying && speedrunManager.isRunStarted)
        {
            StartBreathingAtRandomPoint();
        }
    }

    void UpdateBreathingSounds()
    {
        // Normalize the sprintRemaining to a ratio between 0 and 1
        float sprintRatio = controller.sprintRemaining / controller.sprintDuration;

        // Adjust the pitch of the breathing sound based on sprintRemaining (more tired = higher pitch)
        breathingSound.pitch = Mathf.Lerp(maxBreathingPitch, minBreathingPitch, 1 - sprintRatio);

        // Adjust the volume based on sprintRemaining (more tired = louder breath)
        breathingSound.volume = Mathf.Lerp(volumeMin, volumeMax, 1 - sprintRatio);  // Reversed: volume increases as sprintRemaining decreases
    }

    // Method to start the breathing sound at a random point in the audio clip
    void StartBreathingAtRandomPoint()
    {
        // Generate a random time between 0 and the length of the audio clip
        float randomStartTime = Random.Range(0f, breathingSound.clip.length);

        // Set the AudioSource's time to the random start time
        breathingSound.time = randomStartTime;

        // Play the sound (it will loop, but start at the random point)
        breathingSound.Play();
    }

    // Example methods for reducing and restoring stamina (you can adapt these to your existing logic)
    public void ReduceStamina(float amount)
    {
        controller.sprintRemaining -= amount;
        controller.sprintRemaining = Mathf.Clamp(controller.sprintRemaining, 0f, controller.sprintDuration);  // Ensure it's clamped between 0 and 7.5
    }

    public void RestoreStamina(float amount)
    {
        controller.sprintRemaining += amount;
        controller.sprintRemaining = Mathf.Clamp(controller.sprintRemaining, 0f, controller.sprintDuration);  // Ensure it's clamped between 0 and 7.5
    }
}
