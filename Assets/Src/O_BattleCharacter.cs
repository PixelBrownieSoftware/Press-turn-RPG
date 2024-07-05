using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class S_StatusInstance
{
    public S_StatusInstance()
    {
    }
    public S_StatusInstance(S_StatusEffect status)
    {
        this.status = status;
        duration = UnityEngine.Random.Range(status.minDuration, status.maxDuration);
    }
    public S_StatusInstance(S_StatusEffect status, int damage)
    {
        this.damage = damage;
        this.status = status;
        duration = UnityEngine.Random.Range(status.minDuration, status.maxDuration);
    }

    public S_StatusEffect status;
    public int duration;
    public int damage;
}
[System.Serializable]
public class O_BattleCharacter
{
    public string name;
    public S_Stats characterStats;
    public S_Stats statusStats = new S_Stats();

    //I do plan on changing this to not be something hardcoded but rather a custom keyvaluepair
    public S_Stats characterStatsNet
    {
        get
        {
            S_Stats net = characterStats + statusStats;
            return net;
        }
    }
    public int dexterity {
        get { 
        return characterStatsNet.dexterity;
        }
    }
    public int strength { 
    get
        {
            return characterStatsNet.strength;
        }
    }
    public int vitality
    {
        get
        {
            return characterStatsNet.vitality;
        }
    }
    public int agility
    {
        get
        {
            return characterStatsNet.agility;
        }
    }
    public int magicPow
    {
        get
        {
            return characterStatsNet.magicPow;
        }
    }

    public int luck
    {
        get
        {
            return characterStatsNet.luck;
        }
    }

    public List<S_Move> getCurrentExtraMoves
    {
        get
        {
            List<S_Move> result = new List<S_Move>();
            result.AddRange(extraSkills);
            return result;
        }
    }
    public List<S_Move> getCurrentDefaultMoves
    {
        get
        {
            List<S_Move> result = new List<S_Move>();
            foreach (var mv in baseCharacterData.movesToLearn)
            {
                if (mv.MeetsRequirements(this))
                {
                    //Debug.Log(mv.name);
                    result.Add(mv);
                }
            }
            return result;
        }
    }
    public List<S_Move> getCurrentMoves
    {
        get {
            List<S_Move> result = new List<S_Move>();
            foreach (var mv in baseCharacterData.movesToLearn) {
                if (mv.MeetsRequirements(this)) {
                    //Debug.Log(mv.name);
                    result.Add(mv);
                }
            }
            result.AddRange(extraSkills);
            return result;
        }
    }
    public S_Stats_Util characterHealth; 
    public float experiencePoints = 0f;
    public int level;
    public bool revivable = true;
    public List<S_Move> extraSkills = new List<S_Move>();
    public S_BattleCharacterSetter baseCharacterData;
    public Vector2 position;
    public UnityAction<string> playAnimation;
    public float getAnimHandlerState = 0f;  //A bit of a kludge
    public List<S_StatusInstance> statusEffects = new List<S_StatusInstance>();

    public UnityAction onDefeat;
    public UnityAction onHurt;

    public float GetElementWeakness(S_Element element)
    {
        S_Move elementalPassive = getCurrentMoves.Find(x => x.isPassive && x.element == element);
        Tuple<S_Element, float> affinity = baseCharacterData.FindAffinity(element);
        if (affinity != null) {
            if (elementalPassive != null) {
                if (affinity.Item2 < elementalPassive.percentage)
                {
                    return elementalPassive.percentage;
                }
            }
            return affinity.Item2;
        } else {
            if (elementalPassive != null)
                return elementalPassive.percentage;
        }
        return 1f;
    }

    public void UpateStatusEffectDuration() {
        for (int i = 0; i < statusEffects.Count; i++) { 
            var effect = statusEffects[i];
            effect.duration--;
            Debug.Log("status " + statusEffects[i].duration + " fake status? " + effect.duration);
            if (effect.duration == 0) {
                RemoveStatus(effect.status);
            }
        }
    }

    public void UpdateStatusEffectBuffs()
    {
        S_Stats stats = new S_Stats();

        foreach (var status in statusEffects)
        {
            S_StatusEffect statEff = status.status;
            stats += statEff.statAffect;
        }
        statusStats = stats;
    }

    public void SetStatus(S_StatusEffect statEff)
    {
        SetStatus(statEff, 0);
    }
    public void SetStatus(S_StatusEffect statEff, int dmg)
    {
        bool cancelOut = false;
        if (statusEffects.Find(x => x.status == statEff) == null)
        {
            foreach (var st in statEff.statusReplace)
            {
                if (RemoveStatus(st.replace))
                    statusEffects.Add(new S_StatusInstance(st.toReplace, dmg));
                if (st.replace != null)
                    if (st.replace.statusReplace != null)
                        if (st.replace.statusReplace.ToList().Find(x => x.toReplace == st.replace) != null)
                        {
                            cancelOut = true;
                            RemoveStatus(st.replace);
                        }
            }
            if (!cancelOut)
                statusEffects.Add(new S_StatusInstance(statEff, dmg));
            UpdateStatusEffectBuffs();
        }
    }
    public bool RemoveStatus(S_StatusEffect statEff)
    {
        if (statusEffects.Find(x => x.status == statEff) != null)
        {
            statusEffects.Remove(statusEffects.Find(x => x.status == statEff));
            return true;
        }
        return false;
    }
    public bool IsCritical(S_Move mov)
    {
        foreach (var statusEff in statusEffects)
        {
            foreach (var status in statusEff.status.criticalOnHit)
            {
                if (status.element == mov.element)
                {
                    if (GetElementWeakness(mov.element) <= 0) {
                        return false;
                    }
                    float removeChance = UnityEngine.Random.Range(0, 1f);
                    if (removeChance > status.statusRemovePercentage)
                    {
                        RemoveStatus(statusEff.status);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void ChangeUtilSPStat(int spDmg)
    {
        ChangeUtilStat(0, spDmg);
    }
    public void ChangeUtilHPStat(int hpDmg)
    {
        ChangeUtilStat(hpDmg, 0);
    }
    public void ChangeUtilStat(int hpDmg, int spDmg) {
        characterHealth.health += hpDmg;
        characterHealth.health = Mathf.Clamp(characterHealth.health, 0, characterHealth.maxHealth);
        characterHealth.health += spDmg;
        characterHealth.health = Mathf.Clamp(characterHealth.stamina, 0, characterHealth.maxStamina);
    }
    public void ChangeUtilStat(S_Stats_Util util)
    {
        characterHealth.health += util.health;
        characterHealth.health = Mathf.Clamp(characterHealth.health, 0, characterHealth.maxHealth);
        characterHealth.health += util.stamina;
        characterHealth.health = Mathf.Clamp(characterHealth.stamina, 0, characterHealth.maxStamina);
    }

    public float ExperiencePointsCalculation(float remainder) {
        //70 + 40
        float toAdd = Mathf.Abs(experiencePoints - remainder);
        Debug.Log("To add: " + toAdd + " Remainder: " + remainder);
        //float neededToLevelUp = (level + 1) * Mathf.Pow((1.24f / 0.04f), 1.24f);
        if (remainder > 100f)
        {
            remainder -= toAdd;
            experiencePoints += toAdd;
        }
        else
        {
            if (experiencePoints + remainder > 100f)
            {
                remainder -= toAdd;
                experiencePoints += toAdd;
            }
            else
            {
                experiencePoints += remainder;
                remainder = 0;
            }
        }
        if (experiencePoints >= 100f)
        {
            LevelUp();
        }
        return remainder;
    }

    public void LevelUp()
    {
        experiencePoints = 0;
        level++;
        int stGT = baseCharacterData.levelsStatIncrease.strength;
        int vtGT = baseCharacterData.levelsStatIncrease.vitality;
        int agGT = baseCharacterData.levelsStatIncrease.agility;
        int dxGT = baseCharacterData.levelsStatIncrease.dexterity;
        int lcGT = baseCharacterData.levelsStatIncrease.luck;
        int mgGT = baseCharacterData.levelsStatIncrease.magicPow;
        characterHealth.maxHealth += UnityEngine.Random.Range(
            baseCharacterData.minHealthIncrease.health, 
            baseCharacterData.maxHealthIncrease.health);
        characterHealth.maxStamina += UnityEngine.Random.Range(
            baseCharacterData.minHealthIncrease.stamina, 
            baseCharacterData.maxHealthIncrease.stamina);

        characterStats += new S_Stats
            (
            level % stGT == 0 ? 1 : 0,
            level % vtGT == 0 ? 1 : 0,
            level % agGT == 0 ? 1 : 0,
            level % dxGT == 0 ? 1 : 0,
            level % lcGT == 0 ? 1 : 0,
            level % mgGT == 0 ? 1 : 0
            );
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