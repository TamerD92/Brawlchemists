using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bounce", menuName = "Cases/Bounce", order = 1)]
public class BounceCase : CaseBaseClass
{
    public float maxStrength;

    public PhysicsMaterial2D bounceMat;

    public int BounceAmount;

    [SerializeField]
    public new float MaxStrength { get => maxStrength; set => maxStrength = value; }

}
