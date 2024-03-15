using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move")]
public class S_Move : S_Ability
{
    public int power;
    public bool fixedValue = false;
    public bool targetDead = false;
    public enum TARGET_SCOPE { SINGLE, ALL, AOE, RANDOM }
    public enum FACTION_SCOPE { FOES, ALLY, ALL }
    public TARGET_SCOPE targetScope = TARGET_SCOPE.SINGLE;
    public FACTION_SCOPE factionScope = FACTION_SCOPE.FOES;
    public S_Element element;
    public S_Stats statRequirments;
    public S_Stats_Util cost;
}
