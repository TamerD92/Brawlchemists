using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectPickup : PickupBase
{
    public EffectBaseClass Effect;

    public override void Generate()
    {
        int ID = Random.Range(0, GameController.instance.database.EffectsTypes.Length);

        Effect = GameController.instance.database.EffectsTypes.First(o => o.ID == ID);

        base.Generate();
    }

    public override void Collect(PlayerController player)
    {
        Debug.LogError("Player " + player.name + " Picked up pickup: " + Effect.name);

        player.EffectIngridients.Add(Effect);
        base.Collect(player);
    }
}
