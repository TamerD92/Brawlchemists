using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public virtual void Generate()
    { 
        
    }

    public virtual void Collect(PlayerController player)
    {
        GameController.instance.ReturnToPool(this);
    }
}
