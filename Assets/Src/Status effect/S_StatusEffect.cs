using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Status effect", menuName = "Status effect")]
public class S_StatusEffect : ScriptableObject
{
    /*
    public S_StatusApplier[] statusAppliers;
    [System.Serializable]
    public class S_ElementAffinityChange
    {
        public S_Element element;   //Have this as null if you want all the elements to be affected
        public float elementAffinityChange = 0.5f;
        public float elementAffinityClampMax;
        public float elementAffinityClampMin;
    }
    */
    [System.Serializable]
    public class S_ElementStatusRemove
    {
        public S_Element element;
        public float statusRemovePercentage = 1f;
    }

    public S_ElementStatusRemove GetStatusRemoveElement(S_Element element) {
        foreach (var el in criticalOnHit) {
            if (el.element == element) {
                return el;
            }
        }
        return null;
    }
    [System.Serializable]
    public class s_statusReplace {
        public S_StatusEffect toReplace;
        public S_StatusEffect replace;
    }
    public enum RESTRICTION { NONE, CANNOT_MOVE, RANDOM_FOE, RANDOM_ALLY, RANDOM_ALL }
    public RESTRICTION restriction;
    public int maxDuration;
    public int minDuration;
    public int stack;  //If it gets hit by a move

    public bool statUtilRegen = false;
    public float regenPercentage; //A value from -1.0 - 1.0, this is a percentage of the power of the "damage" dealt by the move 

    public S_Stats statAffect;
    public S_Stats_Util statUtilAffect;

    public bool removeOnEndRound = false;
    public bool permitEvasion = true;

    //If a character with this status effect gets hit by a move with one of these elements,
    //a critical hit will occur
    public S_ElementStatusRemove[] criticalOnHit;
    //public S_ElementAffinityChange[] elementalAffinityChange;
    public Sprite statusImage;
    public AnimationClip animation;

    public s_statusReplace[] statusReplace;
    public S_RPGBehaviourScript changedBehaviour;
}
