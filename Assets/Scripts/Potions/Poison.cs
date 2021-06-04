using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : EffectBaseClass
{
    public float time;

    public override void doEffect(PlayerController player)
    {
        player.StartCoroutine(player.damageOverTime(time, damage));
    }

}
