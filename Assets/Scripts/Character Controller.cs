using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;

public class CharacterController : MonoBehaviourPunCallbacks
{
    public UnityEvent HPChangedEvent, GotHitEvent, ObjectDiedEvent, OnSpawnEvent, OnDespawnEvent, OnTouchingFloor, OnLeaveFloor, OnTouchWall, OnLeaveWall;


    public Stats baseStats, currStats;
    public float MovementSpeed;
    public List<StatusEffect> Effects;
    public Rigidbody2D rb;

    public LifeBarController LifeBar;

    public bool isTouchingFloor;
    public bool isTouchingWall;
    public bool isFacingLeft;

    public CameraWork Camera;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (isTouchingFloor)
        {
            if (!CheckDownCollision())
            {
                isTouchingFloor = false;
            }

        }
    }

    public void LateUpdate()
    {
        
    }

    protected bool CheckDownCollision()
    {
        List<Collider2D> res = new List<Collider2D>();

        rb.GetAttachedColliders(res);

        int layerMask = 1 << 2;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(res[0].bounds.size.x - 0.001f, res[0].bounds.size.y), 0, Vector2.down, 0.01f, ~layerMask);

        return hit;


    }

    public virtual void Init()
    { }

    public virtual void HPChanged()
    {
        float newRatio = (float)currStats.HP / (float)baseStats.HP;

        LifeBar.SetSize(newRatio);

        if (currStats.HP <= 0)
        {
            ObjectDiedEvent.Invoke();
        }
    }

    public virtual void Spawn(Transform point)
    { }

    public virtual void GetHit(float power, Vector3 direction)
    { 
        
    }

    public virtual void Die()
    { 
        //Add Online logic for character death;
    }

    public virtual void AddStatus(StatusEffect eff)
    { }

    public virtual void FloorTouched()
    {
        isTouchingFloor = true;
    }

    public virtual void WallTouched()
    {
        isTouchingWall = true;
    }

    public virtual void FloorLeft()
    {
        isTouchingFloor = false;
    }

    public virtual void WallLeft()
    {
        isTouchingWall = false;
    }

    public virtual void CommitStatus()
    { }

    public virtual void SetWallDir(float dir)
    { }

    public void takeDamage(int damage)
    {
        currStats.HP -= damage;
        HPChangedEvent.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor" && CheckDownCollision())
        {
            OnTouchingFloor.Invoke();
        }

        if (collision.collider.tag == "Wall")
        {
            Vector2 dir = collision.collider.transform.position - transform.position;
            dir = (dir - new Vector2(0, dir.y)).normalized;
            float num = dir.x;
            SetWallDir(num);
            OnTouchWall.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor" && CheckDownCollision())
        {
            OnLeaveFloor.Invoke();
        }

        if (collision.collider.tag == "Wall")
        {
            Vector2 dir = collision.collider.transform.position - transform.position;
            dir = (dir - new Vector2(0, dir.y)).normalized;
            float num = dir.x;
            SetWallDir(num);
            OnLeaveWall.Invoke();
        }
    }

}
