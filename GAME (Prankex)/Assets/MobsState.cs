using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsState : MonoBehaviour
{

    public EnemyScript enemyScript;
    public OnibiScript onibiScript;

    public bool[] tanukiDied;
    public float[] karakasaLife;
    public float[] onibiLife;
    void Start()
    {
        tanukiDied = new bool[12];
        karakasaLife = new float[11];
        onibiLife = new float[7];

        fillArrayBool(tanukiDied);
        fillArrayKarakasa(karakasaLife);
        fillArrayOnibi(onibiLife);
    }

   
    private void fillArrayBool(bool[] tab)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            tab[i] = false;
        }
    }
    private void fillArrayKarakasa(float[] tab)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            tab[i] = enemyScript.maxHealth;
        }
    }
    private void fillArrayOnibi(float[] tab)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            tab[i] = onibiScript.maxHealth;
        }
    }


    public void dieTanuki(GameObject tanuki)
    { 
        int tanukiNumber = parseMobName(tanuki.name);
        tanukiDied[tanukiNumber] = true;
    }

    public void registerLifeKarakasa(GameObject karakasa, float currentHealth)
    {
        int karakasaNumber = parseMobName(karakasa.name);
        karakasaLife[karakasaNumber] = currentHealth;
    }
    public void registerLifeOnibi(GameObject onibi, float currentHealth)
    {
        int onibiNumber = parseMobName(onibi.name);
        onibiLife[onibiNumber] = currentHealth;
    }

    private int parseMobName(string collectible)
    {
        return int.Parse(collectible.Substring(collectible.Length - 2));

    }

}
