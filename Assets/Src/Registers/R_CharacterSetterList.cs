using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Character setter list")]
public class R_CharacterSetterList : R_Default
{
    public List<S_BattleCharacterSetter> characterSetters = new List<S_BattleCharacterSetter>();

    public S_BattleCharacterSetter GetCharacter(string characterName) {
        return characterSetters.Find(x => x.name == characterName);
    }
}
