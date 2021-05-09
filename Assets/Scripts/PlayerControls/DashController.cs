using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : BaseController
{
    public float DashStrength;
    public float DashDuration;
    public int DashAmount;
    [SerializeField]
    private int currDashAmount;

    public float resetWaitTime;
    private bool isReloading;
    [SerializeField]
    private float resetTimer;

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

    public void startDash()
    {
        if (currDashAmount > 0)
        {
            currDashAmount--;
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Dash()
    {
        mainController.DisableAllControllers();

        mainController.rb.gravityScale = 0;

        Vector2 dir = mainController.isFacingLeft ? Vector2.left : Vector2.right;

        mainController.rb.velocity = Vector2.zero;
        mainController.rb.AddForce(dir * DashStrength , ForceMode2D.Impulse);

        yield return new WaitForSeconds(DashDuration);

        mainController.rb.gravityScale = GameController.GRAVITY_SCALE;

        mainController.rb.velocity = Vector2.zero;

        mainController.EnableAllControllers();

        if (!isReloading)
        {
            StartCoroutine(reloadDashes());
        }
        else
        {
            resetTimer = 0;
        }

    }

    public IEnumerator reloadDashes()
    {
        isReloading = true;

        for (resetTimer = 0; resetTimer < resetWaitTime; resetTimer += Time.deltaTime)
        {
            yield return null;
        }

        resetDashes();
        
    }


    public void resetDashes()
    {
        currDashAmount = DashAmount;
        isReloading = false;
    }
}
