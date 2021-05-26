using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Freeze : EffectBaseClass
{
    public int damage;

    public float time;

    public override void doEffect(PlayerController player)
    {
        player.takeDamage(damage);
        player.StartCoroutine(player.TemporaryStun(time));
    }
}
