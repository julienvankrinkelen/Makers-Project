using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeMusic : MonoBehaviour

{
    [SerializeField] private AudioSource audio1;
    [SerializeField] private AudioSource audio2;
    [SerializeField] private AudioSource audio3;
    [SerializeField] private AudioSource audioBoss;
    
    private float volumeMusicValue;

    private void Start()
    {
        ChangeVolumeMusic();
    }
    // Update is called once per frame
    public void ChangeVolumeMusic()
    {
        volumeMusicValue = PlayerPrefs.GetFloat("VolumeMusicValue");
        if (audio1 != null){audio1.volume = volumeMusicValue;}
        if (audio2 != null){audio2.volume = volumeMusicValue;}
        if (audio3 != null){audio3.volume = volumeMusicValue;}
        if (audioBoss != null){audioBoss.volume = volumeMusicValue;}

    }
}
