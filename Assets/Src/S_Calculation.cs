using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class S_Calculation {
    
    public static int CalculateDamage(O_BattleCharacter user, O_BattleCharacter target, S_Move move, List<float> modifiers, int randomVal)
    {
        if (target == null)
            return 0;
        S_Element el = move.element;
        int dmg = 0;
        if (!move.fixedValue)
        {
            float multipler = 1f;
            float elementals = user.GetElementWeakness(el);
            if (elementals < 0 && elementals > -1)
                multipler = (elementals * -1);
            else if (elementals <= -1)
                multipler = ((elementals + 1) * -1);

            if (modifiers != null)
            {
                foreach (float mod in modifiers)
                {
                    multipler += mod;
                }
            }
            int mvPower = move.power + randomVal;
            mvPower = Mathf.Clamp(mvPower, 1, int.MaxValue);
            float elementalDamage = el.stats.GetFloats(user.characterStats);
            Debug.Log(user.name + " stats: " + (move.power * elementalDamage) + " target vitality: " + target.characterStatsNet.vitality);
            dmg = (int)((move.power * elementalDamage / (float)target.characterStatsNet.vitality) * multipler);
            dmg = Mathf.Abs(dmg);
        }
        else { dmg = move.power; }
        return dmg;
    }

    public static bool PredictStatChance(int userVal, int targVal, float connectMax)
    {
        userVal = Mathf.Clamp(userVal, 1, int.MaxValue);
        targVal = Mathf.Clamp(targVal, 1, int.MaxValue);
        int totalVal = userVal + targVal;
        float userModify = (float)userVal + ((float)userVal * connectMax);
        Debug.Log("User chance: " + userModify + " enemy chance: " + targVal);
        float attackConnectChance = (userModify / (float)totalVal);
        float gonnaHit = UnityEngine.Random.Range(0f, 1f);
        if (gonnaHit > attackConnectChance)
            return false;
        return true;
    }
    /*
    public bool PredictStatChance(int userVal, int targVal, float connectMax) {
        userVal = Mathf.Clamp(userVal, 1, int.MaxValue);
        targVal = Mathf.Clamp(targVal, 1, int.MaxValue);
        int totalVal = userVal + targVal;
        float userModify = (float)userVal + ((float)userVal * connectMax);
        print("User chance: " + userModify + " enemy chance: " + targVal);
        float attackConnectChance = (userModify / (float)totalVal);
        float gonnaHit = UnityEngine.Random.Range(0f, 1f);
        if (gonnaHit > attackConnectChance)
            return false;
        return true;
    }
    
    public static void Damage() {

        if (targ.health <= 0)
            yield break;
        willHit = PredictStatChance(currentCharacterObject.dexterity, targ.agiNet, 0.65f);
        bool isLucky = PredictStatChance(currentCharacterObject.luckNet, targ.luckNet, -0.80f);
        bool isCritical = IsCritical(targ);
        print("Will hit? " + willHit + " Is critical? " + isCritical + " Is lucky? " + isLucky);
        ELEMENT_WEAKNESS weaknessType = FigureWeakness(targ);
        if (weaknessType < ELEMENT_WEAKNESS.NULL)
        {
            switch (weaknessType)
            {
                case ELEMENT_WEAKNESS.NONE:
                    damageFlag = DAMAGE_FLAGS.NONE;
                    break;
                case ELEMENT_WEAKNESS.ABSORB:
                    damageFlag = DAMAGE_FLAGS.ABSORB;
                    break;
                case ELEMENT_WEAKNESS.FRAIL:
                    damageFlag = DAMAGE_FLAGS.FRAIL;
                    break;
                case ELEMENT_WEAKNESS.NULL:
                    damageFlag = DAMAGE_FLAGS.VOID;
                    break;
                case ELEMENT_WEAKNESS.REFLECT:
                    damageFlag = DAMAGE_FLAGS.REFLECT;
                    break;
            }
            if (isCritical)
            {
                modifier.Add(0.35f);
                damageFlag = DAMAGE_FLAGS.CRITICAL;
            }
            if (isLucky)
            {
                modifier.Add(0.5f);
                damageFlag = DAMAGE_FLAGS.LUCKY;
            }
        }
        if (willHit)
        {
            dmg = CalculateDamage(targ, currentCharacterObject, currentMove.move, modifier, randomValue);
        }
        else
        {
            damageFlag = DAMAGE_FLAGS.MISS;
        }
        DamageEffect(dmg, targ, characterPos, damageFlag);
        if (!willHit)
        {
            yield return StartCoroutine(DodgeAnimation(targ, characterPos));
        }
    }
    */
}
/*
[System.Serializable]
public class element_affinity
{
    public Tuple<int,int> this[ELEMENT element]
    {
        get
        {
            switch (element)
            {
                case ELEMENT.STRIKE:
                    break;
                case ELEMENT.PEIRCE:
                    break;
                case ELEMENT.FIRE:
                    break;
                case ELEMENT.ICE:
                    break;
                case ELEMENT.WATER:
                    break;
                case ELEMENT.ELECTRIC:
                    break;
            }
            return new Tuple<int, int>(0,0);
        }
    }
    public int CalculateTotal() {
        return strike +
            peirce +
            fire +
            ice +
            water +
            electric +
            wind +
            earth +
            psychic +
            light +
            dark +
            heal +
            support;
    }

    public int strike;
    public int peirce;

    public int fire;
    public int ice;
    public int water;
    public int electric;
    public int wind;
    public int earth;
    public int psychic;
    public int light;
    public int dark;
    public int heal;
    public int support;
}
[System.Serializable]
public struct element_weaknesses
{
    public float this[ELEMENT element]
    {
        get
        {
            if (weaknessChart != null) {
                if (weaknessChart.Length > 0)
                {
                    foreach (var weak in weaknessChart)
                    {
                        if (weak.element == element)
                            return weak.weakness;
                    }
                }
                else
                    return 1f;
            }
            return 1f;
        }
    }
    public weaknesses[] weaknessChart;

    [System.Serializable]
    public struct weaknesses
    {
        public ELEMENT element;
        public float weakness;
    }
}

[CreateAssetMenu(fileName = "Elements", menuName = "Element holder")]
public class s_element : ScriptableObject
{
    public string[] elements;
    
}
*/