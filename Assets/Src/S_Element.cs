using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Element ")]
public class S_Element : ScriptableObject
{
    public S_Stats_Float stats;
    public S_StatusInflict[] statusInflict;
    public Sprite elementImage;
    //For cosmetics
    public Color elementColour = Color.white;
}

[System.Serializable]
public struct S_StatusInflict
{
    public S_StatusEffect statusEffect;
    public float chance;
    public bool add_remove;
}