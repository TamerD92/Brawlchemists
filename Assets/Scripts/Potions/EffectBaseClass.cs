using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EffectBaseClass : MonoBehaviourPunCallbacks, EffectInterface, IPunInstantiateMagicCallback
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

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    protected void onlineTurnOff()
    {
        Duration = 0;
        GameController.instance.ReturnToPoolBase(this);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Duration -= Time.deltaTime;
            if (Duration <= 0)
            {
                photonView.RPC("onlineTurnOff", RpcTarget.All);
            }
        }
    }
}
