using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalljumpController : BaseController
{
    //Can only be 1 pr -1
    public float WallDir;

    public int jumpAmount;
    [SerializeField]
    private int currJumpAmount;
    public float InitialJumpVerticalSpeed;
    public float InitialHorizontalSpeedJumpSpeed;

    public void Jump()
    {
        if (currJumpAmount > 0)
        {
            reduceJumps();


            mainController.rb.velocity = new Vector2(0, 0);
            mainController.rb.AddForce(new Vector2(InitialHorizontalSpeedJumpSpeed *-WallDir, InitialJumpVerticalSpeed * mainController.rb.gravityScale), ForceMode2D.Impulse);
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
