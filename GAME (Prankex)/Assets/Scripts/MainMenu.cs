using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    public void PlayGame()
    {
        SceneManager.LoadScene("Try map");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button pressed");
        Application.Quit();
    }

 
}
    
