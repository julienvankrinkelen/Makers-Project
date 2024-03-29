using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    public float textSpeed;
    private int index;
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject box;

    public bool DialogueStarted = false;

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
            // print("INTERACTING IN NPC DIALOG");
            //Si l'affichage de la ligne de dialogue est termin�e : on passe � la suivante
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
        StartCoroutine(TypeLine());
    }
    //Ecrit une ligne enti�re (ligne � r�f�rencer soit dans le code, soit dans Unity... plus pratique dans le code � l'avenir bien s�r.
    public IEnumerator TypeLine()
    {
       // print("TYPO LINO");
        //Display les caract�res les uns apr�s les autres � la vitesse textSpeed
        foreach (char c in lines[index].ToCharArray())
        {
          //  print("WRITING");
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
            DialogueStarted = true;

        }
       // print("FINISH WRITING");
        
       
    }
    void NextLine()
    {
       // print("NEXTO LINO");
        //Tant qu'il reste des lignes dans la liste de lignes
        if (index < lines.Length - 1)
        {
            index++;
            //On cr�e le textComponent initialis� � vide
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //Si il ne reste plus aucune ligne � display, on quitte et on rend les mouvements libres 
            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);
            DialogueStarted = false;
            StartCoroutine(canInteractTempo());


        }
    }
    // n�cessaire pour pouvoir relancer le dialogue sans sortir de la zone de d�tection.
    //
    // Il faut remettre canInteract � true (il l'�tait que quand on sortait et on re rentrait de la zone). On pourrait mettre canInteract � TRUE
    // en m�me temps que les fonctions juste au-dessus, mais Probl�me encore :
    // Le probl�me vient du fait que quand on appuie sur la touche d'interaction pour enlever la derni�re ligne du dialogue,
    // on remet context.performed � TRUE, et canInteract est alors aussi remis � TRUE si on le met juste au-dessus. Donc il faut le remettre � TRUE juste � apr�s que 
    // context.performed soit remis � 0 (appeler une temporisation tr�s courte suffit).
    private IEnumerator canInteractTempo()
    {
        yield return new WaitForSeconds(0.01f);
        box.SetActive(false);

    }
   





}