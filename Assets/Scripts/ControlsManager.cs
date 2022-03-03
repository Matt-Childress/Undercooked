using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    //hold reference to GameManager instance
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //grab GameManager instance on start
        gm = GameManager.instance;
    }

    // Update is called once per frame
    private void Update()
    {
        //listen for action key input, and handle appropriately
        HandleActions();
    }

    //using FixedUpdate for Movement since there are physics calculations on collisions
    void FixedUpdate()
    {
        //listen for movement key input, and send to the appropriate player movement method
        HandleMovement();
    }

    private void HandleMovement()
    {
        //getting input from keys
        float p1H = Input.GetAxisRaw("P1Horizontal");
        float p1V = Input.GetAxisRaw("P1Vertical");
        float p2H = Input.GetAxisRaw("P2Horizontal");
        float p2V = Input.GetAxisRaw("P2Vertical");

        if (p1H != 0f || p1V != 0f)
        {
            gm.player1.Move(p1H, p1V); //if horizontal or vertical keys are pressed, call the player1 movement method
        }

        if (p2H != 0f || p2V != 0f)
        {
            gm.player2.Move(p2H, p2V); //if horizontal or vertical keys are pressed, call the player2 movement method
        }
    }

    private void HandleActions()
    {
        //if an action key is pressed, call the player method for deciding which action to take
        if(Input.GetButtonDown("P1PickUp"))
        {
            gm.player1.PickItemUp();
        }
        
        if(Input.GetButtonDown("P2PickUp"))
        {
            gm.player2.PickItemUp();
        }

        if (Input.GetButtonDown("P1PutDown"))
        {
            gm.player1.PutItemDown();
        }

        if (Input.GetButtonDown("P2PutDown"))
        {
            gm.player2.PutItemDown();
        }
    }
}
