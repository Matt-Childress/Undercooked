using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //store the type of pickup this is
    public PickupType type;

    //store the player who can exclusively pick this up
    public Player targetPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //only give the pickup if the player is the one who earned it
        Player enteringPlayer = collision.GetComponent<Player>();

        if (enteringPlayer != null && enteringPlayer == targetPlayer)
        {
            GivePickup(enteringPlayer);
        }
    }

    private void GivePickup(Player p)
    {
        //apply pickup effect to player
        switch(type)
        {
            case PickupType.Speed:
                p.AddPickupSpeed();
                break;
            case PickupType.Time:
                p.AddPickupTime();
                break;
            default: //PickupType.Score
                p.AdjustScore(10);
                break;
        }

        //destroy the pickup after use
        Destroy(gameObject);
    }
}
