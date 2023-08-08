using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeMusic : MonoBehaviour

{
     private AudioSource audioSource;
     private float volumeMusicValue;
// Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        audioSource.volume = volumeMusicValue;
    }
}
