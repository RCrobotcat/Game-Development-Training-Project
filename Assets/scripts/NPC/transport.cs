using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transport : MonoBehaviour
{
    public GameObject transportSound;

    // Update is called once per frame
    void Update()
    {

    }

    public void transportScene()
    {
        SceneManager.LoadScene("BattleArena");
    }

    public void transportSoundActive()
    {
        transportSound.SetActive(true);
    }
}
