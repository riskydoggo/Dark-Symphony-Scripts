using UnityEngine;

public class Record : MonoBehaviour, IInteractable
{
    public AudioSource recordAudioSource;
    public bool isHeld = false;

    public void Interact(PlayerInteract player)
    {
        if (!isHeld && player.heldItem == null)
        {
            //player.HoldItem(gameObject);
            isHeld = true;
        }
    }
}
