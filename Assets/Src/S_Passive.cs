using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Passive move")]
public class S_Passive : S_Ability
{
    public enum PASSIVE_TYPE {
        STAT_BOOST,
        ELEMENT_DMG_BOOST,
        RESIST,
        NULL,
        ABSORB,
        REPEL,
        SACRIFICE,
        REGEN,
        COUNTER,
        CUSTOM
    }
    public enum PASSIVE_TRIGGER {
        ALWAYS,
        SELF_HIT,
        ALLY_HIT,
        SELF_ALLY_HIT,
        SELF_BEFORE_HIT,
        ALLY_BEFORE_HIT,
        SELF_DEFEAT,
        ALLY_DEFEAT,
        SELF_ALLY_DEFEAT
    }
    public enum STAT_TYPE
    {
        HEALTH,
        STAMINA,
        HP_SP
    }

    public bool singleUse = false;
    public PASSIVE_TYPE passiveSkillType;
    public PASSIVE_TRIGGER passiveTrigger;
    public STAT_TYPE stat;
    public S_Element element;
    public float percentage;
    public float percentageHeal; //Health will be healed more than stamina
    public string customPassive;
}