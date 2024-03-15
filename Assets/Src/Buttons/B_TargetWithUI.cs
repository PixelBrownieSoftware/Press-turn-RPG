using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class B_TargetWithUI : B_BattleTarget
{
    public Slider health;
    public bool isHP = true;

    public new void SetTargetButton(O_BattleCharacter target) {
        base.SetTargetButton(target);
        float HPCompare = (float)((float)target.characterHealth.health / (float)target.characterHealth.maxHealth);
        health.maxValue = 1f;
        health.value = isHP ? HPCompare : (float)((float)target.characterHealth.stamina / (float)target.characterHealth.maxStamina);
    }
}
