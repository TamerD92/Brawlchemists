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

    public MakeAChoice potionSelect, CaseSelect, EffectSelect;

    int PotionSelector, EffectSelector, CaseSelector;

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
        currStats.HP = baseStats.HP;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

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

        if (Input.GetAxis("Fire1") != 0 && !isThrowing && isTouchingFloor && selectedPotion != null)
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
        else if (Input.GetAxis("Fire1") == 0 && isPreparing)
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
            CreatePotion(CaseSelector , EffectSelector);
        }

    }

    private void CreatePotion(int Case, int Effect)
    {
        if (CaseIngridients.Count > 0 && EffectIngridients.Count > 0)
        {
            Potion pot = GameController.CreatePotion(CaseIngridients[Case], EffectIngridients[Effect]);
            pot.transform.SetParent(transform);
            pot.transform.localPosition = Vector3.zero;
            CaseIngridients.RemoveAt(Case);
            EffectIngridients.RemoveAt(Effect);
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

    public override void GetHit(float power, Vector3 direction)
    {

        DisableAllControllers();

        if (isTouchingFloor)
        {
            direction += (Vector3.up * 0.25f);
        }
        rb.velocity = Vector2.zero;

        float finalPower = power * ((float)baseStats.HP / (float)currStats.HP);

        rb.AddForce(direction.normalized * finalPower, ForceMode2D.Impulse);

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
                    HPChangedEvent.Invoke();
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
        if (collision.tag == "PotionEffect")
        {
            EffectBaseClass effect = collision.gameObject.GetComponent<EffectBaseClass>();

            effect.doEffect(this);

            Vector3 direction = transform.position - collision.transform.position;

            GetHit(effect.damage, direction);
        }

        if (collision.tag == "Pickup")
        {
            OverlappingPickups.Add(collision.GetComponent<PickupBase>());
        }

        if (collision.tag == "DeadZone")
        {
            ObjectDiedEvent.Invoke();
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

    public int getSelector(ListType list)
    {
        switch (list)
        {
            case ListType.Potion:
                return PotionSelector;
            case ListType.Case:
                return CaseSelector;
            case ListType.Effect:
                return EffectSelector;
            default:
                return -1;
        }
    }

    public int getListCount(ListType list)
    {
        switch (list)
        {
            case ListType.Potion:
                return CurrentPotions.Count;
            case ListType.Case:
                return CaseIngridients.Count;
            case ListType.Effect:
                return EffectIngridients.Count;
            default:
                return -1;
        }

    }

    public void setSelector(ListType list, int newNum)
    {
        switch (list)
        {
            case ListType.Potion:
                PotionSelector = newNum;
                break;
            case ListType.Case:
                CaseSelector = newNum;
                break;
            case ListType.Effect:
                EffectSelector = newNum;
                break;
            default:
                break;
        }
    }

    public string increaseSelector(ListType list)
    {
        int currSelect = getSelector(list);

        if (currSelect >= getListCount(list) - 1)
            setSelector(list, 0);
        else
            setSelector(list, currSelect + 1);



        switch (list)
        {
            case ListType.Potion:
                if (CurrentPotions.Count > 0)
                {
                    selectedPotion = CurrentPotions[PotionSelector];
                }
                return CurrentPotions[getSelector(list)].name;
            case ListType.Case:
                return CaseIngridients[getSelector(list)].name;
            case ListType.Effect:
                return EffectIngridients[getSelector(list)].name;
            default:
                return "";
        }

    }

    public string decreaseSelector(ListType list)
    {
        int currSelect = getSelector(list);

        if (currSelect <= 0)
            setSelector(list, getListCount(list) - 1);
        else
            setSelector(list, 0);

        switch (list)
        {
            case ListType.Potion:
                if (CurrentPotions.Count > 0)
                {
                    selectedPotion = CurrentPotions[PotionSelector];
                }
                return CurrentPotions[getSelector(list)].name;
            case ListType.Case:
                return CaseIngridients[getSelector(list)].name;
            case ListType.Effect:
                return EffectIngridients[getSelector(list)].name;
            default:
                return "";
        }

    }
}
