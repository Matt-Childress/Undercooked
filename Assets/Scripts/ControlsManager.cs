using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    // Start is called before the first frame update
    void Start()
    {
        //this statement will handle the case that players have not been defined in the scene
        if(!player1 || !player2)
        {
            var players = FindObjectsOfType<Player>();
            foreach(var player in players)
            {
                if(!player1)
                {
                    player1 = player;
                }
                else if(!player2)
                {
                    player2 = player;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //listen for movement key input, and send to the appropriate player movement method
        HandleMovement();
        //listen for action key input, and handle appropriately
        //HandleAction();
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
            player1.Move(p1H, p1V); //if horizontal or vertical keys are pressed, call the player1 movement method
        }

        if (p2H != 0f || p2V != 0f)
        {
            player2.Move(p2H, p2V); //if horizontal or vertical keys are pressed, call the player2 movement method
        }
    }
}
