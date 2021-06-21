using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBaseClass : MonoBehaviour, EffectInterface
{
    public int ID;

    public Collider2D[] baseCollider;

    public float Duration;

    public int damage;

    public Sprite Image;

    public virtual void doEffect(PlayerController player)
    {
        //throw new System.NotImplementedException();
    }

    private void Update()
    {
        Duration -= Time.deltaTime;
        if (Duration <= 0)
        {
            GameController.instance.ReturnToPool(this);
        }
    }
}
