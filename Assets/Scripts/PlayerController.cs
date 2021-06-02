using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    #region controllers
    public BaseController[] PlayerControllers;

    public bool isJumpingAxisUsed, isPreparing, isThrowing, isPickingUp;
    #endregion


    public SpriteRenderer PlayerGFX;

    public bool IsFrozen, IsPoisoned;
    public float PoisonTime, FreezeTime;

    public List<CaseBaseClass> CaseIngridients;
    public List<EffectBaseClass> EffectIngridients;
    public List<Potion> CurrentPotions;

    public Potion selectedPotion;

    public List<PickupBase> OverlappingPickups;

    public override void Init()
    {
        base.Init();
        OverlappingPickups = new List<PickupBase>();
        
        EffectIngridients = new List<EffectBaseClass>();
        CaseIngridients = new List<CaseBaseClass>();
        CurrentPotions = new List<Potion>();
        for (int i = 0; i < PlayerControllers.Length; i++)
        {
            PlayerControllers[i].reset();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in PlayerControllers)
        {
            if (item is WalkerController)
            {
                (item as WalkerController).currVelocity = Input.GetAxis("Horizontal");    
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 2, Color.black, 100);
        }

        if (Input.GetAxisRaw("Jump") != 0 && !isJumpingAxisUsed)
        {

            foreach (var item in PlayerControllers)
            {
                if (item is JumpingController)
                {
                    (item as JumpingController).Jump();
                }
            }
            isJumpingAxisUsed = true;
        }
        else if(Input.GetAxisRaw("Jump") == 0)
        {
            isJumpingAxisUsed = false;
        }

        if (Input.GetAxis("Fire1") != 0 && !isThrowing && isTouchingFloor)
        {
            isPreparing = true;
            foreach (var item in PlayerControllers)
            {
                if (item is ThrowingController)
                {
                    (item as ThrowingController).PrepareThrow();
                }
            }
            isThrowing = true;
        }
        else if (Input.GetAxis("Fire1") == 0 && isPreparing && CurrentPotions.Count > 0)
        {
            foreach (var item in PlayerControllers)
            {
                if (item is ThrowingController)
                {
                    (item as ThrowingController).Throw();
                }
            }
            isThrowing = false;
            isPreparing = false;
        }

        if (Input.GetAxis("Pickup") != 0 && OverlappingPickups.Count > 0 && !isPickingUp)
        {
            OverlappingPickups[0].Collect(this);
            isPickingUp = true;
        }
        else if (Input.GetAxis("Pickup") == 0)
        {
            isPickingUp = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CreatePotion(0 , 0);
        }

    }

    private void CreatePotion(int Case, int Effect)
    {
        if (CaseIngridients.Count > 0 && EffectIngridients.Count > 0)
        {
            Potion pot = GameController.CreatePotion(CaseIngridients[Case], EffectIngridients[Effect]);
            pot.transform.SetParent(transform);
            pot.transform.localPosition = Vector3.zero;
            CaseIngridients.RemoveAt(0);
            EffectIngridients.RemoveAt(0);
            CurrentPotions.Add(pot);
        }
    }

    private void FixedUpdate()
    {

    }



    public override void FloorTouched()
    {
        base.FloorTouched();
        foreach (var item in PlayerControllers)
        {
            if (item is JumpingController)
            {
                (item as JumpingController).resetJumps();
            }
        }
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
        foreach (var item in PlayerControllers)
        {
            item.enabled = false;
        }
    }

    public void EnableAllControllers()
    {
        foreach (var item in PlayerControllers)
        {
            item.enabled = true;
        }
    }

    public void DisableAllOtherControllers(BaseController controller)
    {
        foreach (var item in PlayerControllers)
        {
            if (item != controller)
            {
                item.enabled = false;
            }
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

    public IEnumerator TemporaryStun(float time)
    {
        FreezeTime = time;
        DisableAllControllers();
        if (!IsFrozen)
        {
            IsFrozen = true;
            while (FreezeTime > 0)
            {
                FreezeTime -= Time.deltaTime;
                yield return null;
            }
            EnableAllControllers();
            IsFrozen = false;
        }

        
    }

    public BaseController GetController(Type controllerType)
    {
        for (int i = 0; i < PlayerControllers.Length; i++)
        {
            if (PlayerControllers[i].GetType() == controllerType)
            {
                return PlayerControllers[i];
            }
        }
        return null;
    }
    #endregion

    public IEnumerator damageOverTime(float time, int damage)
    {
        PoisonTime = time;
        if (!IsPoisoned)
        {
            IsPoisoned = true;
            while (PoisonTime > 0)
            {
                PoisonTime -= Time.deltaTime;
                yield return null;
                if (PoisonTime % 1 == 0)
                {
                    currStats.HP -= damage;
                }
            }
            IsPoisoned = false;
        }
        
    }

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
        if (collision.tag == "Pickup")
        {
            OverlappingPickups.Add(collision.GetComponent<PickupBase>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Pickup")
        {
            OverlappingPickups.Remove(collision.GetComponent<PickupBase>());
        }
    }

    public void CreatePotion(CaseBaseClass Case, EffectBaseClass Effect)
    {
        CurrentPotions.Add(GameController.CreatePotion(Case, Effect));

        CaseIngridients.Remove(Case);
        EffectIngridients.Remove(Effect);

    }
}
