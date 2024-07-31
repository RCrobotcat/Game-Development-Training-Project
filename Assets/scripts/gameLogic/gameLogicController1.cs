using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameLogicController1 : MonoBehaviour
{
    public GameObject GameOverUI;
    public EnemySystem enemy;

    // Update is called once per frame
    void Update()
    {
        if (enemy.isEnd && Time.timeScale == (1))
        {
            GameOverUI.SetActive(true);
            /*enemy.isEnd = false;*/
            Time.timeScale = (0);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("BattleArena");
    }

    public void quit()
    {
        Application.Quit();
    }
}
