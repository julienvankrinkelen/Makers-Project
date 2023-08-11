using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject lifeBar;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    
    public PlayerCollectibles playerCollectibles;

    public GameObject textNote;
    public GameObject textScroll;
    public GameObject textDash;
    public GameObject textCandle;

    [SerializeField] private AudioSource clickSoundEffect;
    [SerializeField] private AudioSource backSoundEffect;

    public static bool isPaused = false;
    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Pause.performed += Pause;

    }
        // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

    }

    public void PauseGame()
    {
        lifeBar.SetActive(false);
        pauseMenu.SetActive(true);
        //Stop toutes les animations, freeze le temps
        Time.timeScale = 0f;
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        isPaused = true;

        setTextItems();
    }

    public void ResumeGame()
    {
        lifeBar.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerMovement.EnableMovement(true);
        playerCombat.EnableCombat(true);
        isPaused = false;
    }

    public void setTextItems()
    {
        textCandle.SetActive(playerCollectibles.checkHasCandle());
        textDash.SetActive(playerCollectibles.checkHasDash());
        textScroll.SetActive(playerCollectibles.getExplosiveScrollNumber()>0);
        textNote.SetActive(playerCollectibles.getNumberOfNotes()>0);

    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SoundButton()
    {
        clickSoundEffect.Play();
    }
    public void SoundBack()
    {
        backSoundEffect.Play();
    }
}