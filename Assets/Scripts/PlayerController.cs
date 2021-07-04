using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : CharacterController, IPunObservable
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

    public bool isBrewing;

    public ObjectDatabase database;

    public int ID, imageID;

    public override void Init()
    {
        base.Init();

        if (Camera != null)
        {
            if (photonView.IsMine)
            {
                Camera.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

        OverlappingPickups = new List<PickupBase>();
        
        EffectIngridients = new List<EffectBaseClass>();
        CaseIngridients = new List<CaseBaseClass>();
        CurrentPotions = new List<Potion>();
        for (int i = 0; i < PlayerControllers.Length; i++)
        {
            PlayerControllers[i].reset();
        }

        PlayerGFX.sprite = database.PlayerSprites[imageID];
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
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

        if (Input.GetAxis("Fire1") != 0 && !isThrowing && selectedPotion != null)
        {
            selectedPotion.DeployPotion(true);
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
            potionSelect.resetIngridiants();
            isThrowing = false;
            isPreparing = false;
        }

        if (Input.GetAxis("Pickup") != 0 && OverlappingPickups.Count > 0 && !isPickingUp)
        {
            CollectPickup();
            isPickingUp = true;
        }
        else if (Input.GetAxis("Pickup") == 0)
        {
            isPickingUp = false;
        }

        if (Input.GetKeyDown(KeyCode.G) && isBrewing)
        {
            CreatePotion(CaseSelector , EffectSelector);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isBrewing = !isBrewing;

            EffectSelect.gameObject.SetActive(isBrewing);
            CaseSelect.gameObject.SetActive(isBrewing);
            potionSelect.gameObject.SetActive(!isBrewing);

            if (isBrewing)
                DisableAllControllers();
            else
                EnableAllControllers();
        }

    }

    private void CollectPickup()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        OverlappingPickups[0].Collect(this);
    }

    private void CreatePotion(int Case, int Effect)
    {
        if (CaseIngridients.Count > 0 && EffectIngridients.Count > 0)
        {
            Potion pot;
            pot = GameController.CreatePotion(CaseIngridients[Case], EffectIngridients[Effect]);
            pot.playerID = ID;
            pot.transform.SetParent(transform);
            pot.transform.localPosition = Vector3.zero;
            CaseIngridients.RemoveAt(Case);
            EffectIngridients.RemoveAt(Effect);
            CurrentPotions.Add(pot);
            EffectSelect.resetIngridiants();
            CaseSelect.resetIngridiants();
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

        if (!IsFrozen)
        {
            Invoke("EnableAllControllers", 0.25f);

        }
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
            PlayerGFX.color = Color.blue;
            while (FreezeTime > 0)
            {
                FreezeTime -= Time.deltaTime;
                yield return null;
            }
            EnableAllControllers();
            PlayerGFX.color = Color.white;
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
            PlayerGFX.color = Color.green;
            IsPoisoned = true;
            while (PoisonTime > 0)
            {
                yield return new WaitForSeconds(1);
                    currStats.HP -= damage;
                    HPChangedEvent.Invoke();
                PoisonTime--;
            }
            PlayerGFX.color = Color.white;
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
        if (!photonView.IsMine)
        {
            return;
        }

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
        if (!photonView.IsMine)
        {
            return;
        }

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

    public PictureStruct increaseSelector(ListType list)
    {
        PictureStruct picStruct = new PictureStruct();

        int currSelect = getSelector(list);

        if (currSelect >= getListCount(list) - 1)
            setSelector(list, 0);
        else
            setSelector(list, currSelect + 1);


        if (getListCount(list) > 0)
        {
            switch (list)
            {
                case ListType.Potion:
                    if (CurrentPotions.Count > 0)
                    {
                        selectedPotion = CurrentPotions[PotionSelector];
                    }
                    picStruct.image = database.PotionHollow;
                    picStruct.image2 = database.PotionFilling;
                    picStruct.color = CurrentPotions[getSelector(list)].MainSprite.color;
                    picStruct.color2 = CurrentPotions[getSelector(list)].FillingSprite.color;
                    return picStruct;
                case ListType.Case:
                    picStruct.image = CaseIngridients[getSelector(list)].image;
                    return picStruct;
                case ListType.Effect:
                    picStruct.image = EffectIngridients[getSelector(list)].Image;
                    return picStruct;
                default:
                    return picStruct;
            }
        }
        return picStruct;

    }

    public PictureStruct decreaseSelector(ListType list)
    {
        PictureStruct picStruct = new PictureStruct();

        int currSelect = getSelector(list);

        if (currSelect <= 0)
            setSelector(list, getListCount(list) - 1);
        else
            setSelector(list, 0);

        if (getListCount(list) > 0)
        {
            switch (list)
            {
                case ListType.Potion:
                    if (CurrentPotions.Count > 0)
                    {
                        selectedPotion = CurrentPotions[PotionSelector];
                    }
                    picStruct.image = database.PotionHollow;
                    picStruct.image2 = database.PotionFilling;
                    picStruct.color = CurrentPotions[getSelector(list)].MainSprite.color;
                    picStruct.color2 = CurrentPotions[getSelector(list)].FillingSprite.color;
                    return picStruct;
                case ListType.Case:
                    picStruct.image = CaseIngridients[getSelector(list)].image;
                    return picStruct;
                case ListType.Effect:
                    picStruct.image = EffectIngridients[getSelector(list)].Image;
                    return picStruct;
                default:
                    return picStruct;
            }
        }
        return picStruct;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isFacingLeft);
            stream.SendNext(IsPoisoned);
            stream.SendNext(IsFrozen);
            stream.SendNext(imageID);
            }
            else
            {
            // Network player, receive data
            isFacingLeft = (bool)stream.ReceiveNext();
            turnGFX();

            PlayerGFX.color = (bool)stream.ReceiveNext() ? Color.green : Color.white;
            PlayerGFX.color = (bool)stream.ReceiveNext() ? Color.blue : Color.white;
            PlayerGFX.sprite = database.PlayerSprites[(int)stream.ReceiveNext()];
        }

       
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        gameObject.SetActive(false);
    }
}
