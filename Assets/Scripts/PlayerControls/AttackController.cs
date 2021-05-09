using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : BaseController
{
    public GameObject attackHitbox;

    public bool isAttacking;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            if (mainController.isTouchingFloor)
            {
                StartCoroutine(AttackGround());

            }
            else
            {
                StartCoroutine(AttackAir());
            }
        }
    }

    public IEnumerator AttackGround()
    {
        mainController.DisableAllControllers();
        attackHitbox.SetActive(true);
        isAttacking = true;

        mainController.rb.velocity = Vector2.zero;
        mainController.rb.AddForce(mainController.returnForward() * 3, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        mainController.rb.gravityScale = GameController.GRAVITY_SCALE;

        mainController.EnableAllControllers();
        attackHitbox.SetActive(false);
        isAttacking = false;
    }

    public IEnumerator AttackAir()
    {
        mainController.DisableAllControllers();
        attackHitbox.SetActive(true);
        isAttacking = true;

        mainController.rb.velocity = Vector2.zero;
        mainController.rb.gravityScale = 0;
        mainController.rb.AddForce(mainController.returnForward(), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        mainController.rb.gravityScale = GameController.GRAVITY_SCALE;

        mainController.EnableAllControllers();
        attackHitbox.SetActive(false);

        while (!mainController.isTouchingFloor)
        {
            yield return null;
        }
        isAttacking = false;
    }

    public string getParentTag()
    {
        return mainController.tag;
    }
}
