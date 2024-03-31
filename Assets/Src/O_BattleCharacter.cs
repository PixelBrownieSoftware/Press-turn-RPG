using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class O_BattleCharacter
{
    public string name;
    public S_Stats characterStats;
    public S_Stats statusStats = new S_Stats();

    public S_Stats characterStatsNet
    {
        get
        {
            S_Stats net = characterStats + statusStats;
            return net;
        }
    }
    public S_Stats_Util characterHealth; 
    public float experiencePoints;
    public int level;
    public List<S_Move> extraSkills = new List<S_Move>();
    public S_BattleCharacterSetter baseCharacterData;
    public Vector2 postion;

    public void Damage(int dmg) {
        characterHealth.health -= dmg;
    }

    public void ExperiencePointsCalculation(float exp) {
    
    }
}
[System.Serializable]
public struct Save_BattleCharacter
{
    public S_Stats characterStats;
    public S_Stats_Util characterHealth;
    public float experiencePoints;
    public int level;
    public List<string> extraSkills;
    public string baseCharacterData;
    public Save_BattleCharacter(O_BattleCharacter chara) { 
        characterStats = chara.characterStats; 
        characterHealth = chara.characterHealth;
        extraSkills = new List<string>();
        foreach (var extraSkill in chara.extraSkills) { 
            extraSkills.Add(extraSkill.name);
        }
        baseCharacterData = chara.baseCharacterData.name;
        experiencePoints = chara.experiencePoints;
        level = chara.level;
    }
}