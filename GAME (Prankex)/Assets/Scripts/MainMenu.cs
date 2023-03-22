using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    [SerializeField] private Toggle toggleFullScreen;
    private void Start()
    {
        int startPref = PlayerPrefs.GetInt("fullScreen");
        Debug.Log(startPref);
        LoadFullScreen();
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button pressed");
        Application.Quit();
    }

    //Save fullScreen dans PlayerPrefs
    public void SaveFullScreen()
    {
        //Return value of toggle. if isOn : return TRUE. else return FALSE.
        bool fullScreen = toggleFullScreen.isOn;
        PlayerPrefs.SetInt("fullScreen", BoolToInt(fullScreen));
        LoadFullScreen();
        Debug.Log("Just Saved VALUE BOOL : " + fullScreen);
        Debug.Log("Just Saved VALUE INT : " + BoolToInt(fullScreen));
       
    }
    //Load fullScreen dans PlayerPrefs
    public void LoadFullScreen()
    {
       
       int fullScreenInt =  PlayerPrefs.GetInt("fullScreen");
       bool fullScreen = IntToBool(fullScreenInt);
       toggleFullScreen.isOn = fullScreen;
       Debug.Log("Just Loaded VALUE : " + fullScreen);
       Screen.fullScreen = fullScreen;

    }

    public int BoolToInt(bool boolean)
    {
        if (boolean)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public bool IntToBool(int integer)
    {
        if (integer == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
    
