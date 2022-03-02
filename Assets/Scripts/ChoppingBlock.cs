using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBlock : MonoBehaviour
{
    private Image blockImage;

    private Color litBlockColor = Color.white;
    private Color dimBlockColor;

    private int highlightingPlayerCount;

    // Start is called before the first frame update
    void Start()
    {
        //assign blockImage and unselected color on start
        blockImage = GetComponent<Image>();
        dimBlockColor = blockImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBlock(Player player)
    {
        //light up the block
        blockImage.color = Color.white;

        highlightingPlayerCount++;

        //replace highlighted block with the new block, dim the old block
        if (player.highlightedChopBlock)
        {
            player.highlightedChopBlock.DeselectBlock(player);
        }
        player.highlightedChopBlock = this;
    }

    public void DeselectBlock(Player player)
    {
        //unassign the player's highlighted chopping block, but only if it's this block
        if (player.highlightedChopBlock == this)
        {
            player.highlightedChopBlock = null;
            highlightingPlayerCount--;
            if (highlightingPlayerCount == 0)
            {
                //dim the block if this was the only player highlighting it
                blockImage.color = dimBlockColor;
            }
        }
    }

    //handle when a player leaves a chopping block
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            //deselect the block
            DeselectBlock(player);
        }
    }

    //handle when a player contacts a chopping block, or re-highlights a block they were on the edge of before
    private void OnTriggerStay2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player && !player.highlightedChopBlock)
        {
            //select the block
            SelectBlock(player);
        }
    }
}
