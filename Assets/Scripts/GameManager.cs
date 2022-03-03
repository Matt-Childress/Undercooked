﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //hold instance of GameManager for other objects to access
    public static GameManager instance;

    //hold player objects
    public Player player1;
    public Player player2;

    //hold the vegetable table objects
    public VegetableTable[] vegetableTables;

    // Start is called before the first frame update
    void Start()
    {
        //assign the gamemanager instance on start
        instance = this;

        //this statement will handle the case that players have not been defined in the scene
        if (!player1 || !player2)
        {
            var players = FindObjectsOfType<Player>();
            foreach (var player in players)
            {
                if (!player1)
                {
                    player1 = player;
                }
                else if (!player2)
                {
                    player2 = player;
                }
            }
        }

        //this statement handles the case that vegetables are not defined in the editor
        if (vegetableTables.Length < 1 || vegetableTables == null)
        {
            vegetableTables = FindObjectsOfType<VegetableTable>();
        }

        //here, make a string array of vegetable names and randomly assign the names types for a different placement each game
        string[] vegeTypeList = System.Enum.GetNames(typeof(VegetableType));
        if (vegetableTables.Length == vegeTypeList.Length - 1) //error checking if the number of defined vegetable tables and vegetable enum types are the same
        {
            for (int i = 1; i < vegeTypeList.Length; i++)//shuffle the list for a random placement of vegetables each game
            {
                string temp = vegeTypeList[i];
                int randomIndex = Random.Range(i, vegeTypeList.Length);
                vegetableTables[i - 1].AssignVegetable(vegeTypeList[randomIndex]);
                vegeTypeList[randomIndex] = temp;
            }
        }
        else
        {
            Debug.LogError("Number of Vegetables and Number of Vegetable enum values differs");
        }
    }

    public void PlayerTimedOut()
    {
        //if both players are out of time, end the game
        if(player1.timeLeft <= 0 && player2.timeLeft <= 0)
        {
            //end game ui popup
            Debug.Log("Game Over");
        }
    }
}
