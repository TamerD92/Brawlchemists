using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : BaseController
{
    public float MaxHorizontalSpeed;

    public float currVelocity;

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (currVelocity < 0)
        {
            mainController.isFacingLeft = true;
            mainController.turnGFX();
        }
        else if (currVelocity > 0)
        {
            mainController.isFacingLeft = false;
            mainController.turnGFX();
        }

    }

    private void FixedUpdate()
    {

        mainController.rb.position += new Vector2(currVelocity * Time.deltaTime * MaxHorizontalSpeed, 0);


    }



}
