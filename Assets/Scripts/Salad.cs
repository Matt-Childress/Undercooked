using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Salad
{
    public List<VegetableType> vegetableCombination; //holds the combination of vegetables in the salad
    public bool isChopped; //tracks if the salad has been picked up from the chopping board

    private Color unchoppedColor = new Color(192f/255f, 96f/255f, 0f/255f);
    private Color choppedColor = new Color(72f/255f, 16f/255f, 120f/255f);

    public Salad(List<VegetableType> vegTypeCombo, bool chopped)
    {
        //constructor for new Salad object
        vegetableCombination = vegTypeCombo;
        isChopped = chopped;
    }

    public void CombineIntoSalad(List<VegetableType> newIngredients)
    {
        //combines new ingredeints into an existing salad (used on chopping blocks)
        List<VegetableType> newCombo = vegetableCombination.Union(newIngredients).ToList<VegetableType>();
        vegetableCombination = newCombo;
    }

    public string GetSaladText()
    {
        //return a string that reflects the vegetable combination
        string comboString = string.Empty;
        for(int i = 0; i < vegetableCombination.Count; i++)
        {
            if (i == vegetableCombination.Count - 1)
            {
                comboString += vegetableCombination[i].ToString();
            }
            else
            {
                comboString += vegetableCombination[i].ToString() + ",\n";
            }
        }
        return comboString;
    }

    public Color GetSaladColor()
    {
        //return purple if chopped, orange if unchopped
        return isChopped ? choppedColor : unchoppedColor;
    }
}
