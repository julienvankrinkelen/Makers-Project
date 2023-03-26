using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int JustLoadedScene;
    public void PlayGame()
    {
        SceneManager.LoadScene("Try map");
       
    }

    public void LoadGame()
    {
        // 1 = true; 0 = false
        JustLoadedScene = 1;
        PlayerPrefs.SetInt("JustLoadedScene", JustLoadedScene);
        SceneManager.LoadScene("Try map");
    }
    public void QuitGame()
    {
        Debug.Log("Quit Button pressed");
        Application.Quit();
    }

 
}
    
