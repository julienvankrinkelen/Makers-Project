using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControl: MonoBehaviour
{
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeGeneralSlider;

    [SerializeField] private TextMeshProUGUI volumeMusicTextUI;
    [SerializeField] private TextMeshProUGUI volumeGeneralTextUI;

    [SerializeField] private Toggle toggleFullScreen;

    public VolumeMusic volumeMusic;
    private void Start()
    {
        int startPref = PlayerPrefs.GetInt("fullScreen");
        Debug.Log(startPref);
        LoadFullScreen();

        LoadValues();
    }
    public void VolumeMusicSlider(float volume)
    {
        volumeMusicTextUI.text = Mathf.Round(volume * 100).ToString() + "%";

    }

    public void VolumeGeneralSlider(float volume)
    {
        volumeGeneralTextUI.text = Mathf.Round(volume * 100).ToString() + "%";
    }

    public void SaveVolumeButton()
    {
        float volumeMusicValue = volumeMusicSlider.value;
        float volumeGeneralValue = volumeGeneralSlider.value;
        PlayerPrefs.SetFloat("VolumeGeneralValue", volumeGeneralValue);
        PlayerPrefs.SetFloat("VolumeMusicValue", volumeMusicValue);
        LoadValues();
        volumeMusic.ChangeVolumeMusic();
    }
    public void LoadValues()
    {
        float volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        volumeMusicSlider.value = volumeMusicValue;
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");
        volumeGeneralSlider.value = volumeGeneralValue;
       
       
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

        int fullScreenInt = PlayerPrefs.GetInt("fullScreen");
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
    

