using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;

    //hold the player's currently highlighted vegeTable
    [HideInInspector]
    public VegetableTable highlightedVegetable;

    //the player's held vegetables
    public VegetableType heldVegetable1 = 0;
    public VegetableType heldVegetable2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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

    }
}
