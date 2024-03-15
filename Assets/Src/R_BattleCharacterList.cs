using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Battle Character List")]
public class R_BattleCharacterList : R_Default
{
    public List<O_BattleCharacter> battleCharList = new List<O_BattleCharacter>();

    private void OnEnable()
    {
        if (_isReset)
        {
            Clear();
        }
    }

    public O_BattleCharacter GetIndex(int ind)
    {
        return battleCharList[ind];
    }

    /*
    public int GetActiveCount()
    {
        return battleCharList.FindAll(x => x.inBattle == true).Count;
    }
    */

    public bool Find(System.Predicate<O_BattleCharacter> pred, O_BattleCharacter bc)
    {
        return pred.Invoke(bc);
    }

    public void Clear()
    {
        battleCharList.Clear();
    }

    public O_BattleCharacter Get(string chName)
    {
        return battleCharList.Find(x => x.name == chName);
    }

    public void Add(O_BattleCharacter bc)
    {
        battleCharList.Add(bc);
    }
}