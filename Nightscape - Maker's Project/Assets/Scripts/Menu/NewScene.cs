using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Video;

public class NewScene : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public VolumeMusic volumeMusic;
    public PauseMenu pauseMenu;
    public MusicZoneScript musicZoneScript;
    public GameObject musicObject;

    int JustDeleteSave;
    public GameObject canvasLoading;
    public GameObject loadingScreen;
    private Animator loading;
    private VideoPlayer videoPlayer;
    public GameObject videoObject;
    public GameObject blackScreen;
    //HUD
    public GameObject HUD;
    public GameObject[] shrinesHUD;
    void Awake()
    {
        playerCombat.EnableCombat(false);
        playerMovement.EnableMovement(false);
        pauseMenu.EnablePauseEchap(false);
        EnableShrinesHUD(false);
        musicObject.SetActive(false);

        loadingScreen.SetActive(false);
        blackScreen.SetActive(true);
        videoPlayer = videoObject.GetComponent<VideoPlayer>();
        Debug.Log("ENTERING START NEW SCENE");
        loading = loadingScreen.GetComponent<Animator>();
        JustDeleteSave = PlayerPrefs.GetInt("JustDeleteSave");
        HUD.SetActive(false);
        if (JustDeleteSave == 1) // In case the player starts a new game : plays cinematic.
        {
            Debug.Log("JUST DELETE SAVE = 1");
            //Play Loading screen & fondu transi
            StartCoroutine(displayCinematic());
        }
        else //In case the player loads a game save
        {
            StartCoroutine(displayLoadingScreen());

        }

    }

    public IEnumerator displayCinematic()
    {
        

        loadingScreen.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(1);

        blackScreen.SetActive(false);

        yield return new WaitForSecondsRealtime(51);
        
        canvasLoading.SetActive(true);
        loading.SetBool("ShowLoadingScreen", true);
        yield return new WaitForSecondsRealtime(1);

        videoObject.SetActive(false);

        yield return new WaitForSecondsRealtime(5);
        loading.SetBool("ShowLoadingScreen", false);
        canvasLoading.SetActive(false);
        HUD.SetActive(true);
        loadingScreen.SetActive(false);

        playerCombat.EnableCombat(true);
        playerMovement.EnableMovement(true);
        pauseMenu.EnablePauseEchap(true);
        EnableShrinesHUD(true);

        musicObject.SetActive(true);
        volumeMusic.ChangeVolumeMusic();

    }

    public IEnumerator displayLoadingScreen()
    {
        //volumeMusic.ChangeVolumeMusic();
        musicObject.SetActive(false);

        loadingScreen.SetActive(true);
        canvasLoading.SetActive(true);
        
        loading.SetBool("ShowLoadingScreen", true);

        yield return new WaitForSecondsRealtime(2);

        blackScreen.SetActive(false);

        yield return new WaitForSecondsRealtime(4);

        loading.SetBool("ShowLoadingScreen", false);
        canvasLoading.SetActive(false);
        HUD.SetActive(true);
        loadingScreen.SetActive(false);

        playerCombat.EnableCombat(true);
        playerMovement.EnableMovement(true);
        pauseMenu.EnablePauseEchap(true);
        EnableShrinesHUD(true);

        musicObject.SetActive(true);
        volumeMusic.ChangeVolumeMusic();
    }

    private void EnableShrinesHUD(bool boolean) 
    {
        
        for(int i=0; i<shrinesHUD.Length; i++)
        {
            shrinesHUD[i].SetActive(boolean);
        }
    }
}
