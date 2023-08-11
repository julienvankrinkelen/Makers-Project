using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int saveExists;

    public GameObject panelChoice;
    public GameObject mainMenu;

    [SerializeField] private AudioSource clickSoundEffect;
    [SerializeField] private AudioSource backSoundEffect;
  
    public void PlayGame()
    {
        
        saveExists = PlayerPrefs.GetInt("Save Exists");
        print(" IN PLAY GAME FCT : saveExists : " + saveExists);
        if (saveExists == 1)
        {
            panelChoice.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            NewGame();
        }
    }

    public void LoadGame()
    {
        saveExists = PlayerPrefs.GetInt("Save Exists");
        print("SAVE EXIST : " + saveExists);
        if (saveExists == 1)
        {
            SceneManager.LoadScene("new map");
            PlayerPrefs.SetInt("JustLoadedScene", 1);
        }
        else
        {
            Debug.Log("Aucune Save Disponible");
        }
       
    }
    public void QuitGame()
    {
        Debug.Log("Quit Button pressed");
        Application.Quit();
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("JustDeleteSave", 1);
        SceneManager.LoadScene("new map");
    }

    public void SoundButton()
    { 
        clickSoundEffect.Play();
    }
    public void SoundBack()
    {
        backSoundEffect.Play();
    }


}