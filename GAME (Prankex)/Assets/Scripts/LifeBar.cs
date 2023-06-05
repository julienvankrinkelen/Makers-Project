using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class LifeBar : MonoBehaviour
{

    public GameObject[] Hearts;
    public GameObject[] HeartShadows;
    
    //Taille du tableau Hearts et HeartShadows
    private int nbOfAssets = 5;
    public PlayerCombat player;
    // Update is called once per frame

    private void Start()
    {
        
    }
    void Update()
    {
        // En partant du principe que maxHealth >= currentHealth et que tout est bien géré.
        // Programmation défensive possible

        setNbHeartsAssets( (int) player.maxHealth);
        setLifeAssets( (int) player.CurrentHealth);
    }

    private void setNbHeartsAssets(int maxHealth)
    {
        for(int i=0; i<maxHealth; i++)
        {
            HeartShadows[i].SetActive(true);
            Hearts[i].SetActive(false);

        }
        for (int i=maxHealth; i<nbOfAssets; i++)
        {
            HeartShadows[i].SetActive(false);
        }
    }

    private void setLifeAssets(int currentHealth)
    {
        for (int i = 0; i < currentHealth; i++)
        {
            Hearts[i].SetActive(true);
            HeartShadows[i].SetActive(false);
        }
        for (int i = currentHealth; i < nbOfAssets; i++)
        {
            Hearts[i].SetActive(false);
        }
    }



}
