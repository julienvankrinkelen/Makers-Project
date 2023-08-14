using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public VolumeMusic volumeMusic;

    private VideoPlayer videoPlayer;


    public void StartCredits()
    {
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        //FadeOut

        //Video playing


        videoPlayer.Play();


        //reset restrictions to avoid being blocked for next game ?
        playerMovement.EnableMovement(true);
        playerCombat.EnableCombat(true);
        //Go back main menu
    }
}
