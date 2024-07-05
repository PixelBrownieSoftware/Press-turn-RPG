using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class B_BattleMove: O_Button
{
    public S_Move move;
    public CH_Move moveClickEvent;
    public S_GuiList spCost;
    public Image elementImag;

    protected new void Awake()
    {
        button = transform.Find("Button").GetComponent<Button>();
    }

    public override void OnClickEvent()
    {
        base.OnClickEvent();
        if (move != null)
            moveClickEvent.RaiseEvent(move);
    }


    public void SetBattleButton(S_Move move, int cost) {
        this.move = move;
        if(text != null)
            text.text = "" + move.name;
        if (spCost != null)
            spCost.amount = cost;
        elementImag.sprite = move.element.elementImage;
        SetButtonColour(move.element.elementColour);
    }

}
