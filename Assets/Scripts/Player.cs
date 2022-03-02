using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;

    //hold the player's currently highlighted vegeTable
    [HideInInspector]
    public VegetableTable highlightedVegetable;

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
}
