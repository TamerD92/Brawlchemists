using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectPickup : PickupBase
{
    public EffectBaseClass Effect;

    public override void Generate()
    {
        int ID = Random.Range(0, GameController.instance.EffectsTypes.Length);

        Effect = GameController.instance.EffectsTypes.First(o => o.ID == ID);

        base.Generate();
    }

    public override void Collect(PlayerController player)
    {
        player.EffectIngridients.Add(Effect);
        base.Collect(player);
    }
}
