using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasePickup : PickupBase
{
    public CaseBaseClass Case;

    public override void Generate()
    {
        int ID = Random.Range(0, GameController.instance.database.CaseTypes.Length);

        Case = GameController.instance.database.CaseTypes[ID];

        //base.Generate();
    }

    public override void Collect(PlayerController player)
    {
        player.CaseIngridients.Add(Case);
        base.Collect(player);
    }
}
