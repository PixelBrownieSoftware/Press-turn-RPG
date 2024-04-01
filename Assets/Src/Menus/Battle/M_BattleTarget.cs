using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_BattleTarget : S_MenuSystem
{
    public R_BattleCharacterList targetCharacters;
    public R_Move moveRef;
    public R_BattleCharacter selectedCharacter;
    public B_BattleTarget[] buttons;

    public CH_BattleCharacter selectTargetFunction;
    public CH_Text switchMenu;
    public CH_Func performMove;

    public void SetTarget(O_BattleCharacter target) {
        selectedCharacter.SetCharacter(target);
        performMove.RaiseEvent();
        switchMenu.RaiseEvent("EMPTY");
        /*
        if (targetMenuTo.text != "CharacterStatus")
            performMove.RaiseEvent();

        switchMenu.RaiseEvent(targetMenuTo.text);
        */
    }

    private void OnDisable()
    {
        selectTargetFunction.OnFunctionEvent -= SetTarget;
    }

    private void OnEnable()
    {
        selectTargetFunction.OnFunctionEvent += SetTarget;
    }

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<B_BattleTarget>();
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
    }

    public override void StartMenu()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        base.StartMenu();
        //mov = moveRef.move;

        for (int i = 0; i < targetCharacters.battleCharList.Count; i++)
        {
            O_BattleCharacter battleChar = targetCharacters.GetIndex(i);
            var tg = buttons[i];
            //bool isStatus = mov.moveType == s_move.MOVE_TYPE.NONE;

            tg.SetTargetButton(battleChar);
            tg.SetButonText(battleChar.name);
            tg.gameObject.SetActive(true);
            tg.gameObject.transform.position = Camera.main.WorldToScreenPoint(battleChar.position);
        }
    }


}
