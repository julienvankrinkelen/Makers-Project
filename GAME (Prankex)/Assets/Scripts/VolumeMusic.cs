using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeMusic : MonoBehaviour

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
        float volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        audio.volume = volumeMusicValue;
    }
}
