using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Int : O_Button
{
    public int number;
    public CH_Int moveClickEvent;

    public override void OnClickEvent()
    {
        base.OnClickEvent();
        moveClickEvent.RaiseEvent(number);
    }

    public void SetIntButton(int _number)
    {
        number = _number;
    }
}

