using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public Sprite image;
    public virtual void Generate()
    { 
        
    }

    [PunRPC]
    public virtual void Collect(PlayerController player)
    {
        GameController.instance.ReturnToPool(this);
    }
}
