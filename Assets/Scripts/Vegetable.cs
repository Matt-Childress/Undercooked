using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vegetable : MonoBehaviour
{
    public Text label;

    private Vegetables type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //vegetable method to assign vegetables type and label
    public void AssignVegetable(string vege)
    {
        type = (Vegetables)System.Enum.Parse(typeof(Vegetables), vege);
        label.text = vege;
    }
}
