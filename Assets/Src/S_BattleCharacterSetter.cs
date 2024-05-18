using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dat_affinity {
    public S_Element element;
    public float affinity = 1f;
}

[CreateAssetMenu(fileName = "New character", menuName = "BC Setter")]
public class S_BattleCharacterSetter : ScriptableObject
{
    public S_Stats baseStats;
    public S_Stats_Util baseHealth;
    public S_Stats_Util minHealthIncrease;
    public S_Stats levelsStatIncrease;
    public S_Stats_Util maxHealthIncrease;
    public List<S_Move> movesToLearn;
    public List<S_Passive> passivesToLearn;
    public Color characterColour = new Color(1, 0.95f, 0.75f);
    public Color characterColour2 = new Color(1, 0.95f, 0.75f);
    public dat_affinity[] defaultAffinities;
    public S_RPGBehaviourScript characterBehaviour;

    public Tuple<S_Element, float> FindAffinity(S_Element el) {
        foreach (var element in defaultAffinities) {
            if (element.element == el) {
                return new Tuple<S_Element, float>(element.element, element.affinity);
            }
        }
        return null;
    }
}
