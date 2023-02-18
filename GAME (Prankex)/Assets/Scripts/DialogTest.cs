using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class DialogTest : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
   
    public float textSpeed;
    private int index;
    public PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public Interactable interactable;


    public bool DialogueStarted = false;

    //ATTENTION : ici, une "line" correspond � une "dialogue line", pas une ligne au sens 1 paragraphe = 30 lignes.
    //D'o� l'initialisation de textComponent quand on change de "line". Une line peut contenir plusieurs lignes de texte.
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
            //Si l'affichage de la ligne de dialogue est termin�e : on passe � la suivante
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            //Sinon on affiche le restant du text (pour les gens comme moi :))
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];

            }
        }
    }

    void Start()
    {

        gameObject.SetActive(false);
        
    }
    
    public void StartDialog()
    {
        //On rend le joueur static, mais il faut aussi BLOQUER TOUTES LES ACTIONS DES KEYS : LE JOUEUR PEUT TOUJOURS TAPER AVEC CLIC, TOURNER SA VUE AVEC DROITE GAUCHE, DASH AVEC CONTROL ETC.
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());

    }
    //Ecrit une ligne enti�re (ligne � r�f�rencer soit dans le code, soit dans Unity... plus pratique dans le code � l'avenir bien s�r.
    IEnumerator TypeLine()
    {
        
        //Display les caract�res les uns apr�s les autres � la vitesse textSpeed
        foreach (char c in lines[index].ToCharArray())
         {
            textComponent.text += c;
             yield return new WaitForSeconds(textSpeed);
            DialogueStarted = true;

        }
        
        NextLine();
        
    }
    void NextLine()
    {
        //Tant qu'il reste des lignes dans la liste de lignes
        if(index < lines.Length - 1) 
        { 
            index++;
            //On cr�e le textComponent initialis� � vide
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //Si il ne reste plus aucune ligne � display, on quitte et on rend les mouvements libres 
            gameObject.SetActive(false);
            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);
            
            DialogueStarted = false;
            

        }
    }

    
    
        
    
}
