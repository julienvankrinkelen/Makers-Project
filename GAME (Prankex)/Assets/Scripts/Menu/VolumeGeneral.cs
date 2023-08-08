using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGeneral : MonoBehaviour
{

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");
        audioSource.volume = volumeGeneralValue;
    }
}
