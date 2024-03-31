using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "BC Setter")]
public class S_BattleCharacterSetter : ScriptableObject
{
    public S_Stats baseStats;
    public S_Stats_Util baseHealth;
    public S_Stats minStatIncrease;
    public S_Stats_Util minHealthIncrease;
    public S_Stats maxStatIncrease;
    public S_Stats_Util maxHealthIncrease;
    public List<S_Move> moves;
    public Color characterColour = new Color(1, 0.95f, 0.75f);
    public Color characterColour2 = new Color(1, 0.95f, 0.75f);
}
