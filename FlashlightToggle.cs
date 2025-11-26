using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; // light GameObject to work with
    public AudioSource flashlightOnAudio;
    public AudioSource flashlightOffAudio;
    private bool isOn = false; // is flashlight on or off?
    public bool isPickedUp = false; // is flashlight picked up?
    private float pickupCooldown = 0.5f; // cooldown period to prevent immediate toggling
    private float lastPickupTime;
    private Light flashlightLight; // Reference to the Light component

    // Use this for initialization
    void Start()
    {
        // Get the Light component on the lightGO
        flashlightLight = lightGO.GetComponent<Light>();

        if (flashlightLight == null)
        {
            Debug.LogError("No Light component found on the lightGO.");
            return;
        }

        // Set the initial state of the light component
        flashlightLight.enabled = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle flashlight on key down
        if (isPickedUp && Input.GetKeyDown(KeyCode.F) && Time.time > lastPickupTime + pickupCooldown)
        {
            // Toggle light
            isOn = !isOn;
            // Turn light on
            if (isOn)
            {
                flashlightLight.enabled = true;
                flashlightOnAudio.Play();
            }
            // Turn light off
            else
            {
                flashlightLight.enabled = false;
                flashlightOffAudio.Play();
            }
        }
    }

    public void PickUpFlashlight()
    {
        isPickedUp = true;
        lastPickupTime = Time.time; // Record the time of pickup
    }
}
