using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PickupBase : MonoBehaviourPunCallbacks
{

    public Sprite image;
    
    [PunRPC]
    public virtual void Generate(int number)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnlineGenerate()
    {

    }

    public virtual void Collect(PlayerController player)
    {
        
        GameController.instance.ReturnToPoolBase(this);
        turnOff();
    }


    public void turnOff()
    {
        photonView.RPC("onlineTurnOff", RpcTarget.All);
    }

    [PunRPC]
    protected void onlineTurnOff()
    {
        gameObject.SetActive(false);
    }

 

}
