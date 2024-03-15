using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Defeated characters")]
public class R_CharacterUnlock : R_Default
{
    public Dictionary<S_BattleCharacterSetter, int> defeatedCharacters = new Dictionary<S_BattleCharacterSetter, int>();

    public void OnEnable()
    {
        defeatedCharacters.Clear();
    }

    public void AddItem(S_BattleCharacterSetter character)
    {
        if (defeatedCharacters.ContainsKey(character))
        {
            defeatedCharacters[character]++;
        }
        else
        {
            defeatedCharacters.Add(character, 1);
        }
    }

    public int GetDefeatedCharacterCount(S_BattleCharacterSetter character) {

        if (defeatedCharacters.ContainsKey(character))
        {
            return defeatedCharacters[character];
        }
        else
        {
            defeatedCharacters.Add(character, 0);
            return defeatedCharacters[character];
        }
    }
}
