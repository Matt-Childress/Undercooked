using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : Selectable
{
    //text to display the vegetable being held
    public Text plateText;

    //hold the current vegetable
    public Salad heldVege;

    public bool ValidVegetable(Salad salad)
    {
        //only new vegetables can be put on plates, and only if the plate is empty
        return heldVege == null && salad.newVegetable;
    }

    public void HoldVegetable(Salad salad)
    {
        //receive the salad from the player
        heldVege = salad;
        UpdateHeldVegetableUI();
    }

    public void RemoveSalad()
    {
        heldVege = null;
        UpdateHeldVegetableUI();
    }

    private void UpdateHeldVegetableUI()
    {
        //updating plate's label
        plateText.text = heldVege != null ? heldVege.GetSaladText() : string.Empty;
    }
}
