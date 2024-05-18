using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class B_TargetWithUI : B_BattleTarget
{
    public Slider health;

    public new void SetTargetButton(ref O_BattleCharacter target) {
        base.SetTargetButton(ref target);
    }

    public void SetSliderBar() {
        float HPCompare =((float)target.characterHealth.health / (float)target.characterHealth.maxHealth) * 100f;
        health.maxValue = 100f;
        health.value = HPCompare;
    }

    private void Update()
    {
        if (target != null) {
            SetSliderBar();
        }
    }
}
