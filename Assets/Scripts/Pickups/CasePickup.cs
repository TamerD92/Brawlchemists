using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CasePickup : PickupBase
{
    public CaseBaseClass Case;


    [PunRPC]
    public override void Generate(int number)
    {
        base.Generate(number);
        Case = GameController.instance.database.CaseTypes[number];
        image = Case.image;
        spriteRenderer.sprite = image;
    }

    public override void OnlineGenerate()
    {
        int ID = Random.Range(0, GameController.instance.database.CaseTypes.Length);

        photonView.RPC("Generate", RpcTarget.AllBufferedViaServer, ID);
    }

    public override void Collect(PlayerController player)
    {
        player.CaseIngridients.Add(Case);
        base.Collect(player);
    }
}
