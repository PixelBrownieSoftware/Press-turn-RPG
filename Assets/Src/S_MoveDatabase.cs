using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move database", menuName = "Move database")]
public class S_MoveDatabase : ScriptableObject
{
    public List<S_Move> moves = new List<S_Move>();
}
