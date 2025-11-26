using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    public Transform headTransform; // The transform of the character's head
    public Transform playerCameraTransform; // The transform of the player's camera
    public float rotationSpeed = 5f; // Speed of the head rotation
    public float chaseDistance = 10f; // Distance within which the head should chase the player

    private Quaternion initialHeadRotation; // Initial rotation of the head

    void Start()
    {
        if (headTransform == null)
        {
            Debug.LogError("Head Transform is not assigned.");
            return;
        }

        initialHeadRotation = headTransform.localRotation; // Store the initial rotation
    }

    void LateUpdate()
    {
        if (headTransform == null || playerCameraTransform == null)
        {
            Debug.LogWarning("Required components are not assigned.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerCameraTransform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            // Calculate direction to player
            Vector3 directionToPlayer = playerCameraTransform.position - headTransform.position;
            directionToPlayer.y = 0; // Keep head level

            // Create target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate head towards player
            headTransform.localRotation = Quaternion.Slerp(headTransform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Smoothly return to initial rotation when out of range
            headTransform.localRotation = Quaternion.Slerp(headTransform.localRotation, initialHeadRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
