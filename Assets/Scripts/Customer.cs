using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Selectable
{
    public Text targetSaladText;

    public Slider waitingBarSlider;

    public Image waitingBarFill;

    private Salad targetSalad;

    private List<VegetableType> vegeList; //store list to reuse each load
    private VegetableType[] shuffleVList; //list to shuffle and pull from each load

    private float waitTime; //store how long the customer will wait before leaving and new order load
    private const float vegetableTimeValue = 20f; //each vegetable adds 20 seconds to the customer's wait time

    private Coroutine waiting;

    private GameManager gm; //hold reference to GameManager instance

    private bool isAngry; //variables for angry customer handling
    private Color calmColor;
    private Color angryColor = Color.red;

    protected override void Start()
    {
        base.Start(); //handle Selectable Start functions

        gm = GameManager.instance;

        //set normal waiting bar color to starting color
        calmColor = waitingBarFill.color;

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
        //calm the new customer
        MakeCalm();

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

    public Salad HandedSalad(Player p)
    {
        //handle when a player hands this customer a salad
        Salad correctSalad = null;

        //find which held salad should be given if any
        if(p.heldSalad1 != null && SaladMatchesTarget(p.heldSalad1.vegetableCombination))
        {
            correctSalad = p.heldSalad2;
        }
        else if(p.heldSalad2 != null && SaladMatchesTarget(p.heldSalad2.vegetableCombination))
        {
            correctSalad = p.heldSalad2;
        }

        //if a held salad was correct
        if(correctSalad != null)
        {
            p.AdjustScore(10); //score handling
            StopCoroutine(waiting); //stop the waiting coroutine
            LoadNewSalad(); //give the customer a new desired Salad
        }
        else
        {
            //handle when an incorrect salad is given
            MakeAngry();
        }

        return correctSalad;
    }

    private bool SaladMatchesTarget(List<VegetableType> givenSalad)
    {
        if(givenSalad != null)
        {
            if(givenSalad.Count == targetSalad.vegetableCombination.Count)
            {
                for(int i = 0; i < givenSalad.Count; i++)
                {
                    if(givenSalad[i] != targetSalad.vegetableCombination[i])
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
        float speed = 1f;

        while(waitTime > 0f)
        {
            yield return null;

            waitingBarSlider.value = Mathf.Lerp(1f, 0f, (totalTime - waitTime) / totalTime);

            if(isAngry)
            {
                speed = 2f;
            }
            waitTime -= Time.deltaTime * speed;
        }

        //deduct score from both players
        gm.AdjustScoreOfBothPlayers(-10);

        //new order after done waiting
        LoadNewSalad();
    }

    private void MakeAngry()
    {
        //handle making the customer angry
        isAngry = true;
        waitingBarFill.color = angryColor;
    }
    private void MakeCalm()
    {
        //handle making the customer calm again
        isAngry = false;
        waitingBarFill.color = calmColor;
    }
}
