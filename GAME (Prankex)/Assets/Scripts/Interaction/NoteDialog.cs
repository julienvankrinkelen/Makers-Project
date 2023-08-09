using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class NoteDialog : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public float textSpeed;
    private int index;
    public PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;

    public GameObject box;

    //public CameraZoom Camera;

    public GameObject note;


    public bool DialogueStarted = false;

    //ATTENTION : ici, une "line" correspond à une "dialogue line", pas une ligne au sens 1 paragraphe = 30 lignes.
    //D'où l'initialisation de textComponent quand on change de "line". Une line peut contenir plusieurs lignes de texte.
    public string[] lines;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && DialogueStarted == true)
        {
            print("INTERACTING IN DIALOG TEST");
            print(index);
            //Si l'affichage de la ligne de dialogue est terminée : on passe à la suivante
            if (textComponent.text == lines[index])
            {
                playerCombat.InteractClickItemUsed();
                NextLine();
            }
            //Sinon on affiche le restant du text (pour les gens comme moi :))
            else
            {
                playerCombat.InteractClickItemUsed();
                StopAllCoroutines();
                textComponent.text = lines[index];

            }
        }
    }

    void Start()
    {
       
        box.SetActive(false);

    }

    public void StartDialog()
    {
        //On rend le joueur static, mais il faut aussi BLOQUER TOUTES LES ACTIONS DES KEYS : LE JOUEUR PEUT TOUJOURS TAPER AVEC CLIC, TOURNER SA VUE AVEC DROITE GAUCHE, DASH AVEC CONTROL ETC.
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        box.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
       // DialogueStarted = true;
        //Camera.ZoomActive = true;
        StartCoroutine(TypeLine());
    }
    //Ecrit une ligne entière (ligne à référencer soit dans le code, soit dans Unity... plus pratique dans le code à l'avenir bien sûr.
    public IEnumerator TypeLine()
    {
        print("TYPO LINO");
        //Display les caractères les uns après les autres à la vitesse textSpeed
        foreach (char c in lines[index].ToCharArray())
        {
            print("WRITING");
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
            //FIX : boolean ici pour créer décalage tempo avec 
            // appel à Interact.performed
            DialogueStarted = true;

        }
        print("FINISH WRITING");
        
       
    }
    void NextLine()
    {
        print("NEXTO LINO");
        //Tant qu'il reste des lignes dans la liste de lignes
        //Tant qu'il reste des lignes dans la liste de lignes
        if (index < lines.Length - 1)
        {
            index++;
            //On crée le textComponent initialisé à vide
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //Si il ne reste plus aucune ligne à display, on quitte et on rend les mouvements libres 

            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);
           // Camera.ZoomActive = false;
            DialogueStarted = false;
            note.SetActive(false);
           // StartCoroutine(canInteractTempo());


        }
    }
    // nécessaire pour pouvoir relancer le dialogue sans sortir de la zone de détection.
    //
    // Il faut remettre canInteract à true (il l'était que quand on sortait et on re rentrait de la zone). On pourrait mettre canInteract à TRUE
    // en même temps que les fonctions juste au-dessus, mais Problème encore :
    // Le problème vient du fait que quand on appuie sur la touche d'interaction pour enlever la dernière ligne du dialogue,
    // on remet context.performed à TRUE, et canInteract est alors aussi remis à TRUE si on le met juste au-dessus. Donc il faut le remettre à TRUE juste à après que 
    // context.performed soit remis à 0 (appeler une temporisation très courte suffit).
   /* private IEnumerator canInteractTempo()
    {
        yield return new WaitForSeconds(0.01f);
        box.SetActive(false);

    }
   */





}