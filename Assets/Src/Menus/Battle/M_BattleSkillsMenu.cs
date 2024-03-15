using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class M_BattleSkillsMenu : S_MenuSystem
{
    public R_BattleCharacter currentCharacter;
    public B_BattleMove[] buttons;
    public R_Items items;
    public R_Boolean isItem;
    public R_Move selectedMove;
    public R_MoveList movesList;
    public CH_Move selectMove;
    [SerializeField]
    private CH_Text changeMenu;
    [SerializeField]
    private R_Text menuText;
    [SerializeField]
    private R_Text battleMenuType;
    [SerializeField]
    private CH_Func excecuteDisplayTargetsChannel;

    public TextMeshProUGUI comboMoveDesc;

    private void OnEnable()
    {
        selectMove.OnMoveFunctionEvent += SelectMove;
    }

    private void OnDisable()
    {
        selectMove.OnMoveFunctionEvent -= SelectMove;
    }

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<B_BattleMove>();
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
    }
    public void SelectMove(S_Move move)
    {
        selectedMove.SetMove(move);
        excecuteDisplayTargetsChannel.OnFunctionEvent.Invoke();
        changeMenu.RaiseEvent("TargetMenu");
    }


    public override void StartMenu()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        base.StartMenu();
        List<S_Move> moves = null;

        switch (battleMenuType.text)
        {

            case "Skills":
                moves = movesList.moveListRef;
                if (moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var button = buttons[i];
                        button.SetButonText(moves[i].name);
                        string strCost = "";
                        int cost = 0;
                        if (moves[i].cost.stamina > 0)
                        {
                            cost = moves[i].cost.stamina;
                            strCost = cost + " SP";
                        }
                        else
                        {
                            /*
                                cost = s_calculation.DetermineHPCost(moves[i], currentCharacter.characterRef.strengthNet, 
                                    currentCharacter.characterRef.vitalityNet,
                                    currentCharacter.characterRef.maxHealth);
                                strCost = cost + " HP";
                            */
                        }

                        bool canUse = true;
                        canUse = currentCharacter.battleCharacter.characterHealth.stamina >= cost;
                        /*
                        if (moves[i].element.isMagic)
                                canUse = currentCharacter.battleCharacter.stamina >= cost;
                            else
                                canUse = currentCharacter.battleCharacter.health > cost;
                        */
                        if (!canUse)
                        {
                            button.SetButtonColour(Color.grey);
                            button.SetButonTextColour(Color.grey);
                            button.SetBattleButton(moves[i], strCost);
                            button.move = null;
                        }
                        else
                        {
                            button.SetButtonColour(Color.white);
                            button.SetButonTextColour(Color.white);
                            button.SetBattleButton(moves[i], strCost);
                        }
                        button.gameObject.SetActive(true);
                    }
                }
                break;
            case "Items":
                isItem.boolean = true;
                if (items.inventory.Count > 0)
                {
                    int ind = 0;
                    foreach (var item in items.inventory)
                    {

                        var button = buttons[ind];
                        button.SetBattleButton(item.Key, "" + item.Value);
                        button.SetButonText(item.Key.name);
                        button.gameObject.SetActive(true);
                        ind++;
                    }
                }
                break;
        }
    }
}
