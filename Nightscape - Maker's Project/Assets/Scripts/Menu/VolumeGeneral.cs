using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGeneral : MonoBehaviour
{

    [Header("Player sfx")]
    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource itemUsedSoundEffect;
    [SerializeField] private AudioSource interactClickSoundEffect;
    [SerializeField] private AudioSource healStatueSoundEffect;
    [SerializeField] private AudioSource wallDestroySoundEffect;
    [SerializeField] private AudioSource bushDestroySoundEffect;
    [SerializeField] private AudioSource lanternLightenSoundEffect;
    [SerializeField] private AudioSource levelUpSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    [SerializeField] private AudioSource runSoundEffect;

    [SerializeField] private AudioSource scrollObtainedSoundEffect;
    [SerializeField] private AudioSource noteObtainedSoundEffect;
    [SerializeField] private AudioSource dashObtainedSoundEffect;
    [SerializeField] private AudioSource candleObtainedSoundEffect;
    [SerializeField] private AudioSource bonusPickupSoundEffect;
    [SerializeField] private AudioSource autelSoundEffect;

    [Header("UI sfx")]
    [SerializeField] private AudioSource menuClickSoundEffect;
    [SerializeField] private AudioSource menuBackSoundEffect;

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

    [SerializeField] private AudioSource tanukiDieSoundEffect00;
    [SerializeField] private AudioSource tanukiDieSoundEffect01;
    [SerializeField] private AudioSource tanukiDieSoundEffect02;
    [SerializeField] private AudioSource tanukiDieSoundEffect03;
    [SerializeField] private AudioSource tanukiDieSoundEffect04;
    [SerializeField] private AudioSource tanukiDieSoundEffect05;
    [SerializeField] private AudioSource tanukiDieSoundEffect06;
    [SerializeField] private AudioSource tanukiDieSoundEffect07;
    [SerializeField] private AudioSource tanukiDieSoundEffect08;
    [SerializeField] private AudioSource tanukiDieSoundEffect09;
    [SerializeField] private AudioSource tanukiDieSoundEffect10;
    [SerializeField] private AudioSource tanukiDieSoundEffect11;

    [Header("Boss sfx")]
    [SerializeField] private AudioSource bossTransiSoundEffect;
    [SerializeField] private AudioSource bossThunderSoundEffect;
    [SerializeField] private AudioSource bossIncantSoundEffect;
    [SerializeField] private AudioSource bossDieSoundEffect;
    [SerializeField] private AudioSource bossAttackSoundEffect;


    // Update is called once per frame
    void Update()
    {
        float volumeGeneralValue = PlayerPrefs.GetFloat("VolumeGeneralValue");
        hurtSoundEffect.volume = volumeGeneralValue;
        dieSoundEffect.volume = volumeGeneralValue;
        attackSoundEffect.volume = volumeGeneralValue;
        itemUsedSoundEffect.volume = volumeGeneralValue;
        interactClickSoundEffect.volume = volumeGeneralValue;
        healStatueSoundEffect.volume = volumeGeneralValue;
        wallDestroySoundEffect.volume = volumeGeneralValue;
        bushDestroySoundEffect.volume = volumeGeneralValue;
        lanternLightenSoundEffect.volume = volumeGeneralValue;
        levelUpSoundEffect.volume = volumeGeneralValue;
        dashSoundEffect.volume = volumeGeneralValue;
        runSoundEffect.volume = volumeGeneralValue;

        scrollObtainedSoundEffect.volume = volumeGeneralValue;
        noteObtainedSoundEffect.volume = volumeGeneralValue;
        dashObtainedSoundEffect.volume = volumeGeneralValue;
        candleObtainedSoundEffect.volume = volumeGeneralValue;
        bonusPickupSoundEffect.volume = volumeGeneralValue;
        autelSoundEffect.volume = volumeGeneralValue;

        menuClickSoundEffect.volume = volumeGeneralValue;
        menuBackSoundEffect.volume = volumeGeneralValue;

        onibiHurtSoundEffect00.volume = volumeGeneralValue;
        dieSoundEffect00.volume = volumeGeneralValue;

        onibiHurtSoundEffect01.volume = volumeGeneralValue;
        dieSoundEffect01.volume = volumeGeneralValue;

        onibiHurtSoundEffect02.volume = volumeGeneralValue;
        dieSoundEffect02.volume = volumeGeneralValue;

        onibiHurtSoundEffect03.volume = volumeGeneralValue;
        dieSoundEffect03.volume = volumeGeneralValue;

        onibiHurtSoundEffect04.volume = volumeGeneralValue;
        dieSoundEffect04.volume = volumeGeneralValue;

        onibiHurtSoundEffect05.volume = volumeGeneralValue;
        dieSoundEffect05.volume = volumeGeneralValue;

        onibiHurtSoundEffect06.volume = volumeGeneralValue;
        dieSoundEffect06.volume = volumeGeneralValue;


        karakasaHurtSoundEffect00.volume = volumeGeneralValue;
        karakasaDieSoundEffect00.volume = volumeGeneralValue;
        karakasaAttackSoundEffect00.volume = volumeGeneralValue;

        karakasaHurtSoundEffect01.volume = volumeGeneralValue;
        karakasaDieSoundEffect01.volume = volumeGeneralValue;
        karakasaAttackSoundEffect01.volume = volumeGeneralValue;

        karakasaHurtSoundEffect02.volume = volumeGeneralValue;
        karakasaDieSoundEffect02.volume = volumeGeneralValue;
        karakasaAttackSoundEffect02.volume = volumeGeneralValue;

        karakasaHurtSoundEffect03.volume = volumeGeneralValue;
        karakasaDieSoundEffect03.volume = volumeGeneralValue;
        karakasaAttackSoundEffect03.volume = volumeGeneralValue;

        karakasaHurtSoundEffect04.volume = volumeGeneralValue;
        karakasaDieSoundEffect04.volume = volumeGeneralValue;
        karakasaAttackSoundEffect04.volume = volumeGeneralValue;

        karakasaHurtSoundEffect05.volume = volumeGeneralValue;
        karakasaDieSoundEffect05.volume = volumeGeneralValue;
        karakasaAttackSoundEffect05.volume = volumeGeneralValue;

        karakasaHurtSoundEffect06.volume = volumeGeneralValue;
        karakasaDieSoundEffect06.volume = volumeGeneralValue;
        karakasaAttackSoundEffect06.volume = volumeGeneralValue;

        karakasaHurtSoundEffect07.volume = volumeGeneralValue;
        karakasaDieSoundEffect07.volume = volumeGeneralValue;
        karakasaAttackSoundEffect07.volume = volumeGeneralValue;

        karakasaHurtSoundEffect08.volume = volumeGeneralValue;
        karakasaDieSoundEffect08.volume = volumeGeneralValue;
        karakasaAttackSoundEffect08.volume = volumeGeneralValue;

        karakasaHurtSoundEffect09.volume = volumeGeneralValue;
        karakasaDieSoundEffect09.volume = volumeGeneralValue;
        karakasaAttackSoundEffect09.volume = volumeGeneralValue;

        karakasaHurtSoundEffect10.volume = volumeGeneralValue;
        karakasaDieSoundEffect10.volume = volumeGeneralValue;
        karakasaAttackSoundEffect10.volume = volumeGeneralValue;

        bossTransiSoundEffect.volume = volumeGeneralValue;
        bossThunderSoundEffect.volume = volumeGeneralValue;
        bossIncantSoundEffect.volume = volumeGeneralValue;
        bossDieSoundEffect.volume = volumeGeneralValue;
        bossAttackSoundEffect.volume = volumeGeneralValue;


        tanukiDieSoundEffect00.volume = volumeGeneralValue;
        tanukiDieSoundEffect01.volume = volumeGeneralValue;
        tanukiDieSoundEffect02.volume = volumeGeneralValue;
        tanukiDieSoundEffect03.volume = volumeGeneralValue;
        tanukiDieSoundEffect04.volume = volumeGeneralValue;
        tanukiDieSoundEffect05.volume = volumeGeneralValue;
        tanukiDieSoundEffect06.volume = volumeGeneralValue;
        tanukiDieSoundEffect07.volume = volumeGeneralValue;
        tanukiDieSoundEffect08.volume = volumeGeneralValue;
        tanukiDieSoundEffect09.volume = volumeGeneralValue;
        tanukiDieSoundEffect10.volume = volumeGeneralValue;
        tanukiDieSoundEffect11.volume = volumeGeneralValue;
    }
}
