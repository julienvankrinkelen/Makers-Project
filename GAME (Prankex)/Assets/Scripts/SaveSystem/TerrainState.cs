using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainState : MonoBehaviour
{

    public bool[] wallDestroyed;
    public bool[] bushDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        wallDestroyed = new bool[12];
        bushDestroyed = new bool[4];
        fillArray(wallDestroyed);
        fillArray(bushDestroyed);
    }

    private void fillArray(bool[] tab)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            tab[i] = false;
        }
    }


    public void destroyWall(GameObject wall)
    {
        string wallName = wall.name;
        int wallNumber = parseCollectibleName(wallName);
        Debug.Log("Has Destroyed wall nb : " + wallNumber);

        wallDestroyed[wallNumber] = true;
    }

    public void destroyBush(GameObject bush)
    {
        string bushName = bush.name;
        int bushNumber = parseCollectibleName(bushName);
        Debug.Log("Has Destroyed bush nb : " + bushNumber);

        bushDestroyed[bushNumber] = true;
    }

    private int parseCollectibleName(string collectible)
    {
        return int.Parse(collectible.Substring(collectible.Length - 2));
    }
}
