using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private void Start()
    {
        gameObject.tag = "Pickup"; // Ensure the object has the "Pickup" tag
        GetComponent<Rigidbody>().isKinematic = true; // Make the Rigidbody kinematic by default
    }
}
