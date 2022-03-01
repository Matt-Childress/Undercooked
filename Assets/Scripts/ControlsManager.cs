using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("P1Action"))
        {
            Debug.Log("P1Action");
        }

        if (Input.GetButtonDown("P2Action"))
        {
            Debug.Log("P2Action");
        }
    }
}
