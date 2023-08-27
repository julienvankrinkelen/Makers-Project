using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptHUD : MonoBehaviour
{

    public GameObject[] Hearts;
    public GameObject[] HeartShadows;

    public GameObject[] darumaCirclesOn;
    public GameObject[] darumaCirclesOff;
    public GameObject[] omamoriCirclesOn;
    public GameObject[] omamoriCirclesOff;

    [SerializeField] private TextMeshProUGUI textDaruma;
    [SerializeField] private TextMeshProUGUI textOmamori;

    [SerializeField] private GameObject scrollSpriteOn;
    [SerializeField] private GameObject scrollSpriteOff;
    [SerializeField] private TextMeshProUGUI textScroll;

    [SerializeField] private GameObject noteSpriteOn;
    [SerializeField] private GameObject noteSpriteOff;
    [SerializeField] private TextMeshProUGUI textNotes;

    [SerializeField] private GameObject candleSpriteOn;
    [SerializeField] private GameObject candleSpriteOff;

    [SerializeField] private GameObject dashSpriteOn;
    [SerializeField] private GameObject dashSpriteOff;

    
    //Taille du tableau Hearts et HeartShadows
    private int nbOfLifeAssets = 9;
    private int nbOfCircleAssets = 3;
    [SerializeField] private PlayerCombat player;
    [SerializeField] private PlayerCollectibles playerCollectibles;

    void Update()
    {
        // En partant du principe que maxHealth >= currentHealth et que tout est bien géré.
        // Programmation défensive possible

        setNbHeartsAssets( (int) player.maxHealth);
        if (player.CurrentHealth > 0)
        {
            setLifeAssets((int)player.CurrentHealth);
        }
        setNbDarumaCircles();
        setNbOmamoriCircles();
        setTextDaruma();
        setTextOmamori();
        setTextSpriteScroll();
        setTextSpriteNotes();
        setSpriteCandle();
        setSpriteDash();
    }
   
    private void setSpriteCandle()
    {
        if (playerCollectibles.checkHasCandle())
        {
            candleSpriteOn.SetActive(true);
            candleSpriteOff.SetActive(false);
        }
        else
        {
            candleSpriteOn.SetActive(false);
            candleSpriteOff.SetActive(true);
        }
    }
    private void setSpriteDash()
    {
        if (playerCollectibles.checkHasDash())
        {
            dashSpriteOn.SetActive(true);
            dashSpriteOff.SetActive(false);
        }
        else
        {
            dashSpriteOn.SetActive(false);
            dashSpriteOff.SetActive(true);
        }
    }

    private void setTextSpriteScroll()
    {
        int numberOfScrolls = playerCollectibles.getExplosiveScrollNumber();
        if(numberOfScrolls == 0)
        {
            textScroll.text = "";
        }
        else
        {
            textScroll.text = "" + numberOfScrolls;
        }
        if (numberOfScrolls > 0)
        {
            scrollSpriteOn.SetActive(true);
            scrollSpriteOff.SetActive(false);
        }
        else
        {
            scrollSpriteOn.SetActive(false);
            scrollSpriteOff.SetActive(true);
        }
    }

    private void setTextSpriteNotes()
    {
        int numberOfNotes = playerCollectibles.getNumberOfNotes();
        if(numberOfNotes == 0)
        {
            textNotes.text = "";
        }
        else
        {
            textNotes.text = "" + numberOfNotes;
        }
        if (numberOfNotes > 0)
        {
            noteSpriteOn.SetActive(true);
            noteSpriteOff.SetActive(false);
        }
        else
        {
            noteSpriteOn.SetActive(false);
            noteSpriteOff.SetActive(true);
        }
    }
    private void setTextDaruma()
    {
        int lvlNumber = playerCollectibles.getDarumaNumber() / 3;
        switch (lvlNumber){
            case 0:
                textDaruma.text = "<color=#767676>Lvl " + lvlNumber + "</color>";
                break;
            case 1:
                textDaruma.text = "<color=#FFFFFF>Lvl " + lvlNumber + "</color>";
                break;
            case 2:
                textDaruma.text = "<color=#45d850>Lvl " + lvlNumber + "</color>";
                break;
            case 3:
                textDaruma.text = "<color=#233ec3>Lvl " + lvlNumber + "</color>";
                break;
            case 4:
                textDaruma.text = "<color=#9b21c4>Lvl " + lvlNumber + "</color>";
                break;
            case 5:
                textDaruma.text = "<color=#ea9d35>Lvl " + lvlNumber + "</color>";
                break;

        }
              
    }
    private void setTextOmamori()
    {
        int lvlNumber = playerCollectibles.getOmamoriNumber() / 3;
        switch (lvlNumber)
        {
            case 0:
                textOmamori.text = "<color=#767676>Lvl " + lvlNumber + "</color>";
                break;
            case 1:
                textOmamori.text = "<color=#FFFFFF>Lvl " + lvlNumber + "</color>";
                break;
            case 2:
                textOmamori.text = "<color=#45d850>Lvl " + lvlNumber + "</color>";
                break;
            case 3:
                textOmamori.text = "<color=#233ec3>Lvl " + lvlNumber + "</color>";
                break;
            case 4:
                textOmamori.text = "<color=#9b21c4>Lvl " + lvlNumber + "</color>";
                break;
            case 5:
                textOmamori.text = "<color=#ea9d35>Lvl " + lvlNumber + "</color>";
                break;

        }
    }

    private void setNbOmamoriCircles()
    {
        // 0, 1 or 2 circles to display
        int nbCirclesToDisplay = playerCollectibles.getOmamoriNumber() % 3;
        for(int i=0; i<nbCirclesToDisplay; i++)
        {
            omamoriCirclesOff[i].SetActive(false);
            omamoriCirclesOn[i].SetActive(true);
        }
        for(int i=nbCirclesToDisplay; i<nbOfCircleAssets; i++)
        {
            omamoriCirclesOff[i].SetActive(true);
            omamoriCirclesOn[i].SetActive(false);
        }
    }
    private void setNbDarumaCircles()
    {
        // 0, 1 or 2 circles to display
        int nbCirclesToDisplay = playerCollectibles.getDarumaNumber() % 3;
        for (int i = 0; i < nbCirclesToDisplay; i++)
        {
            darumaCirclesOff[i].SetActive(false);
            darumaCirclesOn[i].SetActive(true);
        }
        for (int i = nbCirclesToDisplay; i < nbOfCircleAssets; i++)
        {
            darumaCirclesOff[i].SetActive(true);
            darumaCirclesOn[i].SetActive(false);
        }
    }
    private void setNbHeartsAssets(int maxHealth)
    {
        for(int i=0; i<maxHealth; i++)
        {
            HeartShadows[i].SetActive(true);
            Hearts[i].SetActive(false);
        }
        for (int i=maxHealth; i<nbOfLifeAssets; i++)
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
        for (int i = currentHealth; i < nbOfLifeAssets; i++)
        {
            Hearts[i].SetActive(false);
        }
    }



}
