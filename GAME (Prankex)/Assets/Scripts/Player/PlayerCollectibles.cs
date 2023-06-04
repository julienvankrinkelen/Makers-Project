using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollectibles : MonoBehaviour
{
    public TerrainState terrainState;

    private int darumaNumber;
    private int omamoriNumber;
    private int explosiveScrollNumber;
    private bool hasCandle;
    private bool hasDash;
    private bool messageCanDisable = false;
    private int lanternLightenedNumber;

    private bool firstPickDaruma;
    private bool firstPickOmamori;
    private bool firstPickScroll;

    public bool[] omamoriPicked;
    public bool[] darumaPicked;
    public bool[] scrollPicked;
    public bool[] notePicked;
    public bool[] lanternLightened;

    public GameObject[] lanterns;

    public GameObject messageDash;
    public GameObject messageDashCanPress;

    public GameObject messageCandle;
    public GameObject messageCandleCanPress;

    public GameObject messageOmamori;
    public GameObject messageOmamoriCanPress;

    public GameObject messageDaruma;
    public GameObject messageDarumaCanPress;

    public GameObject messageScroll;
    public GameObject messageScrollCanPress;

    // variables used to select item message to display in switch system 
    public GameObject message;
    public GameObject messageCanPress;


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
        
        omamoriPicked = new bool[15];
        darumaPicked = new bool[15];
        scrollPicked = new bool[12];
        notePicked = new bool[5];
        lanternLightened = new bool[3];

        

        explosiveScrollNumber = 0;
        darumaNumber =0;
        omamoriNumber =0;
        hasCandle = false;
        hasDash = false;
        lanternLightenedNumber = 0;

        fillArray(notePicked);
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


    // In order to "unfreeze" the screen when picking dash. Press F, the "interact" button.
    public void Interact(InputAction.CallbackContext context)
    {


        if (messageCanDisable)
        {
            messageCanPress.SetActive(false);
            Debug.Log("Disable 2e Message Item");

            Time.timeScale = 1f;
            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);

            messageCanDisable = false;
        }
    }

    

    public void pickedDash()
    {
        hasDash = true;
        StartCoroutine(displayMessageItem("dash"));
    
    }
    private IEnumerator displayMessageItem(string item)
    {
       
        switch (item){
            case "dash":
                this.message = messageDash;
                this.messageCanPress = messageDashCanPress;
                break;
            case "candle":
                this.message = messageCandle;
                this.messageCanPress = messageCandleCanPress;
                break;
            case "omamori":
                this.message = messageOmamori;
                this.messageCanPress = messageOmamoriCanPress;
                break;
            case "daruma":
                this.message = messageDaruma;
                this.messageCanPress = messageDarumaCanPress;
                break;
            case "scroll":
                this.message = messageScroll;
                this.messageCanPress = messageScrollCanPress;
                break;
        }
        
        message.SetActive(true);
        Debug.Log("Display 1er Message Item");
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        Time.timeScale = 0f;

        // On empêche le joueur de skip sans le vouloir le message
        yield return new WaitForSecondsRealtime(3);
        messageCanDisable = true;
        message.SetActive(false);
        Debug.Log("Disable 1er Message Item");

        messageCanPress.SetActive(true);
        Debug.Log("Display 2e Message Item");

    }
    public void setDash(bool dash)
    {
        hasDash = dash;
    }

    public void pickedCandle()
    {
        hasCandle = true;

        StartCoroutine(displayMessageItem("candle"));

    }
    public void setCandle(bool candle)
    {
        hasCandle = candle;
    }


    public void pickDaruma(GameObject daruma)
    {
        string darumaName = daruma.name;
        int darumaNumber = parseCollectibleName(darumaName);
        Debug.Log("HAS PICKED DARUMA NB : " + darumaNumber);
        addDaruma();
        if (!firstPickDaruma)
        {
            firstPickDaruma = true;
        }

        darumaPicked[darumaNumber] = true;

        StartCoroutine(displayMessageItem("daruma"));
        

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
        int scrollNumber = parseCollectibleName(scrollName);
        Debug.Log("HAS PICKED SCROLL NB : " + scrollNumber);
        addExplosiveScroll();
        if (!firstPickScroll)
        {
            firstPickScroll = true;
        }


        scrollPicked[scrollNumber] = true;

        StartCoroutine(displayMessageItem("scroll"));

    }
    public void addExplosiveScroll()
    {
        explosiveScrollNumber++;
    }


    // Get the number of the omamori picked to upload the omamori array
    public void pickOmamori(GameObject omamori)
    {
        string omamoriName = omamori.name;
        int omamoriNumber = parseCollectibleName(omamoriName);
        Debug.Log("HAS PICKED OMAMORI NB : " + omamoriNumber);
        addOmamori();

        omamoriPicked[omamoriNumber] = true;

        if (!firstPickOmamori)
        {
            firstPickOmamori = true;
        }

        StartCoroutine(displayMessageItem("omamori"));

    }
    public void addOmamori()
    {
        omamoriNumber++;
        //Stat buff hp
        playerCombat.AddLife(1);
    }

    public void pickNote(GameObject note)
    {
        string noteName = note.name;
        int noteNumber = parseCollectibleName(noteName);
        Debug.Log("HAS PICKED NOTE NB : " + noteNumber);
        notePicked[noteNumber] = true;

    }

    public void lightenLantern(GameObject lantern, bool boolean)
    {
        string lanternName = lantern.name;
        int lanternNumber = parseCollectibleName(lanternName);
        Debug.Log("HAS LIGHTENED LANTERN NB : " + lanternNumber);
        setLanternLightened(lanternNumber, boolean);
        lanternLightened[lanternNumber] = true;
        lanternLightenedNumber++;

    }

    public void setLanternLightened(int i, bool boolean)
    {
        terrainState.lightDoor(i);
        Animator anim = lanterns[i].GetComponent<Animator>();
        anim.SetBool("Lantern_ON", boolean);
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
    public int getLanternLightenedNumber() // Return the total lightened lantern number. Not correlated to the lantern number (name / id)
    {
        return lanternLightenedNumber;
    }

    public bool getFirstPickDaruma()
    {
        return firstPickDaruma;
    }
    public bool getFirstPickOmamori()
    {
        return firstPickOmamori;
    }

    public bool getFirstPickScroll()
    {
        return firstPickScroll;
    }


    // Convert the last two char of a string into integer
    private int parseCollectibleName(string collectible) {
        return int.Parse(collectible.Substring(collectible.Length - 2));

    }
}
