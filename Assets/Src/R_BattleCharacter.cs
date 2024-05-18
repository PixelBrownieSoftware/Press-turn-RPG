using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Battle character")]
public class R_BattleCharacter : R_Default
{
    public O_BattleCharacter battleCharacter;

    private void OnEnable()
    {
        if(_isReset)
        {
            battleCharacter = null;
        }
    }

    public void SetCharacter(O_BattleCharacter bcD)
    {
        battleCharacter = bcD;
    }
}