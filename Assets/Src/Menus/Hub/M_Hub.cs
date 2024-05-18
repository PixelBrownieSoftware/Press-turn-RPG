using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Hub : S_MenuSystem
{
    public R_Float money;
    public R_ShopItem shopItems;
    public CH_Text menuChanger;
    public R_Boolean skillsReadonly;

    public override void StartMenu()
    {
        base.StartMenu();
    }
}
