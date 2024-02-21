using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class O_BattleCharacter
{
    public S_Stats characterStats; 
    public S_Stats_Util characterHealth; 
    public float experiencePoints;
    public int level;
    public List<S_Move> extraSkills;
    public S_BattleCharacterSetter baseCharacterData;

    public void Damage(int dmg) {
        characterHealth.health -= dmg;
    }

    public void ExperiencePointsCalculation(float exp) {
    
    }
}
