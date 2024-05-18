using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move")]
public class S_Move : ScriptableObject
{
    public int power;
    public float accuracy = 1f;  //Value exists between 0 - 1. If set to less than zero or more than 1, it will always connect
    public bool fixedValue = false;
    public bool targetDead = false;
    public bool isHeal = false;
    public enum TARGET_SCOPE { SINGLE, ALL, AOE, RANDOM }
    public enum FACTION_SCOPE { FOES, ALLY, ALL }
    public TARGET_SCOPE targetScope = TARGET_SCOPE.SINGLE;
    public FACTION_SCOPE factionScope = FACTION_SCOPE.FOES;
    public S_Element element;
    public int cost;
    public S_ActionAnim[] pre_animations;
    public S_ActionAnim[] animations = new S_ActionAnim[1] { new S_ActionAnim() };   //These are the things that will repeat
    public int maxRepeat = 1, minRepeat = 1;
    public string customFunction;

    public enum PASSIVE_TYPE
    {
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
    public enum PASSIVE_TRIGGER
    {
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

    public bool isPassive = false;
    public PASSIVE_TYPE passiveSkillType;
    public PASSIVE_TRIGGER passiveTrigger;
    public STAT_TYPE stat;
    public S_Element passiveElement;
    public float percentage;    //This will, for example override an affinity.
    public float percentageHeal; //Health will be healed more than stamina

    public S_Stats statReq;
    public bool MeetsRequirements(O_BattleCharacter bc)
    {
        if (statReq <= bc.characterStats)
            return true;
        return false;
    }

    public bool MeetsRequirements(O_BattleCharacter bc, S_Element element)
    {
        int discount = 1//bc.characterDataSource.GetDiscounts[element] * -1
            ;
        if (statReq <= bc.characterStats + discount)
            return true;
        return false;
    }
}


[System.Serializable]
public class S_ActionAnim
{
    public S_ActionAnim() {
        actionType = ACTION_TYPE.CALCULATION;
    }
    public string name;
    public enum ACTION_TYPE
    {
        WAIT, //either wait for a projectile,
        MOVE,
        FACE,
        PROJECTILE,
        HIT_ANIMATION,
        CALCULATION, //calculations,
        ANIMATION,
        CHAR_ANIMATION,
        MOVE_CAMERA,
        ZOOM_CAMERA,
        FADE_TARGET,
        FADE_SCREEN,
        PLAY_SOUND
    }
    public ACTION_TYPE actionType;

    public enum MOTION
    {
        FORWARDS,
        BACKWARDS,
        POINT,
        TO_TARGET,
        SELF,
        ALL,
        ALL_SELF,
        ALL_TARGET,
        USER_2,
        USER_3,
        USER_4,
        USER_5
    }
    public bool teleport = false;

    public MOTION start;
    public MOTION goal;
    public Vector2 offset;
    public int animation_id;
    public int minimumPowerRandomness;
    public int maximumPowerRandomness;
    public Sprite picture;
    public Animation animframes;
    public AudioClip sound;
    public float soundPitchMin = 1, soundPitchMax = 1;
    public float time;
    public float speed;
    public float toZoom;
    public float initialZoom;
    public bool simulteneous = false;

    //Used for the target option
    public enum TARGET_POS  //Places will be relative to the targets direction
    {
        TOP,
        TOP_BACK,
        TOP_FRONT,
        CENTRE,
        CENTRE_BACK,
        CENTRE_FRONT,
        BASE,
        BASE_FRONT,
        BASE_BACK
    }
    public TARGET_POS targetPos;

    public Color startColour;
    public Color endColour;
}

//The animation of a hit or a projectile
[System.Serializable]
public struct s_moveAnim
{
    public string name;
    public enum MOVEPOSTION
    {
        ON_TARGET,
        FIXED,
        ALL_SAME_TIME,
        ALL_LEFT_TO_RIGHT,
        ALL_RIGHT_TO_LEFT
    }
    public MOVEPOSTION pos;
}
