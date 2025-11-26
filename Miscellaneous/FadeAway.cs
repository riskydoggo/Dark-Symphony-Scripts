using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    public float fadeTime = 2f;  // Example fade time
    private TextMeshProUGUI fadeAwayText;
    private float alphavalue;
    private float fadeAwayPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        fadeAwayText = GetComponent<TextMeshProUGUI>();
        fadeAwayPerSecond = 1 / fadeTime;
        alphavalue = fadeAwayText.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            alphavalue -= Time.deltaTime * fadeAwayPerSecond;
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphavalue);
        }
    }

    // Method to reset the alpha and start fading again
    public void ResetAlphaAndFade()
    {
        alphavalue = 1f;  // Set alpha to 1 (fully visible)
        fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphavalue);

        fadeTime = 5f; // Reset the fade time (adjust as needed)
    }
}
