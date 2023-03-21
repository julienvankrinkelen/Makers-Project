using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeMusicSlider = null;
    [SerializeField] private TextMeshProUGUI volumeMusicTextUI = null;

    private void Start()
    {
        LoadValues();
    }
    public void VolumeSlider(float volume)
    {
        volumeMusicTextUI.text = volume.ToString("0.0");
    }

    public void SaveVolumeButton()
    {
        float volumeMusicValue = volumeMusicSlider.value;
        PlayerPrefs.SetFloat("VolumeMusicValue", volumeMusicValue);
        LoadValues();
    }
    public void LoadValues()
    {
        float volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        volumeMusicSlider.value = volumeMusicValue;
        AudioListener.volume = volumeMusicValue;
    }
}
