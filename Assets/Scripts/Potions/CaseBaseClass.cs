using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseBaseClass : ScriptableObject, CaseInterface
{
    public float maxStrength;

    public float MaxStrength { get => maxStrength; set => maxStrength = value; }
}
