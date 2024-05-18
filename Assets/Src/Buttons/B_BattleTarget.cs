using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BattleTarget : O_Button
{
    public O_BattleCharacter target;
    public CH_BattleCharacter moveClickEvent;

    public override void OnClickEvent()
    {
        base.OnClickEvent();
        moveClickEvent.RaiseEvent(target);
    }

    public void SetTargetButton(ref O_BattleCharacter target)
    {
        this.target = target;
        text.text = "" + target.name + " (" + (target.characterHealth.health * 100) + "%" + ")";
    }

}
