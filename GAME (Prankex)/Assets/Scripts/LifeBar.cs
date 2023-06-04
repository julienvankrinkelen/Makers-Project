using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class LifeBar : MonoBehaviour
{

    public GameObject[] Hearts;
    public GameObject[] HeartShadows;
    /*
    public GameObject Heart0;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;

    public GameObject HeartShadow0;
    public GameObject HeartShadow1;
    public GameObject HeartShadow2;
    public GameObject HeartShadow3;
    */
    public PlayerCombat player;
    // Update is called once per frame

    private void Start()
    {
        
    }
    void Update()
    {
        switch (player.CurrentHealth)
        {
            case 0:
                setLifeAssets(0);

                break;
            case 1:
                setLifeAssets(1);
                break;
            case 2:
                setLifeAssets(2);

                break;
            case 3:
                setLifeAssets(3);

                break;
            case 4:
                setLifeAssets(4);

                break;

        }
    }

    private void setLifeAssets(int nbOfAssets)
    {
        for(int i=0; i<nbOfAssets; i++)
        {
            Hearts[i].SetActive(true);
            HeartShadows[i].SetActive(false);
        }
        for(int i=nbOfAssets; i<player.maxHealth; i++)
        {

            Hearts[i].SetActive(false);
            HeartShadows[i].SetActive(true);
        }
    }
   

}
