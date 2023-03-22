using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeGeneralSlider;

    [SerializeField] private TextMeshProUGUI volumeMusicTextUI;
    [SerializeField] private TextMeshProUGUI volumeGeneralTextUI;


    private void Start()
    {
        LoadValues();
    }
    public void VolumeMusicSlider(float volume)
    {
        volumeMusicTextUI.text = volume.ToString("0.0");
    }

    public void VolumeGeneralSlider(float volume)
    {
        volumeGeneralTextUI.text = volume.ToString("0.0");
    }

    public void SaveVolumeButton()
    {
        float volumeMusicValue = volumeMusicSlider.value;
        float volumeGeneralValue = volumeGeneralSlider.value;
        PlayerPrefs.SetFloat("VolumeGeneralValue", volumeGeneralValue);
        PlayerPrefs.SetFloat("VolumeMusicValue", volumeMusicValue);
        LoadValues();
    }
    public void LoadValues()
    {
        float volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        volumeMusicSlider.value = volumeMusicValue;
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");
        volumeGeneralSlider.value = volumeGeneralValue;
       
       
    }
}
