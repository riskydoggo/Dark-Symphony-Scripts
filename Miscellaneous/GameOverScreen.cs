using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(2);
        player.SetActive(true);
    }
}
