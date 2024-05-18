using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
//WIP Class for a new status effect system
public abstract class S_StatusApplier : ScriptableObject
{
    public abstract void ApplyStatusBeforeTurn(O_BattleCharacter target);
    public abstract void ApplyStatusAfterTurn(O_BattleCharacter target);
}
