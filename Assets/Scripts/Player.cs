using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //speed of player movement
    public float speed = 1;

    //players time remaining to play
    public float timeLeft = 300f;
    public int score = 0;

    //references to text fields for time and score
    public Text timeText;
    public Text scoreText;

    //text fields above the player that display what vegetables are picked up
    public Text heldSalad1Text;
    public Text heldSalad2Text;

    //hold the player's currently highlighted vegeTable and chopBlocks
    [HideInInspector]
    public Selectable highlightedSelectable;

    //the player's held vegetables
    [HideInInspector]
    public Salad heldSalad1;
    [HideInInspector]
    public Salad heldSalad2;

    private bool isChopping;

    private Rigidbody2D rb;

    private GameManager gm;

    //used for speed adjustment on pickups
    private float normalSpeed;
    private float fastSpeed;

    private Coroutine speedPickupCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //handling if heldVegetableTexts are undefined on start
        if(!heldSalad1Text || !heldSalad2Text)
        {
            Text[] vegeTexts = GetComponentsInChildren<Text>();
            if(!heldSalad1Text)
            {
                heldSalad1Text = vegeTexts[0];
            }
            if(!heldSalad2Text)
            {
                heldSalad2Text = vegeTexts[1];
            }
        }

        //setting rigidbody on start
        rb = GetComponent<Rigidbody2D>();

        //setting reference to GameManager instance
        gm = GameManager.instance;

        //start time countdown
        StartCoroutine(Countdown());

        //initialize score
        AdjustScore(0);

        //set normal and fast speeds for pickups
        normalSpeed = speed;
        fastSpeed = normalSpeed + 0.05f;
    }

    //player movement method
    public void Move(float hIn, float vIn)
    {
        if (!isChopping) //don't allow movement if the player is currently chopping
        {
            //make a Vector2 with the movement input parameters
            Vector3 direction = new Vector2(hIn, vIn);

            //update the player object's position with respect to the Vector3 direction and the player's speed
            transform.position += direction * speed;
        }
    }

    //method for deciding on which action to perform when pick up button is pressed, if any
    public void PickItemUp()
    {
        if (highlightedSelectable && !isChopping) //dont perform anything if the player is chopping or if nothing is selected
        {
            if (highlightedSelectable is VegetableTable) //if the highlighted selectable is a vegetable table
            {
                VegetableTable vTable = highlightedSelectable as VegetableTable; //make a temporary vegetableTable variable with access to the vegetable attributes
                List<VegetableType> newVege = new List<VegetableType>();
                newVege.Add(vTable.type); //add the vegetable to a new list for initialization as a new salad
                Salad sal = new Salad(newVege, false);
                PickupSalad(sal);                
            }
            else if(highlightedSelectable is ChoppingBlock) //if selecting a chopping block
            {
                ChoppingBlock chopBlock = highlightedSelectable as ChoppingBlock; //temp variable with access to chopping block attributes
                if(chopBlock.heldSalad != null && PickupSalad(chopBlock.heldSalad)) //try to pick up salad from chopping block
                {
                    //remove salad from the table if it was successfully picked up
                    chopBlock.RemoveSalad();
                }
            }
            else if (highlightedSelectable is Plate) //if selecting a plate
            {
                Plate plate = highlightedSelectable as Plate; //variable with access to plate attributes
                if (plate.heldVege != null && PickupSalad(plate.heldVege)) //try to pick up vegetable from plate
                {
                    //remove vegetable from the plate if it was successfully picked up
                    plate.RemoveSalad();
                }
            }
        }
    }

    //method for deciding on which action to perform when put down button is pressed, if any
    public void PutItemDown()
    {
        if (highlightedSelectable && !isChopping) //dont perform anything if the player is chopping or if nothing is selected
        {
            if (highlightedSelectable is ChoppingBlock) //if selecting a chopping block
            {
                ChoppingBlock chopBlock = highlightedSelectable as ChoppingBlock; //temporary variable with access to chopping block attributes

                if (chopBlock.ValidChop(heldSalad1)) //if a held salad can be chopped
                {
                    chopBlock.StartChop(this, heldSalad1); //chop method
                }
                else if (chopBlock.ValidChop(heldSalad2)) //handling dropping from slot 2
                {
                    chopBlock.StartChop(this, heldSalad2);
                }
            }
            else if(highlightedSelectable is Customer)
            {
                Customer cust = highlightedSelectable as Customer; //access to customer attributes

                //can only hand finished salads to customers
                if((heldSalad1 != null && heldSalad1.isChopped) || (heldSalad2 != null && heldSalad2.isChopped))
                {
                    Salad salad = cust.HandedSalad(this); //let customer handle which salad is correct
                    DropSalad(salad);
                }
            }
            else if (highlightedSelectable is TrashCan)
            {
                //can only throw finished salads in the trash
                if (heldSalad1 != null && heldSalad1.isChopped)
                {
                    DropSalad(heldSalad1);
                    AdjustScore(-5);
                }
                else if (heldSalad2 != null && heldSalad2.isChopped)
                {
                    DropSalad(heldSalad2);
                    AdjustScore(-5);
                }
            }
            else if (highlightedSelectable is Plate)
            {
                Plate plate = highlightedSelectable as Plate; //variable with access to plate attributes

                if(heldSalad1 != null && plate.ValidVegetable(heldSalad1))
                {
                    plate.HoldVegetable(heldSalad1);
                    DropSalad(heldSalad1);
                }
                else if(heldSalad2 != null && plate.ValidVegetable(heldSalad2))
                {
                    plate.HoldVegetable(heldSalad2);
                    DropSalad(heldSalad2);
                }
            }
        }
    }

    private void UpdateHeldSaladUI()
    {
        //update slot 1 salad text and color
        if(heldSalad1 != null)
        {
            heldSalad1Text.text = heldSalad1.GetSaladText();
            heldSalad1Text.color = heldSalad1.GetSaladColor();
        }
        else
        {
            heldSalad1Text.text = string.Empty;
        }

        //update slot 2 salad text and color
        if (heldSalad2 != null)
        {
            heldSalad2Text.text = heldSalad2.GetSaladText();
            heldSalad2Text.color = heldSalad2.GetSaladColor();
        }
        else
        {
            heldSalad2Text.text = string.Empty;
        }
    }

    public void HandlePlayerLock(bool locking)
    {
        //flip the isChopping bool to lock/unlock movement and actions by the player object, as well as the physics rigidbody so the other player can't push them
        isChopping = locking;
        rb.bodyType = locking ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }

    public bool PickupSalad(Salad salad)
    {
        //create a salad object and slot it to the correct hand
        if (heldSalad1 == null)
        {
            heldSalad1 = salad;
        }
        else if (heldSalad2 == null)
        {
            heldSalad2 = salad;
        }
        else
        {
            return false;
        }

        //update the held salad UI texts
        UpdateHeldSaladUI();
        return true;
    }

    public void DropSalad(Salad salad)
    {
        //handle when a player puts a salad down
        if(heldSalad1 != null && heldSalad1 == salad)
        {
            if(heldSalad2 != null) //if there is a 2nd salad, slide it into slot 1
            {
                heldSalad1 = heldSalad2;
                heldSalad2 = null;
            }
            else //otherwise just drop the 1st salad
            {
                heldSalad1 = null;
            }
        }
        else if(heldSalad2 != null && heldSalad2 == salad) //if dropping the 2nd salad, no slide required
        {
            heldSalad2 = null;
        }

        //update held vege UI
        UpdateHeldSaladUI();
    }

    public void AdjustScore(int adjustment)
    {
        score += adjustment;

        //dont let score go below 0
        if(score < 0)
        {
            score = 0;
        }

        scoreText.text = score.ToString();
    }

    public void AddPickupTime()
    {
        //add 10 seconds to play time left and update the UI
        timeLeft += 10;
        timeText.text = timeLeft.ToString();
    }

    public void AddPickupSpeed()
    {
        //handle adding player speed for 30 seconds. only having 1 coroutine active at a time effectively caps the player speed, and restarts the time if a 2nd speed pickup is grabbed
        if(speedPickupCoroutine != null)
        {
            StopCoroutine(speedPickupCoroutine);
        }

        speedPickupCoroutine = StartCoroutine(SpeedPickupRoutine());
    }

    private IEnumerator SpeedPickupRoutine()
    {
        //give player a speed boost for 30 seconds then set back to normal speed
        speed = fastSpeed;

        float timer = 0f;
        float totalTime = 30f;

        while(timer < totalTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        speed = normalSpeed;
    }

    private IEnumerator Countdown()
    {
        //initialize timeText
        timeText.text = timeLeft.ToString();

        float timeStep = 1f;

        while(timeLeft > 0)
        {
            yield return new WaitForSeconds(timeStep);
            timeLeft -= timeStep;
            timeText.text = timeLeft.ToString();
        }

        gm.PlayerTimedOut(this); //when a player times out, check if the game needs to end
    }
}
