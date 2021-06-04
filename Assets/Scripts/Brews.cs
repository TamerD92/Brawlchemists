using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brews : MonoBehaviour
{

    public bool isBrewing;

    public GameObject brewMats;
    public GameObject potion;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isBrewing = !isBrewing;

            brewMats.SetActive(isBrewing);
            potion.SetActive(!isBrewing);
        }
    }
}
