using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class B_BattleMove: O_Button
{
    public S_Move move;
    public CH_Move moveClickEvent;
    public TextMeshProUGUI tmp;

    public override void OnClickEvent()
    {
        base.OnClickEvent();
        if (move != null)
            moveClickEvent.RaiseEvent(move);
    }


    public void SetBattleButton(S_Move move, string cost) {
        this.move = move;
        if(text != null)
            text.text = "" + move.name;
        if (tmp != null)
            tmp.text = "" + cost;
    }

}
