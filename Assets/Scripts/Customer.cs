using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Selectable
{
    public Text targetSaladText;

    private Salad targetSalad;

    protected override void Start()
    {
        base.Start(); //handle Selectable Start functions

        //initialize the customer's data
        LoadNewSalad();
    }

    private void LoadNewSalad()
    {
        //get new salad combo
        targetSalad = RandomSalad();

        //display the customer's desired salad
        targetSaladText.text = targetSalad.GetSaladText();
    }

    private Salad RandomSalad() //make a salad with a randomized vegetable combination
    {
        //decide how many vegetables in this salad
        int vegetableCount = Random.Range(0, 3); //evenly weighted between 1, 2, or 3...this could go higher

        //store number of vegetableType options for randomly selecting in loop below
        int vegeTypesCount = System.Enum.GetValues(typeof(VegetableType)).Length;

        //initialize list of vegetable types
        List<VegetableType> vegeList = new List<VegetableType>();
        for(int i = 0; i <= vegetableCount; i++)
        {
            vegeList.Add((VegetableType)Random.Range(1, vegeTypesCount)); //add vegetable type enum at the randomly chosen index value
        }

        //return new salad with the created random vege combo (not a new vegetable and is a finished salad for comparison)
        return new Salad(vegeList, false, true);
    }

    public void HandedSalad(Player p, Salad s)
    {
        //handle when a player hands this customer a salad

        //TODO: score handling
        p.AdjustScore(10);

        //give the customer a new desired Salad
        LoadNewSalad();
    }
}
