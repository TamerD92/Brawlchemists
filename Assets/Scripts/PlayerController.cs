using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public WalkerController playerWalk;
    public JumpingController playerJump;
    public DashController playerDash;
    public AttackController playerAttack;
    public WalljumpController playerWallJump;


    public bool isJumpingAxisUsed, isAttackAxisUsed, isDashAxisUsed;

    public SpriteRenderer PlayerGFX;

    public override void Init()
    {
        base.Init();
        playerJump.resetJumps();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerWalk.currVelocity = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 2, Color.black, 100);

        }

        if (Input.GetAxisRaw("Jump") != 0 && !isJumpingAxisUsed)
        {
            if (isTouchingWall && !isTouchingFloor)
            {
                playerWallJump.Jump();
                //DisableController(playerWalk);
                //Invoke("EnableAllControllers", 0.3f);
            }
            else
            {
                playerJump.Jump();
            }
            isJumpingAxisUsed = true;
        }
        else if(Input.GetAxisRaw("Jump") == 0)
        {
            isJumpingAxisUsed = false;
        }

        if (Input.GetAxis("Fire1") != 0 && !isAttackAxisUsed)
        {
            playerAttack.Attack();
            isAttackAxisUsed = true;
        }
        else if (Input.GetAxis("Fire1") == 0)
        {
            isAttackAxisUsed = false;
        }

        if (Input.GetAxis("Dash") != 0 && !isDashAxisUsed)
        {
            playerDash.startDash();
            isDashAxisUsed = true;
        }
        else if (Input.GetAxis("Dash") == 0)
        {
            isDashAxisUsed = false;
        }
    }

    private void FixedUpdate()
    {

    }



    public override void FloorTouched()
    {
        base.FloorTouched();
        playerJump.resetJumps();
        playerDash.resetDashes();
    }

    public override void WallTouched()
    {
        base.WallTouched();
        playerWallJump.resetJumps();
        
    }

    public override void SetWallDir(float dir)
    {
        playerWallJump.WallDir = dir;
    }

    public void turnGFX()
    {
        PlayerGFX.flipX = isFacingLeft;
    
    }

    public override void GetHit()
    {

        base.GetHit();
        DisableAllControllers();
        Vector2 hitVector = returnForward() * -1;

        if (isTouchingFloor)
        {
            hitVector += (Vector2.up * 0.25f);
        }
        rb.velocity = Vector2.zero;
        rb.AddForce(hitVector.normalized, ForceMode2D.Impulse);

        Invoke("EnableAllControllers", 0.25f);
    }

    #region controller management
    public void DisableAllControllers()
    {
        foreach (var item in GetComponents<BaseController>())
        {
            item.enabled = false;
        }
    }

    public void EnableAllControllers()
    {
        foreach (var item in GetComponents<BaseController>())
        {
            item.enabled = true;
        }
    }

    public void DisableController(BaseController controller)
    {
        controller.enabled = false;
    }

    public void EnableController(BaseController controller)
    {
        controller.enabled = true;
    }
    #endregion

    public Vector2 returnForward()
    {
        if (isFacingLeft)
        {
            return Vector2.left;
        }
         return Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack")
        {
            if (collision.GetComponent<AttackController>().getParentTag() == "Enemy")
            {
                GotHitEvent.Invoke();
            }
        }
    }
}
