using UnityEngine;

public class Acorn : MonoBehaviour, IInteractable
{
    public bool isHeld = false;

    public void Interact(PlayerInteract player)
    {
        if (!isHeld)
        {
            //player.HoldItem(gameObject);
            isHeld = true;
        }
    }
}
