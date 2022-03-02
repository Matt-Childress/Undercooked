using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //hold instance of GameManager for other objects to access
    public static GameManager instance;

    //hold the vegetable table objects
    public VegetableTable[] vegetableTables;

    // Start is called before the first frame update
    void Start()
    {
        //assign the gamemanager instance on start
        instance = this;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
