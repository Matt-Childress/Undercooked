﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Salad
{
    public List<VegetableType> vegetableCombination; //holds the combination of vegetables in the salad
    public bool newVegetable; //tracks if this is newly picked up from a vegetable table
    public bool isFinished; //tracks if the salad has been picked up from the chopping board

    public Salad(List<VegetableType> vegTypeCombo, bool newVege, bool finished)
    {
        //constructor for new Salad object
        vegetableCombination = vegTypeCombo;
        newVegetable = newVege;
        isFinished = finished;
    }

    public void CombineIntoSalad(List<VegetableType> newIngredients)
    {
        //combines new ingredeints into an existing salad (used on chopping blocks)
        List<VegetableType> newCombo = vegetableCombination.Union(newIngredients).ToList<VegetableType>();
        vegetableCombination = newCombo;
    }

    public string GetSaladText()
    {
        //return a string that reflects the vegetables and if it is a newly gathered vege
        if(newVegetable)
        {
            return vegetableCombination[0].ToString() + "\nVegetable";
        }
        else
        {
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
    }
}
