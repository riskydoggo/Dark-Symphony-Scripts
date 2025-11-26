using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPosition; // The position where the object will be held by the player
    public float pickupRange = 2f; // Range within which the player can pick up objects
    public LayerMask pickupLayer; // Layer mask to specify which layers are pickable
    public Camera playerCamera; // Reference to the player's camera
    public float dropHeightOffset = 0.5f; // Offset to ensure the object is above the ground when dropped

    private GameObject pickedUpObject = null;

    private void Update()
    {
        // Check if the player presses the pickup button (e.g., "E")
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUpObject == null)
            {
                // Try to pick up an object
                TryPickupObject();
            }
        }

        // Check if the player presses the drop button (e.g., "Q")
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pickedUpObject != null)
            {
                // Drop the currently held object
                DropObject();
            }
        }

        // Debug: Draw raycast in the scene view
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * pickupRange, Color.red);
    }


    private void TryPickupObject()
    {
        // Perform a raycast from the camera to detect objects to pick up
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            if (hit.collider.CompareTag("Pickup"))
            {
                // Pick up the object
                pickedUpObject = hit.collider.gameObject;
                Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                pickedUpObject.transform.position = holdPosition.position;
                pickedUpObject.transform.parent = holdPosition;
                Debug.Log("Picked up: " + pickedUpObject.name);
            }
        }
        else
        {
            Debug.Log("No object to pick up in range.");
        }
    }

    private void DropObject()
    {
        if (pickedUpObject != null)
        {
            Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Allow physics to affect the object
                pickedUpObject.transform.parent = null; // Unparent the object from the hold position

                // Perform a raycast to detect the ground below the object
                RaycastHit hit;
                if (Physics.Raycast(pickedUpObject.transform.position, Vector3.down, out hit, Mathf.Infinity, pickupLayer))
                {
                    // Ensure the object is dropped just above the ground
                    Vector3 dropPosition = hit.point + Vector3.up * dropHeightOffset;
                    pickedUpObject.transform.position = dropPosition;
                    Debug.Log("Object dropped on the ground at: " + dropPosition);
                }
                else
                {
                    Debug.LogWarning("No ground detected. Object dropped at its current position.");
                }

                pickedUpObject.GetComponent<Collider>().enabled = true; // Re-enable the collider
                pickedUpObject = null; // Clear the reference to the picked-up object
            }
            else
            {
                Debug.LogWarning("Rigidbody component not found on picked-up object.");
            }
        }
        else
        {
            Debug.LogWarning("No object is currently picked up.");
        }
    }

}
