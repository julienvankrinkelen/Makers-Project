using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    public GameObject Heart0;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;

    public GameObject HeartShadow0;
    public GameObject HeartShadow1;
    public GameObject HeartShadow2;
    public GameObject HeartShadow3;

    public PlayerCombat player;
    // Update is called once per frame
    void Update()
    {
        switch (player.CurrentHealth)
        {
            case 0:
                Heart0.SetActive(false);
                Heart1.SetActive(false);
                Heart2.SetActive(false);
                Heart3.SetActive(false);

                HeartShadow0.SetActive(true);
                HeartShadow1.SetActive(true);
                HeartShadow2.SetActive(true);
                HeartShadow3.SetActive(true);
                break;
            case 1:
                Heart0.SetActive(true);
                Heart1.SetActive(false);
                Heart2.SetActive(false);
                Heart3.SetActive(false);

                HeartShadow0.SetActive(false);
                HeartShadow1.SetActive(true);
                HeartShadow2.SetActive(true);
                HeartShadow3.SetActive(true);
                break;
            case 2:
                Heart0.SetActive(true);
                Heart1.SetActive(true);
                Heart2.SetActive(false);
                Heart3.SetActive(false);

                HeartShadow0.SetActive(false);
                HeartShadow1.SetActive(false);
                HeartShadow2.SetActive(true);
                HeartShadow3.SetActive(true);
                break;
            case 3:
                Heart0.SetActive(true);
                Heart1.SetActive(true);
                Heart2.SetActive(true);
                Heart3.SetActive(false);

                HeartShadow0.SetActive(false);
                HeartShadow1.SetActive(false);
                HeartShadow2.SetActive(false);
                HeartShadow3.SetActive(true);
                break;
            case 4:
                Heart0.SetActive(true);
                Heart1.SetActive(true);
                Heart2.SetActive(true);
                Heart3.SetActive(true);


                HeartShadow0.SetActive(false);
                HeartShadow1.SetActive(false);
                HeartShadow2.SetActive(false);
                HeartShadow3.SetActive(false);
                break;

        }
    }
}
