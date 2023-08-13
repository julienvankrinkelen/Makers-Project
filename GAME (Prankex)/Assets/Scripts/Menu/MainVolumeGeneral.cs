using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVolumeGeneral : MonoBehaviour
{
    [Header("UI sfx")]
    [SerializeField] private AudioSource menuClickSoundEffect;
    [SerializeField] private AudioSource menuBackSoundEffect;

    // Update is called once per frame
    void Update()
    {
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");

        menuClickSoundEffect.volume = volumeGeneralValue;
        menuBackSoundEffect.volume = volumeGeneralValue;
    }
}
