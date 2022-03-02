using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : MonoBehaviour
{
    private Image selectableImage;

    private Color litColor = Color.white;
    private Color dimColor;

    private int highlightingPlayerCount;

    // Start is called before the first frame update
    void Start()
    {
        //assign selectableImage and unselected color on start
        selectableImage = GetComponent<Image>();
        dimColor = selectableImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(Player player)
    {
        //light up the selectable
        selectableImage.color = Color.white;

        highlightingPlayerCount++;

        //replace highlighted selectable with the new selectable, dim the old selectable
        if (player.highlightedSelectable)
        {
            player.highlightedSelectable.Deselect(player);
        }
        player.highlightedSelectable = this;
    }

    public void Deselect(Player player)
    {
        //unassign the player's highlighted selectable if it is this selectable
        if (player.highlightedSelectable == this)
        {
            player.highlightedSelectable = null;
            highlightingPlayerCount--;
            if (highlightingPlayerCount == 0)
            {
                //dim the selectable if this was the only player highlighting it
                selectableImage.color = dimColor;
            }
        }
    }

    //handle when a player leaves a selectable
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            //deselect the selectable
            Deselect(player);
        }
    }

    //handle when a player contacts a selectable, or re-highlights a selectable they were on the edge of before
    private void OnTriggerStay2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player && !player.highlightedSelectable)
        {
            //select the selectable
            Select(player);
        }
    }
}
