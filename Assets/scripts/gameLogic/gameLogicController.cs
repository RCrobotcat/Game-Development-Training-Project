using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameLogicController : MonoBehaviour
{
    public GameObject GameOverUI;

    // Update is called once per frame
    void Update()
    {
        if (playerController.instance.isDead && Time.timeScale == (1))
        {
            GameOverUI.SetActive(true);
            playerController.instance.isDead = false;
            playerController.instance.ChangeHealth(10);
            Time.timeScale = (0);
        }
    }

    public void GameOverUi_developerMode()
    {
        GameOverUI.SetActive(false);
        Time.timeScale = (1);
    }

    public void Restart()
    {
        SceneManager.LoadScene("ChatRoom");
    }

    public void quit()
    {
        Application.Quit();
    }
}
