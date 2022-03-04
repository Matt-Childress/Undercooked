using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Selectable
{
    public Text targetSaladText;

    public Slider waitingBarSlider;

    private Salad targetSalad;

    private List<VegetableType> vegeList; //store list to reuse each load
    private VegetableType[] shuffleVList; //list to shuffle and pull from each load

    private float waitTime; //store how long the customer will wait before leaving and new order load
    private const float vegetableTimeValue = 20f; //each vegetable adds 20 seconds to the customer's wait time

    private Coroutine waiting;

    private GameManager gm; //hold reference to GameManager instance

    protected override void Start()
    {
        base.Start(); //handle Selectable Start functions

        gm = GameManager.instance;

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

        //update the customer's wait time based on number of vegetables
        waitTime = targetSalad.vegetableCombination.Count * vegetableTimeValue;
        //start customer waiting coroutine
        waiting = StartCoroutine(Waiting());
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

        //return new salad with the created random vege combo (is a chopped salad for comparison)
        return new Salad(vegeList, true);
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
            StopCoroutine(waiting); //stop the waiting coroutine
            LoadNewSalad(); //give the customer a new desired Salad
        }
        else
        {
            Debug.Log("Wrong salad!");
        }
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
                        return false; //if any of the vegetables aren't the same at the same index, the combos are NOT equal
                    }
                }

                return true; // if all vegetables are the same, the combos are equal
            }
        }

        return false; //if a list is null or has a different count than the other, the combos are NOT equal
    }

    private IEnumerator Waiting()
    {
        float totalTime = waitTime;

        while(waitTime > 0f)
        {
            yield return null;

            waitingBarSlider.value = Mathf.Lerp(1f, 0f, (totalTime - waitTime) / totalTime);

            waitTime -= Time.deltaTime;
        }

        //deduct score from both players
        gm.AdjustScoreOfBothPlayers(-10);

        //new order after done waiting
        LoadNewSalad();
    }
}
