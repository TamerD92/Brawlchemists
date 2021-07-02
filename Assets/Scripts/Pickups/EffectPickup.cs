using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class EffectPickup : PickupBase
{

    public EffectBaseClass Effect;
    
    [PunRPC]
    public override void Generate(int number)
    {
        base.Generate(number);
        Effect = GameController.instance.database.EffectsTypes.First(o => o.ID == number);
        image = Effect.Image;
        spriteRenderer.sprite = image;
    }

    public override void OnlineGenerate()
    {
        int ID = Random.Range(0, GameController.instance.database.EffectsTypes.Length);

        photonView.RPC("Generate", RpcTarget.AllBufferedViaServer, ID);

    }

    public override void Collect(PlayerController player)
    {

        player.EffectIngridients.Add(Effect);
        base.Collect(player);
    }
}
