using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGeneral : MonoBehaviour
{

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");
        audio.volume = volumeGeneralValue;
    }
}
