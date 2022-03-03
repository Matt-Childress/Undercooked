using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //speed of player movement
    public float speed = 1;

    //text fields above the player that display what vegetables are picked up
    public Text heldVegetable1Text;
    public Text heldVegetable2Text;

    //hold the player's currently highlighted vegeTable and chopBlocks
    [HideInInspector]
    public Selectable highlightedSelectable;

    //the player's held vegetables
    [HideInInspector]
    public Vegetable heldVegetable1;
    [HideInInspector]
    public Vegetable heldVegetable2;

    private bool isChopping;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //handling if heldVegetableTexts are undefined on start
        if(!heldVegetable1Text || !heldVegetable2Text)
        {
            Text[] vegeTexts = GetComponentsInChildren<Text>();
            if(!heldVegetable1Text)
            {
                heldVegetable1Text = vegeTexts[0];
            }
            if(!heldVegetable2Text)
            {
                heldVegetable2Text = vegeTexts[1];
            }
        }

        //setting rigidbody on start
        rb = GetComponent<Rigidbody2D>();
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

    //method for deciding on which action to perform, if any
    public void PerformAction()
    {
        if (highlightedSelectable && !isChopping) //dont perform anything if the player is chopping or if nothing is selected
        {
            if (highlightedSelectable is VegetableTable) //if the highlighted selectable is a vegetable table
            {
                VegetableTable vTable = highlightedSelectable as VegetableTable; //make a temporary vegetableTable variable with access to the vegetable attributes
                PickupVegetable(vTable.type);                
            }
            else if(highlightedSelectable is ChoppingBlock)
            {
                ChoppingBlock chopBlock = highlightedSelectable as ChoppingBlock; //temporary variable with access to chopping block attributes

                if(heldVegetable1 != null && !heldVegetable1.isChopped) //if the player is holding a vegetable that isn't already chopped, chop it
                {
                    chopBlock.StartChop(this, heldVegetable1);
                }
                else if(heldVegetable2 != null && !heldVegetable2.isChopped)
                {
                    chopBlock.StartChop(this, heldVegetable2); //handling dropping from slot 2
                }
            }
        }
    }

    private void UpdateHeldVegetableUI()
    {
        //set text field above the player to the correct held vegetables, or empty if no vegetable is held in that slot
        heldVegetable1Text.text = heldVegetable1 != null ? heldVegetable1.GetVegetableText() : string.Empty;
        heldVegetable2Text.text = heldVegetable2 != null ? heldVegetable2.GetVegetableText() : string.Empty;
    }

    public void HandlePlayerLock(bool locking)
    {
        //flip the isChopping bool to lock/unlock movement and actions by the player object, as well as the physics rigidbody so the other player can't push them
        isChopping = locking;
        rb.bodyType = locking ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }

    public void PickupVegetable(VegetableType vType, bool chopped = false)
    {
        //create a vegetable object and slot it to the correct hand
        if (heldVegetable1 == null)
        {
            heldVegetable1 = new Vegetable(vType, chopped);
        }
        else if (heldVegetable2 == null)
        {
            heldVegetable2 = new Vegetable(vType, chopped);
        }

        //update the held vege UI texts
        UpdateHeldVegetableUI();
    }

    public void DropVegetable(Vegetable vege)
    {
        //handle when a player puts a vegetable down
        if(heldVegetable1 != null && heldVegetable1 == vege)
        {
            if(heldVegetable2 != null) //if there is a 2nd vege, slide it into slot 1
            {
                heldVegetable1 = heldVegetable2;
                heldVegetable2 = null;
            }
            else //otherwise just drop the 1st vege
            {
                heldVegetable1 = null;
            }
        }
        else if(heldVegetable2 != null && heldVegetable2 == vege) //if dropping the 2nd vege, no slide required
        {
            heldVegetable2 = null;
        }

        //update held vege UI
        UpdateHeldVegetableUI();
    }
}
