using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VegetableTable : MonoBehaviour
{
    public Text label;

    private Image tableImage;
    public VegetableType type;

    private Color litTableColor = Color.white;
    private Color dimTableColor;

    private int highlightingPlayerCount;

    // Start is called before the first frame update
    void Start()
    {
        //assign tableImage and unselected color on start
        tableImage = GetComponent<Image>();
        dimTableColor = tableImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //vegetable method to assign vegetables type and label
    public void AssignVegetable(string vege)
    {
        type = (VegetableType)System.Enum.Parse(typeof(VegetableType), vege);
        label.text = vege;
    }

    public void SelectTable(Player player)
    {
        //light up the table
        tableImage.color = Color.white;

        highlightingPlayerCount++;

        //replace highlighted table with the new table, dim the old table
        if(player.highlightedVegetable)
        {
            player.highlightedVegetable.DeselectTable(player);
        }
        player.highlightedVegetable = this;
    }

    public void DeselectTable(Player player)
    {        
        //unassign the player's highlighted vegetable, but only if it's from this table
        if (player.highlightedVegetable == this)
        {
            player.highlightedVegetable = null;
            highlightingPlayerCount--;
            if(highlightingPlayerCount == 0)
            {
                //dim the table if this was the only player highlighting it
                tableImage.color = dimTableColor;
            }
        }
    }

    //handle when a player leaves a table
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            //deselect the table
            DeselectTable(player);
        }
    }

    //handle when a player contacts a vegetable table, or re-highlights a table they were on the edge of before
    private void OnTriggerStay2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if(player && !player.highlightedVegetable)
        {
            //select the table
            SelectTable(player);
        }
    }
}
