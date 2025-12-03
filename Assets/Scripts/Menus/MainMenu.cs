using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene1()
    {
        //Later, this will look at the save data and load the current scene that the player is at
        SceneManager.LoadSceneAsync("Scene_1", LoadSceneMode.Single);
    }

    public void LoadScene2()
    {
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
