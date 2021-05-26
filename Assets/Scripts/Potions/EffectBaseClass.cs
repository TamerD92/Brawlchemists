using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBaseClass : MonoBehaviour, EffectInterface
{
    public virtual void doEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
