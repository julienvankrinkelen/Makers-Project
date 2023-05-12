using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollectibles : MonoBehaviour
{
    private int darumaNumber;
    private int omamoriNumber;
    private int explosiveScrollNumber;
    private bool hasCandle;
    private int artefactNumber;
    private bool hasDash;
    private bool messageDashIsActive = false;

    public GameObject messageDash;
    public PlayerInput playerInput;
    public PlayerInputActions playerInputActions;
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact;
    }

    public void Start()
    {
        explosiveScrollNumber = 0;
        darumaNumber =0;
        omamoriNumber =0;
        artefactNumber = 0;
        hasCandle = false;
        hasDash = false;

    }


    // Ino order to "unfreeze" the screen when picking dash. Press F, the "interact" button.
    public void Interact(InputAction.CallbackContext context)
    {
        if (messageDashIsActive)
        {
            messageDash.SetActive(false);

            Time.timeScale = 1f;
            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);

            messageDashIsActive = false;
        }
    }

    public void pickedDash()
    {
        hasDash = true;
        messageDash.SetActive(true);

        Time.timeScale = 0f;
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);

        messageDashIsActive = true;
    }

    public void setDash(bool dash)
    {
        hasDash = dash;
    }
    public void pickedCandle()
    {
        hasCandle = true;
    }
    public void setCandle(bool candle)
    {
        hasCandle = candle;
    }
    public void addExplosiveScroll()
    {
        explosiveScrollNumber++;
    }

    public void addArtefact()
    {
        artefactNumber++;
    }
    public void addDaruma()
    {
        darumaNumber++;
        //Stat buff atk
        playerCombat.AddDamage(0.1f);
    }
    public void addOmamori()
    {
        omamoriNumber++;
        //Stat buff hp
        playerCombat.AddLife(1);
    }

    //Explosive scroll is usable.
    public void removeExplosiveScroll()
    {
        explosiveScrollNumber--;
    }

    public int getDarumaNumber()
    {
        return darumaNumber;
    }
    public int getOmamoriNumber()
    {
        return omamoriNumber;
    }
    public int getArtefactNumber()
    {
        return artefactNumber;
    }
    public int getExplosiveScrollNumber()
    {
        return explosiveScrollNumber;
    }
    public bool checkHasCandle()
    {
        return hasCandle;
    }
    public bool checkHasDash()
    {
        return hasDash;
    }
}
