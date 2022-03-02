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
    public VegetableTable highlightedVegetable;
    [HideInInspector]
    public ChoppingBlock highlightedChopBlock;

    //the player's held vegetables
    [HideInInspector]
    public VegetableType heldVegetable1 = 0;
    [HideInInspector]
    public VegetableType heldVegetable2 = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //player movement method
    public void Move(float hIn, float vIn)
    {
        //make a Vector2 with the movement input parameters
        Vector3 direction = new Vector2(hIn, vIn);

        //update the player object's position with respect to the Vector3 direction and the player's speed
        transform.position += direction * speed;
    }

    //method for deciding on which action to perform, if any
    public void PerformAction()
    {
        //this if statement looks at situations like if a player is targetting a vegetable or cutting board, and decides with a priority which to perform

        if(highlightedVegetable) //picking up vegetables from tables
        {
            if(heldVegetable1 == 0)
            {
                heldVegetable1 = highlightedVegetable.type;
                UpdateHeldVegetableUI();
            }
            else if(heldVegetable2 == 0)
            {
                heldVegetable2 = highlightedVegetable.type;
                UpdateHeldVegetableUI();
            }
            else
            {
                Debug.Log("Hands are full, can't pick up a 3rd Vegetable");
            }
        }
    }

    private void UpdateHeldVegetableUI()
    {
        //set text field above the player to the correct held vegetables
        heldVegetable1Text.text = heldVegetable1 != 0 ? heldVegetable1.ToString() : "";
        heldVegetable2Text.text = heldVegetable2 != 0 ? heldVegetable2.ToString() : "";
    }
}
