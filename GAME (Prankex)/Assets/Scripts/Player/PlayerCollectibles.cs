using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollectibles : MonoBehaviour
{
    public TerrainState terrainState;

    private int numberOfDaruma;
    private int numberOfOmamori;
    private int numberOfExplosiveScroll;
    private int numberScrollsPicked;

    private bool hasCandle;
    private bool hasDash;
    private bool messageCanDisable = false;

    private int lanternLightenedNumber;
    private int numberOfNotes;

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

        

        numberOfDaruma = 0;
        numberOfOmamori =0;
        numberOfExplosiveScroll =0;
        numberScrollsPicked = 0;
        hasCandle = false;
        hasDash = false;
        lanternLightenedNumber = 0;
        numberOfNotes = 0;

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

    public void pickedDash()
    {
        hasDash = true;
        StartCoroutine(displayMessageItem("dash"));

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
        darumaPicked[darumaNumber] = true;

        if (numberOfDaruma == 0)
        {
            StartCoroutine(displayMessageItem("daruma"));
        }
        addDaruma();


    }
    // Uniquement appelée lors du ramassage d'un objet in-game.
    // Pas appelé lors du save donc aucun pb de sur-utilisation
    public void addDaruma()
    {
        numberOfDaruma++;
        //Stat buff atk
        if (numberOfDaruma % 3 == 0)
        {
            Debug.Log("Number of Daruma : " + numberOfDaruma + " -> adding 1 atkDamage to player");
            playerCombat.AddDamage(1);
        }
    }

    public void pickScroll(GameObject scroll) {
        string scrollName = scroll.name;
        int scrollNumber = parseCollectibleName(scrollName);
        Debug.Log("HAS PICKED SCROLL NB : " + scrollNumber);

        scrollPicked[scrollNumber] = true;
        if (numberScrollsPicked == 0)
        {
            StartCoroutine(displayMessageItem("scroll"));
        }
        addExplosiveScroll();


    }
    public void addExplosiveScroll()
    {
        numberOfExplosiveScroll++; // Dynamic number (+/-).
        numberScrollsPicked++; // Total number : can only add up.
    }


    // Get the number of the omamori picked to upload the omamori array
    public void pickOmamori(GameObject omamori)
    {
        string omamoriName = omamori.name;
        int omamoriNumber = parseCollectibleName(omamoriName);
        Debug.Log("HAS PICKED OMAMORI NB : " + omamoriNumber);
        omamoriPicked[omamoriNumber] = true;

        if (numberOfOmamori == 0){
            StartCoroutine(displayMessageItem("omamori"));
        }
        addOmamori();


    }

    // Uniquement appelée lors du ramassage d'un objet in-game.
    // Pas appelé lors du save donc aucun pb de sur-utilisation
    public void addOmamori()
    {
        numberOfOmamori++;
        //Stat buff hp tous les 3 omamori
        if(numberOfOmamori % 3 == 0)
        {
            Debug.Log("Number of Omamori : " + numberOfOmamori + " -> adding 1 hp");
            playerCombat.AddLife(1);
        }
    }

    public void pickNote(GameObject note)
    {
        string noteName = note.name;
        int noteNumber = parseCollectibleName(noteName);
        Debug.Log("HAS PICKED NOTE NB : " + noteNumber);
        notePicked[noteNumber] = true;
        numberOfNotes++; //Update number of notes
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
        numberOfExplosiveScroll = scrollNumber;
    }
    public void setNumberScrollsPicked(int scrollNumberPicked)
    {
        numberScrollsPicked = scrollNumberPicked;
    }
 
    public void setNumberOmamori(int numberOmamori)
    {
        numberOfOmamori = numberOmamori;
    }

    public void setNumberDaruma(int numberDaruma)
    {
        numberOfDaruma = numberDaruma;
    }

    //Explosive scroll is usable.
    public void removeExplosiveScroll()
    {
        numberOfExplosiveScroll--;
    }

    public void setNumberOfNotes(int numberNotes)
    {
        numberOfNotes = numberNotes;
    }

    public int getDarumaNumber()
    {
        return numberOfDaruma;
    }
    public int getOmamoriNumber()
    {
        return numberOfOmamori;
    }
    public int getExplosiveScrollNumber()
    {
        return numberOfExplosiveScroll;
    }
    public int getExplosiveScrollTotalNumber()
    {
        return numberScrollsPicked;
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
    public int getNumberOfNotes()
    {
        return numberOfNotes;
    }

    // Convert the last two char of a string into integer
    private int parseCollectibleName(string collectible) {
        return int.Parse(collectible.Substring(collectible.Length - 2));

    }
}
