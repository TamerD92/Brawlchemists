using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingController : BaseController
{
    public int jumpAmount;
    [SerializeField]
    private int currJumpAmount;
    public float InitialJumpSpeed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    

    private void FixedUpdate()
    {


    }

    public void Jump()
    {
        if (currJumpAmount > 0)
        {
            reduceJumps();


            mainController.rb.velocity = new Vector2(mainController.rb.velocity.x, 0);
            mainController.rb.AddForce(new Vector2(0, InitialJumpSpeed * mainController.rb.gravityScale), ForceMode2D.Impulse);
        }
    }

    public void reduceJumps()
    {
        currJumpAmount--;
    }

    public void resetJumps()
    {
        currJumpAmount = jumpAmount;
    }

    
}
