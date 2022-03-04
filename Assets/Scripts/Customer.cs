using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Selectable
{
    public Text targetSaladText;

    private Salad targetSalad;

    private List<VegetableType> vegeList; //store list to reuse each load
    private VegetableType[] shuffleVList; //list to shuffle and pull from each load

    protected override void Start()
    {
        base.Start(); //handle Selectable Start functions

        vegeList = new List<VegetableType>(); //init vege list

        //set length to count of vegeTypes (minus the None type)
        shuffleVList = new VegetableType[System.Enum.GetValues(typeof(VegetableType)).Length - 1];
        //initialize the shuffling list with the ordered vegetabletypes enum
        for (int i = 0; i < shuffleVList.Length; i++)
        {
            shuffleVList[i] = (VegetableType)(i + 1);
        }

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
        //clear out the vege List
        vegeList.Clear();

        //shuffle the stored vege list
        ShuffleVegeList();

        //decide how many vegetables in this salad
        int vegetableCount = Random.Range(1, 4); //evenly weighted between 1, 2, or 3. this could be higher
        //pull the desired number of vegetables from the shuffled list
        for(int i = 0; i < vegetableCount; i++)
        {
            vegeList.Add(shuffleVList[i]);
        }

        //return new salad with the created random vege combo (not a new vegetable and is a finished salad for comparison)
        return new Salad(vegeList, false, true);
    }

    private void ShuffleVegeList()
    {
        //shuffle the list for a random order of vegetables each load
        for (int i = 0; i < shuffleVList.Length; i++)
        {
            VegetableType temp = shuffleVList[i];
            int randomIndex = Random.Range(i, shuffleVList.Length);
            shuffleVList[i] = shuffleVList[randomIndex];
            shuffleVList[randomIndex] = temp;
        }
    }

    public void HandedSalad(Player p, List<VegetableType> veges)
    {
        //handle when a player hands this customer a salad

        //score handling
        if(EqualVegeCombos(veges, targetSalad.vegetableCombination))
        {
            p.AdjustScore(10);
        }
        else
        {
            Debug.Log("Wrong salad!");
        }

        //give the customer a new desired Salad
        LoadNewSalad();
    }

    private bool EqualVegeCombos(List<VegetableType> list1, List<VegetableType> list2)
    {
        if(list1 != null && list2 != null)
        {
            if(list1.Count == list2.Count)
            {
                for(int i = 0; i < list1.Count; i++)
                {
                    if(list1[i] != list2[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }
}
