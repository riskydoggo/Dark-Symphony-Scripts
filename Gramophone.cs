using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Gramophone : MonoBehaviour, IInteractable
{
    public int gramophoneID;
    public bool isRecordPlaced = false;

    public void Interact(PlayerInteract player)
    {
        if (player.heldItem != null && player.heldItem.GetComponent<Record>() != null)
        {
            Record record = player.heldItem.GetComponent<Record>();
            record.transform.SetParent(transform);
            record.transform.localPosition = Vector3.zero;
            record.transform.localRotation = Quaternion.identity;
            Rigidbody rb = record.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            record.recordAudioSource.Play();
            player.heldItem = null;
        }
    }
    private void Update()
    {

    }
}
