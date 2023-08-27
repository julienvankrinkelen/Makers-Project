using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class BossRPEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI nameComponent;

    [SerializeField] private float textSpeed;
    private int index;
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject box;
    [SerializeField] private BossScript bossScript;
    [SerializeField] private Credits credits;

    [SerializeField] private bool DialogueStarted = false;
    [SerializeField] private string[] lines;

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
            //Sinon on affiche le restant du texte
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
        StartCoroutine(TypeLine());
    }

    //Ecrit une ligne entière (ligne à référencer soit dans le code, soit dans Unity).
    public IEnumerator TypeLine()
    {
        //print("TYPO LINO");
        //Display les caractères les uns après les autres à la vitesse textSpeed
        foreach (char c in lines[index].ToCharArray())
        {
           // print("WRITING");
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
            //On crée le textComponent initialisé à vide
            textComponent.text = string.Empty;
            if(index%2 == 0)
            {

                nameComponent.text = "Hina";
            }
            else
            {
               // print("INDEX : " + index);
               // print("YORU DISPLAYS");
                nameComponent.text = "Yoru";
            }
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            //Si il ne reste plus aucune ligne à display, on quitte et on rend les mouvements libres 

            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);
            DialogueStarted = false;
            box.SetActive(false);
            credits.StartCredits();

        }
    }



}