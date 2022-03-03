using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable
{
    public VegetableType type;
    public bool isChopped;

    public Vegetable(VegetableType vegType, bool chopped)
    {
        type = vegType;
        isChopped = chopped;
    }

    public string GetVegetableText()
    {
        //return a string that reflects the vegetable and chopped status
        string choppedString = "Chopped\n";
        return isChopped ? choppedString + type.ToString() : type.ToString();
    }
}
