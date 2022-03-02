using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //hold the vegetable table objects
    public Vegetable[] vegetables;

    // Start is called before the first frame update
    void Start()
    {
        //this statement handles the case that vegetables are not defined in the editor
        if (vegetables.Length < 1 || vegetables == null)
        {
            vegetables = FindObjectsOfType<Vegetable>();
        }

        //here, make a string array of vegetable names and randomly assign the names types for a different placement each game
        string[] vegeEnumList = System.Enum.GetNames(typeof(Vegetables));
        if (vegetables.Length == vegeEnumList.Length) //error checking if the number of defined vegetable tables and vegetable enum types are the same
        {
            for (int i = 0; i < vegeEnumList.Length; i++)//shuffle the list for a random placement of vegetables each game
            {
                string temp = vegeEnumList[i];
                int randomIndex = Random.Range(i, vegeEnumList.Length);
                vegetables[i].AssignVegetable(vegeEnumList[randomIndex]);
                vegeEnumList[randomIndex] = temp;
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
