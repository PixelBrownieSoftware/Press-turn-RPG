using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class S_BattleBattlecharacterFactory : MonoBehaviour
{
    public CH_BattleGroupMember factoryInput;
    public R_BattleCharacter factoryOutput;
    public R_CharacterSetterList factoryCharactersData;
    public R_MoveList allSkills;

    private void OnEnable()
    {
        factoryInput.OnGroupMemberEvent += CreateBattleCharacter;
    }

    private void OnDisable()
    {
        factoryInput.OnGroupMemberEvent -= CreateBattleCharacter;
    }
    public void CreateBattleCharacter(Save_BattleCharacter baseData)
    {
        S_BattleCharacterSetter characterSettings = factoryCharactersData.characterSetters.Find(x => x.name == baseData.baseCharacterData);
        if (characterSettings == null)
        {
            print("Character data could not be found");
            return;
        }
        List<S_Move> extraSkills = new List<S_Move>();
        foreach (var ability in baseData.extraSkills)
        {
            S_Move moveFind = allSkills.GetMove(ability);
            extraSkills.Add(moveFind);
        }
        O_BattleCharacter BC = CreateBattleCharacter(characterSettings, baseData.level, extraSkills);
        BC.revivable = true;
        factoryOutput.battleCharacter = BC;
    }

    public O_BattleCharacter CreateBattleCharacter(S_BattleCharacterSetter characterSettings, int level, List<S_Move> extraSkills)
    {
        O_BattleCharacter BC = new O_BattleCharacter();
        S_Stats_Util characterHealth = new S_Stats_Util(characterSettings.baseHealth);
        BC.characterHealth = characterHealth;
        BC.baseCharacterData = characterSettings;
        BC.characterStats = characterSettings.baseStats;
        BC.name = characterSettings.name;
        ///TODO: Once I get the flexible stats system going
        ///create a dictionary containing variable name and value.
        ///Until then, I'll hard-code the values.
        S_Stats stats = new S_Stats(characterSettings.baseStats);
        for (int i = 0; i < level; i++)
        {
            BC.LevelUp();
        }
        foreach (var ability in extraSkills)
        {
            if (ability.MeetsRequirements(BC))
            {
                BC.extraSkills.Add(ability);
            }
        }
        BC.characterStats = stats;
        return BC;
    }

    public void CreateBattleCharacter(S_BattleGroup.S_GroupMember baseData)
    {
        S_BattleCharacterSetter characterSettings = baseData.memberDat;
        O_BattleCharacter BC = CreateBattleCharacter(characterSettings, baseData.level, baseData.extraSkills.ToList());
        BC.revivable = baseData.revivable;
        factoryOutput.battleCharacter = BC;
    }
}
