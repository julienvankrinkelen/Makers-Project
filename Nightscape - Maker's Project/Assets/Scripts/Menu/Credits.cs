using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private VolumeMusic volumeMusic;
    [SerializeField] private PauseMenu pauseMenu;


    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject canvasLoading;
    [SerializeField] private GameObject loadingScreen;
    private Animator fadeOut;
    [SerializeField] private GameObject blackScreen;

    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        canvasLoading.SetActive(false);
        loadingScreen.SetActive(false);
        blackScreen.SetActive(false);
        fadeOut = loadingScreen.GetComponent<Animator>();

    }

    public void StartCredits()
    {
        volumeMusic.CancelMusicVolume();

        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);

        canvasLoading.SetActive(true);
        loadingScreen.SetActive(true);

        StartCoroutine(CreditsPlaying());

    }

    public IEnumerator CreditsPlaying()
    {
        //FadeOut

        // yield return new WaitForSecondsRealtime(5);

        fadeOut.SetTrigger("FadeOut");

        yield return new WaitForSecondsRealtime(2);

        blackScreen.SetActive(true);

        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.5f);
        HUD.SetActive(false);
        canvasLoading.SetActive(false);
        loadingScreen.SetActive(false);



        //Duration of the video
        blackScreen.SetActive(false);

        yield return new WaitForSecondsRealtime(29);

        //reset restrictions to avoid being blocked for next game ?
        playerMovement.EnableMovement(true);
        playerCombat.EnableCombat(true);
        pauseMenu.EnablePauseEchap(true);

        //Go back main menu
        PlayerPrefs.SetInt("Save Exists", 0);
        SceneManager.LoadScene("Start Menu");

    }

}
