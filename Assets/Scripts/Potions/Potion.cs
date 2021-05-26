using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    GameObject Colliders;

    public EffectInterface effect;

    public CaseInterface Case;

    public float currStrength, Angle;

    public int BounceAmount;

    public Rigidbody2D rb;

    public PhysicsMaterial2D Mat;

    public void init()
    {
        if (Case is BounceCase)
        {
            BounceAmount = (Case as BounceCase).BounceAmount;
            Mat = (Case as BounceCase).bounceMat;
        }
        else
        {
            BounceAmount = 1;
        }
    }

    public void Launch(Rigidbody2D mainObject)
    {
        Vector2 throwVector = new Vector2(0, currStrength);

        throwVector = Rotate(throwVector, Angle);

        mainObject.AddForce(throwVector, ForceMode2D.Impulse);
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public void Detonate()
    { 
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BounceAmount--;
        if (BounceAmount == 0 || collision.transform.tag == "Player")
        {
            BounceAmount = 0;
            Detonate();
        }
    }
}
