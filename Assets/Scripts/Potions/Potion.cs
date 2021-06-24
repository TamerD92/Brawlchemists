using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Potion : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
{
    public EffectBaseClass effect;

    public CaseBaseClass Case;

    public Collider2D collider;

    public float currStrength;

    public int BounceAmount;

    public Rigidbody2D rb;

    public PhysicsMaterial2D Mat;

    public void preInit()
    {
        collider.enabled = false;
    }

    public void init()
    {
        rb.gravityScale = 0;

        if (Case is BounceCase)
        {
            BounceAmount = (Case as BounceCase).BounceAmount;
            Mat = (Case as BounceCase).bounceMat;
        }
        else
        {
            BounceAmount = 1;
        }
        effect.gameObject.SetActive(false);
    }

    public void Launch(float Strength, float Angle)
    {
        transform.SetParent(null);

        collider.enabled = true;

        Vector2 throwVector = new Vector2(Strength, 0);

        throwVector = Rotate(throwVector, Angle);

        rb.gravityScale = 1;
        rb.AddForce(throwVector, ForceMode2D.Impulse);
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

    [PunRPC]
    public void Detonate()
    {
        if (photonView.IsMine)
        {

            effect.transform.SetParent(null);

            effect.gameObject.SetActive(true);

            effect.transform.rotation = Quaternion.identity;
        }

        GameController.instance.ReturnToPoolBase(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        BounceAmount--;
        if (BounceAmount <= 0 || collision.transform.tag == "Player")
        {
            BounceAmount = 0;
            photonView.RPC("Detonate", RpcTarget.All);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(effect);
        }
        else
        {
            // Network player, receive data
            gameObject.SetActive((bool)stream.ReceiveNext());
            effect = (EffectBaseClass)stream.ReceiveNext();
        }
    }
}
