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

    private void OnEnable()
    {
        factoryInput.OnGroupMemberEvent += CreateBattleCharacter;
    }

    private void OnDisable()
    {
        factoryInput.OnGroupMemberEvent -= CreateBattleCharacter;
    }

    public void CreateBattleCharacter(S_BattleGroup.S_GroupMember baseData)
    {
        O_BattleCharacter BC = new O_BattleCharacter();
        S_BattleCharacterSetter characterSettings = baseData.memberDat;
        print(baseData.memberDat.name);
        S_Stats_Util characterHealth = new  S_Stats_Util(characterSettings.baseHealth);
        BC.baseCharacterData = characterSettings;
        BC.characterStats = characterSettings.baseStats;
        BC.name = characterSettings.name;
        ///TODO: Once I get the flexible stats system going
        ///create a dictionary containing variable name and value.
        ///Until then, I'll hard-code the values.
        int stGT = characterSettings.levelsStatIncrease.strength;
        int vtGT = characterSettings.levelsStatIncrease.vitality;
        int agGT = characterSettings.levelsStatIncrease.agility;
        int dxGT = characterSettings.levelsStatIncrease.dexterity;
        int lcGT = characterSettings.levelsStatIncrease.luck;
        int mgGT = characterSettings.levelsStatIncrease.magicPow;
        S_Stats stats = new S_Stats(characterSettings.baseStats);
        for (int i = 0; i < baseData.level; i++) {
            characterHealth.maxHealth += UnityEngine.Random.Range(characterSettings.minHealthIncrease.health, characterSettings.maxHealthIncrease.health);
            characterHealth.maxStamina += UnityEngine.Random.Range(characterSettings.minHealthIncrease.stamina, characterSettings.maxHealthIncrease.stamina);
            
            BC.characterStats += new S_Stats
                (
                i % stGT == 0 ? 1 : 0, 
                i % vtGT == 0 ? 1 : 0,
                i % agGT == 0 ? 1 : 0, 
                i % dxGT == 0 ? 1 : 0,
                i % lcGT == 0 ? 1 : 0, 
                i % mgGT == 0 ? 1 : 0
                );
        }
        foreach (var ability in baseData.extraSkills)
        {
            if (ability.MeetsRequirements(BC))
            {
                BC.extraSkills.Add(ability);
            }
        }
        BC.characterHealth = characterHealth;
        BC.characterStats = stats;
        factoryOutput.battleCharacter = BC;
    }
}
