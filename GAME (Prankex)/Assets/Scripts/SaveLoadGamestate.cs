using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveLoadGamestate : MonoBehaviour
{
    //player
    public PlayerCombat player;
    public Transform transformPlayer;

    //ennemies
    public Transform transformEnnemy1;






    public void SaveGamestate()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadGamestate()
    {
        Gamestate data = SaveSystem.LoadGamestate();

        //player
        Debug.Log("LOADING DATA : " + data.health);
        player.CurrentHealth = data.health;
        player.attackDamage = data.attackDamage;

        Vector2 positionPlayer;
        positionPlayer.x = data.positionPlayer[0];
        positionPlayer.y = data.positionPlayer[1];
        transformPlayer.position = positionPlayer;

        //ennemies
        Vector2 positionEnnemy1;
        positionEnnemy1.x = data.positionEnnemy1[0];
        positionEnnemy1.y = data.positionEnnemy1[1];
        transformEnnemy1.position = positionEnnemy1;
    }

    public Gamestate createGamestate(Gamestate gamestate)
    {
        //player
        gamestate.health = player.CurrentHealth;
        gamestate.attackDamage = player.attackDamage;

        gamestate.positionPlayer = new float[2];
        gamestate.positionPlayer[0] = transformPlayer.position.x;
        gamestate.positionPlayer[1] = transformPlayer.position.y;


        //ennemies
        gamestate.positionEnnemy1 = new float[2];
        gamestate.positionEnnemy1[0] = transformEnnemy1.position.x;
        gamestate.positionEnnemy1[1] = transformEnnemy1.position.y;
        return gamestate;
    }
}
