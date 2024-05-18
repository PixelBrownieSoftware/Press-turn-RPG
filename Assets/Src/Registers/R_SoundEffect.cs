using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Sound")]
public class R_SoundEffect : R_Default
{
    public float pitchMax = 1;
    public float pitchMin = 1;
    public AudioClip sound;
}
