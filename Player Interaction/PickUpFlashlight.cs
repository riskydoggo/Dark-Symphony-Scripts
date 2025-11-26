using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlashlight : MonoBehaviour
{
    public GameObject FlashlightOnPlayer;
    public GameObject PickUpText;
    public GameObject FlashLightTipText;
    public GameObject RealFlashlight;

    private Outline outline; // Cached reference to the Outline component

    void Start()
    {
        FlashlightOnPlayer.SetActive(false);
        PickUpText.SetActive(false);
        FlashLightTipText.SetActive(false);
        RealFlashlight.SetActive(true);

        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Ensure the outline is initially disabled
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUpText.SetActive(true);
            if (outline != null)
            {
                outline.enabled = true; // Enable the outline
            }

            if (Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                FlashlightOnPlayer.SetActive(true);
                PickUpText.SetActive(false);
                FlashLightTipText.SetActive(true);
                RealFlashlight.SetActive(true);
                FlashlightOnPlayer.GetComponent<FlashlightToggle>().PickUpFlashlight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUpText.SetActive(false);
            if (outline != null)
            {
                outline.enabled = false; // Disable the outline
            }
        }
    }
}
