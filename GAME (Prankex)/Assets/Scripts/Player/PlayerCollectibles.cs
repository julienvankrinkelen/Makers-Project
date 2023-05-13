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
    private bool hasDash;
    private bool messageDashIsActive = false;

    public bool[] omamoriPicked;
    public bool[] darumaPicked;
    public bool[] scrollPicked;


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
        
        omamoriPicked = new bool[4];
        darumaPicked = new bool[4];
        scrollPicked = new bool[1];

        

        explosiveScrollNumber = 0;
        darumaNumber =0;
        omamoriNumber =0;
        hasCandle = false;
        hasDash = false;

        fillArray(omamoriPicked);
        fillArray(darumaPicked);
        fillArray(scrollPicked);

    }

    private void fillArray(bool[] tab)
    {
        for(int i=0; i<tab.Length; i++)
        {
            tab[i] = false;
        }
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


    public void pickDaruma(GameObject daruma)
    {
        string darumaName = daruma.name;
        int darumaNumber = (int)char.GetNumericValue(darumaName[6]);
        Debug.Log("HAS PICKED DARUMA NB : " + darumaNumber);
        addDaruma();

        darumaPicked[darumaNumber] = true;
    }

    public void addDaruma()
    {
        darumaNumber++;
        //Stat buff atk
        playerCombat.AddDamage(0.1f);
    }

    public void pickScroll(GameObject scroll)
    {
        string scrollName = scroll.name;
        int scrollNumber = (int)char.GetNumericValue(scrollName[6]);
        Debug.Log("HAS PICKED SCROLL NB : " + scrollNumber);
        addExplosiveScroll();

        scrollPicked[scrollNumber] = true;
    }
    public void addExplosiveScroll()
    {
        explosiveScrollNumber++;
    }


    // Get the number of the omamori picked to upload the omamori array
    public void pickOmamori(GameObject omamori)
    {
        string omamoriName = omamori.name;
        int omamoriNumber = (int) char.GetNumericValue(omamoriName[7]);
        Debug.Log("HAS PICKED OMAMORI NB : " + omamoriNumber);
        addOmamori();

        omamoriPicked[omamoriNumber] = true;
    }
    public void addOmamori()
    {
        omamoriNumber++;
        //Stat buff hp
        playerCombat.AddLife(1);
    }


    public void setNumberExplosiveScroll(int scrollNumber)
    {
        explosiveScrollNumber = scrollNumber;
    }

 
    public void setNumberOmamori(int numberOmamori)
    {
        omamoriNumber = numberOmamori;
    }

    public void setNumberDaruma(int numberDaruma)
    {
        darumaNumber = numberDaruma;
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
