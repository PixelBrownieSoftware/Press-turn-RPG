using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Move : ScriptableObject
{
    public int power;
    public enum TARGET_SCOPE { SINGLE, ALL, AOE, RANDOM }
    public S_Element element;
    public S_Stats statRequirments;
}
