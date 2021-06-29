using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Potion : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
{
    public EffectBaseClass effect;

    public CaseBaseClass Case;

    public Collider2D collider;

    public float currStrength;

    public int BounceAmount;

    public Rigidbody2D rb;

    public PhysicsMaterial2D Mat;

    public int playerID;

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
        effect.transform.SetParent(null);

        effect.gameObject.SetActive(true);

        effect.transform.rotation = Quaternion.identity;

        GameController.instance.ReturnToPoolBase(this);

        if (photonView.IsMine)
        {
            DeployPotion(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        BounceAmount--;
        if (BounceAmount <= 0 || collision.transform.tag == "Player")
        {
            if (collision.transform.tag == "Player")
            {
                if (collision.gameObject.GetComponent<PlayerController>().ID == playerID)
                {
                    return;
                }
            }
            BounceAmount = 0;
            photonView.RPC("Detonate", RpcTarget.All);
            
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(false);
    }

    public void DeployPotion(bool active)
    {
        photonView.RPC("onlineDeployPotion", RpcTarget.All, active);
    }

    [PunRPC]
    private void onlineDeployPotion(bool active)
    {
        gameObject.SetActive(active);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(effect.ID);
            stream.SendNext(rb.gravityScale);
            stream.SendNext(collider.enabled);
            stream.SendNext(playerID);
        }
        else
        {
            // Network player, receive data

            int effID = ((int)stream.ReceiveNext());

            if (!effect)
            {
                Debug.Log("Starting effect deserealization");
                EffectBaseClass syncEffect = GameController.instance.EffectsPool.Where(o => o.ID == effID).ToList()[0];
                GameController.instance.EffectsPool.Remove(syncEffect);
                
                syncEffect.transform.SetParent(transform);
                syncEffect.transform.localPosition = Vector3.zero;
                effect = syncEffect;
            }

            rb.gravityScale = (float)stream.ReceiveNext();

            collider.enabled = (bool)stream.ReceiveNext();

            playerID = (int)stream.ReceiveNext();

        }
    }
}
