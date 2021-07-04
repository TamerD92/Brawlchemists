using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "Database", order = 1)]
public class ObjectDatabase : ScriptableObject
{
    public EffectBaseClass[] EffectsTypes;

    public CaseBaseClass[] CaseTypes;

    public PickupBase[] PickupTypes;

    public Sprite FullPotion, PotionHollow, PotionFilling;

    public Sprite[] PlayerSprites;
}
