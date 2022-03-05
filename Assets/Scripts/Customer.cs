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

    private AngerTargetPlayer angerTarget; //variables for angry customer handling
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
            correctSalad = p.heldSalad1;
        }
        else if(p.heldSalad2 != null && SaladMatchesTarget(p.heldSalad2.vegetableCombination))
        {
            correctSalad = p.heldSalad2;
        }

        //if a held salad was correct
        if(correctSalad != null)
        {
            if(GetWaitTimeRemainingPercentage() >= 0.7f)//if the correct delivery was made with 70% wait time remaining or more
            {
                //spawn a pickup for the appropriate player
                gm.SpawnPickup(p);
            }

            p.AdjustScore(10); //score handling
            StopCoroutine(waiting); //stop the waiting coroutine
            LoadNewSalad(); //give the customer a new desired Salad
        }
        else
        {
            //handle when an incorrect salad is given
            MakeAngry(p);
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

            if(angerTarget != AngerTargetPlayer.None) //if the customer is angry
            {
                speed = 2f;
            }
            waitTime -= Time.deltaTime * speed;
        }

        if (angerTarget == AngerTargetPlayer.None)
        {
            //deduct normal score from both players if the customer isn't angry
            gm.AdjustScoreOfBothPlayers(-10);
        }
        else
        {
            //deduct double score from the anger target players if the customer is angry
            switch(angerTarget)
            {
                case AngerTargetPlayer.Player1:
                    gm.player1.AdjustScore(-20);
                    break;
                case AngerTargetPlayer.Player2:
                    gm.player2.AdjustScore(-20);
                    break;
                default:
                    gm.AdjustScoreOfBothPlayers(-20);
                    break;
            }
        }

        //new order after done waiting
        LoadNewSalad();
    }

    private void MakeAngry(Player target)
    {
        //handle making the customer angry
        
        if(target == gm.player1) //if the customer is getting angered by player 1
        {
            angerTarget = angerTarget == AngerTargetPlayer.None ? AngerTargetPlayer.Player1 : AngerTargetPlayer.Both; //add Player1 to the anger target type
        }
        else if(target == gm.player2) //if the customer is getting angered by player 2
        {
            angerTarget = angerTarget == AngerTargetPlayer.None ? AngerTargetPlayer.Player2 : AngerTargetPlayer.Both; //add Player2 to the anger target type
        }

        waitingBarFill.color = angryColor;
    }
    private void MakeCalm()
    {
        //handle making the customer calm again
        angerTarget = AngerTargetPlayer.None;
        waitingBarFill.color = calmColor;
    }

    private float GetWaitTimeRemainingPercentage()
    {
        //return the percent of waitTime remaining to decide if a pickup should be spawned
        return waitingBarSlider.value;
    }
}
