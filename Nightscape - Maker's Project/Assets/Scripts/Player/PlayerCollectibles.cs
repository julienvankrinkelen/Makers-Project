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

    [SerializeField] private GameObject[] lanterns;

    [SerializeField] private GameObject messageDash;
    [SerializeField] private GameObject messageDashCanPress;

    [SerializeField] private GameObject messageCandle;
    [SerializeField] private GameObject messageCandleCanPress;

    [SerializeField] private GameObject messageOmamori;
    [SerializeField] private GameObject messageOmamoriCanPress;

    [SerializeField] private GameObject messageDaruma;
    [SerializeField] private GameObject messageDarumaCanPress;

    [SerializeField] private GameObject messageScroll;
    [SerializeField] private GameObject messageScrollCanPress;

    // variables used to select item message to display in switch system 
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject messageCanPress;


    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private AudioSource DashObtainedSoundEffect;
    [SerializeField] private AudioSource CandleObtainedSoundEffect;
    [SerializeField] private AudioSource ScrollObtainedSoundEffect;
    [SerializeField] private AudioSource NoteObtainedSoundEffect;
    [SerializeField] private AudioSource BonusObtainedSoundEffect;


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
        notePicked = new bool[6];
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
          //  Debug.Log("Disable 2e Message Item");
            playerCombat.InteractClickItemUsed();

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
      //  Debug.Log("Display 1er Message Item");
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        Time.timeScale = 0f;

        // On empêche le joueur de skip sans le vouloir le message
        yield return new WaitForSecondsRealtime(3);
        messageCanDisable = true;
        message.SetActive(false);
     //   Debug.Log("Disable 1er Message Item");

        messageCanPress.SetActive(true);
     //   Debug.Log("Display 2e Message Item");

    }

    public void pickedDash()
    {
        hasDash = true;
        DashObtainedSoundEffect.Play();
        StartCoroutine(displayMessageItem("dash"));

    }

    public void setDash(bool dash)
    {
        hasDash = dash;
    }

    public void pickedCandle()
    {
        hasCandle = true;
        CandleObtainedSoundEffect.Play();
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
      //  Debug.Log("HAS PICKED DARUMA NB : " + darumaNumber);
        darumaPicked[darumaNumber] = true;

        if (numberOfDaruma == 0)
        {
            StartCoroutine(displayMessageItem("daruma"));
        }
        addDaruma();
        BonusObtainedSoundEffect.Play();


    }
    // Uniquement appelée lors du ramassage d'un objet in-game.
    // Pas appelé lors du save donc aucun pb de sur-utilisation
    public void addDaruma()
    {
        numberOfDaruma++;
        //Stat buff atk
        if (numberOfDaruma % 3 == 0)
        {
          //  Debug.Log("Number of Daruma : " + numberOfDaruma + " -> adding 1 atkDamage to player");
            playerCombat.AddDamage(1);
            playerCombat.SoundLevelUp();
        }
    }

    public void pickScroll(GameObject scroll) {
        string scrollName = scroll.name;
        int scrollNumber = parseCollectibleName(scrollName);
      //  Debug.Log("HAS PICKED SCROLL NB : " + scrollNumber);

        scrollPicked[scrollNumber] = true;
        if (numberScrollsPicked == 0)
        {
            StartCoroutine(displayMessageItem("scroll"));
        }
        addExplosiveScroll();
        ScrollObtainedSoundEffect.Play();

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
      //  Debug.Log("HAS PICKED OMAMORI NB : " + omamoriNumber);
        omamoriPicked[omamoriNumber] = true;

        if (numberOfOmamori == 0){
            StartCoroutine(displayMessageItem("omamori"));
        }
        addOmamori();
        BonusObtainedSoundEffect.Play();

    }

    // Uniquement appelée lors du ramassage d'un objet in-game.
    // Pas appelé lors du save donc aucun pb de sur-utilisation
    public void addOmamori()
    {
        numberOfOmamori++;
        //Stat buff hp tous les 3 omamori
        if(numberOfOmamori % 3 == 0)
        {
          //  Debug.Log("Number of Omamori : " + numberOfOmamori + " -> adding 1 hp");
            playerCombat.AddLife(1);
            playerCombat.SoundLevelUp();
        }
    }

    public void pickNote(GameObject note)
    {
        string noteName = note.name;
        int noteNumber = parseCollectibleName(noteName);
      //  Debug.Log("HAS PICKED NOTE NB : " + noteNumber);
        notePicked[noteNumber] = true;
        numberOfNotes++; //Update number of notes
        NoteObtainedSoundEffect.Play();
    }

    public void lightenLantern(GameObject lantern, bool boolean)
    {
        string lanternName = lantern.name;
        int lanternNumber = parseCollectibleName(lanternName);
      //  Debug.Log("HAS LIGHTENED LANTERN NB : " + lanternNumber);
        setLanternLightened(lanternNumber, boolean);
        lanternLightened[lanternNumber] = boolean;
        lanternLightenedNumber++;

    }

    public void setLanternLightened(int i, bool boolean)
    {
        Animator anim = lanterns[i].GetComponent<Animator>();
        if (boolean)
        {
            terrainState.lightDoor(i, true);
            anim.SetBool("Lantern_ON", true);
        }
        else
        {
            terrainState.lightDoor(i, false);
            Debug.Log("Setting anim bool to false");
            anim.SetBool("Lantern_ON", false);
        }
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
    public void setLanternLightenedNumber(int nb)
    {
        lanternLightenedNumber = nb;
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
