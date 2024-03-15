using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct s_ability_req { }

public class S_Ability : ScriptableObject
{
    public S_Stats statReq;
    public bool MeetsRequirements(O_BattleCharacter bc)
    {
        if (statReq <= bc.characterStats)
            return true;
        return false;
    }

    public bool MeetsRequirements(O_BattleCharacter bc, S_Element element)
    {
        int discount = 1//bc.characterDataSource.GetDiscounts[element] * -1
            ;
        if (statReq <= bc.characterStats + discount)
            return true;
        return false;
    }
}
