using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject Camera, StartUI, OptionsUI, TipsUI; //148.5 is default y rotation
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowOptions()
    {
        // y == 188
        Camera.transform.rotation = Quaternion.Euler(-8.73f, 188f, 0);
        StartUI.SetActive(false);
        OptionsUI.SetActive(true);
    }

    public void ShowTips()
    {
        // y = 52
        Camera.transform.rotation = Quaternion.Euler(-8.73f, 52f, 0);
        StartUI.SetActive(false);
        TipsUI.SetActive(true);
        Debug.Log("Tips loaded");
    }

    public void GiveUp()
    {
        Debug.Log("Player gave up lmao");
        Application.Quit();
    }

    public void SpeedrunMode()
    {

    }

    public void BackButton()
    {
        Camera.transform.rotation = Quaternion.Euler(-8.73f, 148.5f, 0);
        StartUI.SetActive(true);
        TipsUI.SetActive(false);
        OptionsUI.SetActive(false);
    }
    public void VinylButton()
    {
        SceneManager.LoadScene(2);
    }
}
