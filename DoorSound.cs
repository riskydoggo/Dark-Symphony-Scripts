using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSoundWithVelocity : MonoBehaviour
{
    public AudioSource openSound;
    public AudioSource closeSound;
    public Transform playerTransform;
    public float maxDistance = 20f; // The maximum distance at which the sound is heard
    public float initialVolume = 0.5f; // Set an appropriate initial volume
    public float velocityThreshold = 0.1f; // The threshold for door velocity to trigger the sound

    private HingeJoint hinge;
    private bool isMoving = false;

    private void Start()
    {
        hinge = GetComponent<HingeJoint>();
        if (hinge == null)
        {
            Debug.LogWarning("HingeJoint component not found on the door.");
        }

        // Set initial volume levels
        openSound.volume = initialVolume;
        closeSound.volume = initialVolume;

        // Configure the audio source for 3D sound
        openSound.spatialBlend = 1.0f; // Fully 3D
        closeSound.spatialBlend = 1.0f; // Fully 3D

        openSound.spread = 180f; // Semi-diffuse sound
        closeSound.spread = 180f; // Semi-diffuse sound

        // Set custom roll-off for better distance attenuation control
        openSound.rolloffMode = AudioRolloffMode.Linear;
        closeSound.rolloffMode = AudioRolloffMode.Linear;

        openSound.maxDistance = maxDistance;
        closeSound.maxDistance = maxDistance;
    }

    private void Update()
    {
        if (hinge != null)
        {
            // Get the angular velocity of the hinge joint
            float angularVelocity = hinge.velocity;

            // Check if the door is moving above the velocity threshold
            if (!isMoving && Mathf.Abs(angularVelocity) > velocityThreshold)
            {
                isMoving = true;
                PlaySound(openSound);
            }

            // Check if the door has stopped moving (below the velocity threshold)
            if (isMoving && Mathf.Abs(angularVelocity) <= velocityThreshold)
            {
                isMoving = false;
                PlaySound(closeSound);
            }

            // Adjust sound volume based on player distance
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            float volume = Mathf.Clamp(1 - (distance / maxDistance), 0, initialVolume);
            openSound.volume = volume;
            closeSound.volume = volume;
        }
    }

    private void PlaySound(AudioSource sound)
    {
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
}
