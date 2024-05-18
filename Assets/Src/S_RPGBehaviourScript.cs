using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RPGB_Setting {
    public RPGB_Condition[] conditions;
}
[System.Serializable]
public class RPGB_Condition
{
    public S_Element elementConstrain;
    public float percentage;
    public int amount;
    public float rentChance = 1;
    public CONDITION_TYPE conditionType;
    public bool isMove;
    public bool targetReverse = false;
    public enum CONDITION_TYPE { 
        ALWAYS,
        TYPE_OFFENSIVE,
        TYPE_HEALING,
        POWER_EQUAL,
        POWER_LESS,
        POWER_GREATER,
        POWER_LESS_EQUAL,
        POWER_GREATER_EQUAL,
        HEALTH_GREATER,
        HEALTH_LOWER,
        HEALTH_LOWER_EQUAL,
        HEALTH_GREATER_EQUAL,
        HEALTH_EQUAL,
        STAMINA_EQUAL,
        STAMIMA_LOWER,
        STAMIMA_LOWER_EQUAL,
        STAMINA_GREATER,
        STAMINA_GREATER_EQUAL,
        COST_LOWER,
        COST_LOWER_EQUAL,
        ELEMENTAL_AFFINITY_LOWER,
        ELEMENTAL_AFFINITY_LOWER_EQUAL,
        ELEMENTAL_AFFINITY_GREATER_EQUAL,
        ELEMENTAL_AFFINITY_GREATER,
        ELEMENTAL_AFFINITY_EQUAL,
        MOVE_OF_ELEMENT,
        HAS_STATUS_EFFECT
    }
}

[CreateAssetMenu(fileName = "New RPG Behaviour script", menuName = "RPG Behaviour")]
public class S_RPGBehaviourScript : ScriptableObject
{
    public RPGB_Setting[] settings;
}
