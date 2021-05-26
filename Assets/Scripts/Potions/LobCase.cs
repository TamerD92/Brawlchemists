using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalLob", menuName = "Cases/Lob", order = 1)]
public class LobCase : CaseBaseClass
{
    public float maxStrength;

    [SerializeField]
    public new float MaxStrength { get => maxStrength; set => maxStrength = value; }
}
