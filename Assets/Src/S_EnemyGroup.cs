using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
using MagnumFoundation2.System;
using MagnumFoundation2.Objects;

[System.Serializable]
public class s_battleEvent
{
    //public List<s_bEvent> events;
    public ev_script eventScript;
    public enum B_COND
    {
        HEALTH,
        STAMINA,
        TURNS_ELAPSED,
        MOVE_USED
    }
    public enum B_CHECK_COND
    {
        PER_TURN,
        ON_START
    }
    public B_COND battleCond;
    public B_CHECK_COND battleCheckCond;
    public bool enabled = true;
    public int int0;
}
*/

[CreateAssetMenu(fileName = "New enemy group", menuName = "Battle group")]
public class S_BattleGroup : ScriptableObject
{
    [System.Serializable]
    public class S_GroupMember{
        public S_BattleCharacterSetter memberDat;
        public enum LEVEL_TYPE {
            RANDOM,
            FIXED
        }
        public LEVEL_TYPE levType;
        public int level;
        public int maxLevel;
        public S_Move[] extraSkills;
        public S_Passive[] extraPassives;
        //public charAI[] extraSkillsAI;
    }
    public bool fleeable = true;
    public bool tempOnly = false;
    public S_GroupMember[] members_Enemy;
    public S_GroupMember[] members_Player;
    //public S_GroupMember[] members_summonable;
    //public S_GroupMember[] members_pre_summon;

    public S_BattleCharacterSetter[] unlockCharacters;

    public S_BattleGroup[] branches;
    public S_BattleGroup[] perishBranches;
    public Shop_item[] shopItems;

    public Sprite bg1;
    public Sprite bg2;
    public Material material1;
    public Material material2;
}
