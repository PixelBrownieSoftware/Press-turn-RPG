using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move")]
public class S_Move : S_Ability
{
    public int power;
    public bool fixedValue = false;
    public bool targetDead = false;
    public enum TARGET_SCOPE { SINGLE, ALL, AOE, RANDOM }
    public enum FACTION_SCOPE { FOES, ALLY, ALL }
    public TARGET_SCOPE targetScope = TARGET_SCOPE.SINGLE;
    public FACTION_SCOPE factionScope = FACTION_SCOPE.FOES;
    public S_Element element;
    public S_Stats statRequirments;
    public S_Stats_Util cost;
}


[System.Serializable]
public class S_ActionAnim
{
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
        FADE_SCREEN
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
