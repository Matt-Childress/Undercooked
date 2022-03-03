using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBlock : Selectable
{
    //time it takes to chop a vegetable
    public float choppingWaitTime = 3f;

    //text to display the vegetable being chopped
    public Text chopText;

    public void StartChop(Player player, Vegetable vege)
    {
        //lock the player
        player.HandlePlayerLock(true);

        //drop the first vegetable (passing it to the chop block)
        player.DropVegetable(vege);

        //method starts a Coroutine to display vegetable is chopping
        StartCoroutine(ChoppingRoutine(player, vege));
    }

    private IEnumerator ChoppingRoutine(Player player, Vegetable vege)
    {
        float timer = 0f; //timer to check when the chopping loop should exit
        float timeStep = 0.25f; //how often loop restarts to update chopText

        //make 4 strings with an additional period on each one, to cycle through and let the player know a process is happening
        string originVegeText = vege.type.ToString() + "\n"; //put the elipses on a new line
        string vegeText1 = originVegeText + ".";
        string vegeText2 = vegeText1 + ".";
        string vegeText3 = vegeText2 + ".";

        //while the player is chopping, update the text
        while (timer < choppingWaitTime)
        {
            if (chopText.text.Equals(originVegeText))
            {
                chopText.text = vegeText1;
            }
            else if (chopText.text.Equals(vegeText1))
            {
                chopText.text = vegeText2;
            }
            else if (chopText.text.Equals(vegeText2))
            {
                chopText.text = vegeText3;
            }
            else
            {
                chopText.text = originVegeText;
            }

            yield return new WaitForSeconds(timeStep);
            timer += timeStep;
        }

        //perform finished chopping actions
        EndChop(player, vege.type);
    }

    private void EndChop(Player player, VegetableType vType)
    {
        //clear the chopping block's text label
        chopText.text = string.Empty;

        //player should pickup the new chopped vegetable
        player.PickupVegetable(vType, true);

        //unlock the player
        player.HandlePlayerLock(false);
    }
}
