using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBlock : Selectable
{
    //time it takes to chop
    public float choppingWaitTime = 3f;

    //text to display the salad combination being chopped
    public Text chopText;

    //hold the current salad combination
    public Salad heldSalad;

    public bool ValidChop(Salad saladBeingPlaced)
    {
        //check if a salad can be placed on a chopping block or not
        if(saladBeingPlaced != null && !saladBeingPlaced.isFinished) //not valid if the salad has been picked up from the board (finished)
        {
            if(heldSalad != null || saladBeingPlaced.newVegetable) //valid if the salad is being combined into an existing salad, or it is a new vegetable
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveSalad()
    {
        heldSalad = null;
        UpdateHeldSaladUI();
    }

    public void StartChop(Player player, Salad salad)
    {
        //lock the player
        player.HandlePlayerLock(true);

        //drop the first salad (passing it to the chop block)
        player.DropSalad(salad);

        //method starts a Coroutine to display the salad is chopping
        StartCoroutine(ChoppingRoutine(player, salad));
    }

    private IEnumerator ChoppingRoutine(Player player, Salad salad)
    {
        float timer = 0f; //timer to check when the chopping loop should exit
        float timeStep = 0.25f; //how often loop restarts to update chopText

        //combine the new ingredients with the current held salad combination if there is one, or create a new held salad
        if (heldSalad != null)
        {
            heldSalad.CombineIntoSalad(salad.vegetableCombination);
        }
        else
        {
            heldSalad = salad;
            heldSalad.newVegetable = false;
            heldSalad.isFinished = true;
        }

        //make 4 strings with an additional period on each one, to cycle through and let the player know a process is happening
        string saladText0 = heldSalad.GetSaladText() + "\n"; //put the elipses on a new line
        string saladText1 = saladText0 + ".";
        string saladText2 = saladText1 + ".";
        string saladText3 = saladText2 + ".";

        //while the player is chopping, update the text
        while (timer < choppingWaitTime)
        {
            if (chopText.text.Equals(saladText0))
            {
                chopText.text = saladText1;
            }
            else if (chopText.text.Equals(saladText1))
            {
                chopText.text = saladText2;
            }
            else if (chopText.text.Equals(saladText2))
            {
                chopText.text = saladText3;
            }
            else
            {
                chopText.text = saladText0;
            }

            yield return new WaitForSeconds(timeStep);
            timer += timeStep;
        }

        //perform finished chopping actions
        EndChop(player);
    }

    private void EndChop(Player player)
    {
        //display correct chopped salad text
        UpdateHeldSaladUI();

        //unlock the player
        player.HandlePlayerLock(false);
    }

    private void UpdateHeldSaladUI()
    {
        //updating chopBlock's label
        chopText.text = heldSalad != null ? heldSalad.GetSaladText() : string.Empty;
    }
}
