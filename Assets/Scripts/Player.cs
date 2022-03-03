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
    public VegetableType heldVegetable1 = 0;
    [HideInInspector]
    public VegetableType heldVegetable2 = 0;

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

    // Update is called once per frame
    void Update()
    {
        
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
                VegetableTable vegetable = highlightedSelectable as VegetableTable; //make a temporary vegetableTable variable with access to the vegetable attributes
                if (heldVegetable1 == 0)
                {
                    heldVegetable1 = vegetable.type;
                    UpdateHeldVegetableUI();
                }
                else if (heldVegetable2 == 0)
                {
                    heldVegetable2 = vegetable.type;
                    UpdateHeldVegetableUI();
                }
            }
            else if(highlightedSelectable is ChoppingBlock)
            {
                ChoppingBlock chopBlock = highlightedSelectable as ChoppingBlock; //temporary variable with access to chopping block attributes

                if(heldVegetable1 > 0) //if the player is holding a vegetable, chop it
                {
                    Chop(chopBlock.choppingWaitTime);
                }
            }
        }
    }

    private void UpdateHeldVegetableUI()
    {
        //set text field above the player to the correct held vegetables
        heldVegetable1Text.text = heldVegetable1 != 0 ? heldVegetable1.ToString() : "";
        heldVegetable2Text.text = heldVegetable2 != 0 ? heldVegetable2.ToString() : "";
    }

    private void Chop(float waitTime)
    {
        //method starts a Chopping Coroutine to hold the player in place and chop the vegetable over a wait time
        StartCoroutine(ChoppingRoutine(waitTime));
    }

    private IEnumerator ChoppingRoutine(float waitTime)
    {
        //flip the isChopping bool to lock movement and actions by the player object, as well as lock the physics rigidbody so the other player can't push them
        isChopping = true;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(waitTime);

        rb.bodyType = RigidbodyType2D.Dynamic;
        isChopping = false;
    }
}
