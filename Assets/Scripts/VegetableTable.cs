using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VegetableTable : Selectable
{
    public Text label;
    public VegetableType type;

    //vegetable method to assign vegetables type and label
    public void AssignVegetable(string vege)
    {
        type = (VegetableType)System.Enum.Parse(typeof(VegetableType), vege);
        label.text = vege;
    }
}
