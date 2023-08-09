using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGeneral : MonoBehaviour
{

    private AudioSource audioSource;
    // Start is called before the first frame update
    [Header("Player sfx")]
    [SerializeField] private AudioSource runSoundEffect;
    [SerializeField] private AudioSource itemUsedSoundEffect;
    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;

    [Header("UI sfx")]
    [SerializeField] private AudioSource menuClickSoundEffect;
    [SerializeField] private AudioSource interactSoundEffect;
    [SerializeField] private AudioSource menuBackSoundEffect;

    [Header("Props sfx")]
    [SerializeField] private AudioSource murDestruSoundEffect;
    [SerializeField] private AudioSource lanternLightenSoundEffect;
    [SerializeField] private AudioSource scrollObtainedSoundEffect;
    [SerializeField] private AudioSource noteObtainedSoundEffect;
    [SerializeField] private AudioSource légendaireObtainedSoundEffect;
    [SerializeField] private AudioSource buissonDestruSoundEffect;
    [SerializeField] private AudioSource bonusPickupDestruSoundEffect;
    [SerializeField] private AudioSource bonusLevelupDestruSoundEffect;
    [SerializeField] private AudioSource autelDestruSoundEffect;
    [SerializeField] private AudioSource autelHealDestruSoundEffect;

    [Header("Onibi sfx")]
    [SerializeField] private AudioSource onibiHurtSoundEffect00;
    [SerializeField] private AudioSource dieSoundEffect00;

    [SerializeField] private AudioSource onibiHurtSoundEffect01;
    [SerializeField] private AudioSource dieSoundEffect01;

    [SerializeField] private AudioSource onibiHurtSoundEffect02;
    [SerializeField] private AudioSource dieSoundEffect02;

    [SerializeField] private AudioSource onibiHurtSoundEffect03;
    [SerializeField] private AudioSource dieSoundEffect03;

    [SerializeField] private AudioSource onibiHurtSoundEffect04;
    [SerializeField] private AudioSource dieSoundEffect04;

    [SerializeField] private AudioSource onibiHurtSoundEffect05;
    [SerializeField] private AudioSource dieSoundEffect05;

    [SerializeField] private AudioSource onibiHurtSoundEffect06;
    [SerializeField] private AudioSource dieSoundEffect06;


    [Header("Karakasa sfx")]
    [SerializeField] private AudioSource karakasaHurtSoundEffect00;
    [SerializeField] private AudioSource karakasaDieSoundEffect00;
    [SerializeField] private AudioSource karakasaAttackSoundEffect00;

    [SerializeField] private AudioSource karakasaHurtSoundEffect01;
    [SerializeField] private AudioSource karakasaDieSoundEffect01;
    [SerializeField] private AudioSource karakasaAttackSoundEffect01;

    [SerializeField] private AudioSource karakasaHurtSoundEffect02;
    [SerializeField] private AudioSource karakasaDieSoundEffect02;
    [SerializeField] private AudioSource karakasaAttackSoundEffect02;

    [SerializeField] private AudioSource karakasaHurtSoundEffect03;
    [SerializeField] private AudioSource karakasaDieSoundEffect03;
    [SerializeField] private AudioSource karakasaAttackSoundEffect03;

    [SerializeField] private AudioSource karakasaHurtSoundEffect04;
    [SerializeField] private AudioSource karakasaDieSoundEffect04;
    [SerializeField] private AudioSource karakasaAttackSoundEffect04;

    [SerializeField] private AudioSource karakasaHurtSoundEffect05;
    [SerializeField] private AudioSource karakasaDieSoundEffect05;
    [SerializeField] private AudioSource karakasaAttackSoundEffect05;

    [SerializeField] private AudioSource karakasaHurtSoundEffect06;
    [SerializeField] private AudioSource karakasaDieSoundEffect06;
    [SerializeField] private AudioSource karakasaAttackSoundEffect06;

    [SerializeField] private AudioSource karakasaHurtSoundEffect07;
    [SerializeField] private AudioSource karakasaDieSoundEffect07;
    [SerializeField] private AudioSource karakasaAttackSoundEffect07;

    [SerializeField] private AudioSource karakasaHurtSoundEffect08;
    [SerializeField] private AudioSource karakasaDieSoundEffect08;
    [SerializeField] private AudioSource karakasaAttackSoundEffect08;

    [SerializeField] private AudioSource karakasaHurtSoundEffect09;
    [SerializeField] private AudioSource karakasaDieSoundEffect09;
    [SerializeField] private AudioSource karakasaAttackSoundEffect09;

    [SerializeField] private AudioSource karakasaHurtSoundEffect10;
    [SerializeField] private AudioSource karakasaDieSoundEffect10;
    [SerializeField] private AudioSource karakasaAttackSoundEffect10;

    [Header("Boss sfx")]
    [SerializeField] private AudioSource bossTransiSoundEffect;
    [SerializeField] private AudioSource bossThunderSoundEffect;
    [SerializeField] private AudioSource bossIncantSoundEffect;
    [SerializeField] private AudioSource bossDieSoundEffect;
    [SerializeField] private AudioSource bossAttackSoundEffect;





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
