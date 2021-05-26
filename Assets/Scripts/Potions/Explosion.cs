using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : EffectBaseClass
{
    public int damage;

    public override void doEffect(PlayerController player)
    {
        player.takeDamage(damage);
    }
}
