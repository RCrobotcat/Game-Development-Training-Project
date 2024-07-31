using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartMenu()
    {
        SceneManager.LoadScene("ChatRoom");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
