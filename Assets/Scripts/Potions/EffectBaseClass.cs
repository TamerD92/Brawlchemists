using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EffectBaseClass : MonoBehaviour, EffectInterface, IPunInstantiateMagicCallback, IPunObservable
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

    private void Update()
    {
        Duration -= Time.deltaTime;
        if (Duration <= 0)
        {
            GameController.instance.ReturnToPoolBase(this);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(gameObject.activeSelf);
        }
        else
        {
            // Network player, receive data
            gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
