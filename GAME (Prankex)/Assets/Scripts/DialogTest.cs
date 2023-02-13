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
    
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;

    //ATTENTION : ici, une "line" correspond à une "dialogue line", pas une ligne au sens 1 paragraphe = 30 lignes.
    //D'où l'initialisation de textComponent quand on change de "line". Une line peut contenir plusieurs lignes de texte.
    public string[] lines;
   
    void Start()
    {

        gameObject.SetActive(false);
        
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        { 
            //Si l'affichage de la ligne de dialogue est terminée : on passe à la suivante
            if(textComponent.text == lines[index])
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
    public void startDialog()
    {
        //On rend le joueur static, mais il faut aussi BLOQUER TOUTES LES ACTIONS DES KEYS : LE JOUEUR PEUT TOUJOURS TAPER AVEC CLIC, TOURNER SA VUE AVEC DROITE GAUCHE, DASH AVEC CONTROL ETC.
        playerMovement.EnableMovement(false);
        playerCombat.EnableCombat(false);
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }
    //Ecrit une ligne entière (ligne à référencer soit dans le code, soit dans Unity... plus pratique dans le code à l'avenir bien sûr.
    IEnumerator TypeLine()
    {
        //Display les caractères les uns après les autres à la vitesse textSpeed
        foreach (char c in lines[index].ToCharArray())
         {
            textComponent.text += c;
             yield return new WaitForSeconds(textSpeed);

         }
       
        NextLine();
        
    }
    void NextLine()
    {
        //Tant qu'il reste des lignes dans la liste de lignes
        if(index < lines.Length - 1) 
        { 
            index++;
            //On crée le textComponent initialisé à vide
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //Si il ne reste plus aucune ligne à display, on quitte et on rend les mouvements libres 
            gameObject.SetActive(false);
            playerMovement.EnableMovement(true);
            playerCombat.EnableCombat(true);
        }
    }
    
        
    
}
