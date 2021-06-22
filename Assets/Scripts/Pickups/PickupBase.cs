using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PickupBase : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
{

    public Sprite image;
    
    [PunRPC]
    public virtual void Generate(int number)
    { 
        
    }

    public virtual void OnlineGenerate()
    { 
        
    }

    public virtual void Collect(PlayerController player)
    {
        
        GameController.instance.ReturnToPoolBase(this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(false);
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(gameObject.activeSelf);
            //stream.SendNext(image);
        }
        else
        {
            // Network player, receive data
            gameObject.SetActive((bool)stream.ReceiveNext());
            //image = (Sprite)stream.ReceiveNext();
        }
    }

 

}
