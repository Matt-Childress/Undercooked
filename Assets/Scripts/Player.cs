using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;

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
        //make a Vector3 with horizontal along the x axis and vertical along the z axis (translated for a top-down view)
        Vector3 direction = new Vector3(hIn, 0, vIn);

        //update the player object's position with respect to the Vector3 direction and the player's speed
        transform.position += direction * speed;
    }
}
