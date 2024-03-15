using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BattleMainMenu : S_MenuSystem
{
    [SerializeField]
    private R_BattleCharacter currentCharacterRef;
    //private CH_BattleCharacter currentCharacter;
    private R_BattleCharacter currentCharacter;
    [SerializeField]
    private R_BattleCharacter targetCharacterRef;
    [SerializeField]
    private R_MoveList currentMovesRef;
    [SerializeField]
    private R_Move currentMoveRef;
    [SerializeField]
    private R_Items inventory;
    [SerializeField]
    private R_EnemyGroup currentGroup;

    public R_BattleCharacterList targetList;
    public R_BattleCharacterList players;
    public R_BattleCharacterList opponents;

    public B_Function primaryAttackButton;
    public B_Function secondaryAttackButton;
    public B_Function teritaryAttackButton;
    public B_Function skillsButton;
    public B_Function guardButton;
    public B_Function itemButton;
    public B_Function analyseButton;
    public B_Function passButton;
    public B_Function runButton;

    public S_Move analyseMove;
    public S_Move guard;
    public S_Move pass;

    [SerializeField]
    private CH_Text changeMenu;
    [SerializeField]
    private R_Text menuText;
    [SerializeField]
    private R_Text battleMenuType;

    [SerializeField]
    private CH_Func goToFirst;
    [SerializeField]
    private CH_Func goToSecond;
    [SerializeField]
    private CH_Func goToThird;
    [SerializeField]
    private CH_Func goToSkills;
    [SerializeField]
    private CH_Func goToPass;
    [SerializeField]
    private CH_Func goToGuard;
    [SerializeField]
    private CH_Func performMove;
    [SerializeField]
    private CH_Func goToAnalyse;
    [SerializeField]
    private CH_Func goToItems;
    [SerializeField]
    private CH_Func runFromBattle;
    [SerializeField]
    private CH_Func getMovesMenu;

    private void OnEnable()
    {
        goToSkills.OnFunctionEvent += GoToSkills;
        goToPass.OnFunctionEvent += PassAction;
        goToGuard.OnFunctionEvent += GuardAction;
        runFromBattle.OnFunctionEvent += RunFromBattle;
        goToItems.OnFunctionEvent += GoToItems;
    }

    private void OnDisable()
    {
        goToSkills.OnFunctionEvent -= GoToSkills;
        goToPass.OnFunctionEvent -= PassAction;
        goToGuard.OnFunctionEvent -= GuardAction;
        runFromBattle.OnFunctionEvent -= RunFromBattle;
        goToItems.OnFunctionEvent -= GoToItems;
    }

    public void RunFromBattle()
    {
        changeMenu.RaiseEvent("EMPTY");
    }

    public void GoToSkills()
    {
        battleMenuType.text = "Skills";
        //menuText.text = "EMPTY";
        getMovesMenu.OnFunctionEvent.Invoke();
        changeMenu.RaiseEvent("BattleSkillsMenu");
    }
    public void GoToItems()
    {
        battleMenuType.text = "Items";
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("BattleSkillMenu");
    }
    public void GuardAction()
    {
       // targetCharacterRef.SetCharacter(currentCharacter);
        currentMoveRef.SetMove(guard);
        performMove.RaiseEvent();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("EMPTY");
    }
    public void PassAction()
    {
        //targetCharacterRef.SetCharacter(currentCharacter.);
        currentMoveRef.SetMove(pass);
        performMove.RaiseEvent();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("EMPTY");
    }

    public override void StartMenu()
    {
        currentCharacter = currentCharacterRef;
        secondaryAttackButton.gameObject.SetActive(false);
        teritaryAttackButton.gameObject.SetActive(false);
        skillsButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        base.StartMenu();
        primaryAttackButton.gameObject.SetActive(true);
        guardButton.gameObject.SetActive(true);
        passButton.gameObject.SetActive(true);
        analyseButton.gameObject.SetActive(true);
        /*
        if(currentCharacter.characterData.characterDataSource.secondMove != null)
            secondaryAttackButton.gameObject.SetActive(true);
        if (currentCharacter.characterData.characterDataSource.thirdMove != null)
            teritaryAttackButton.gameObject.SetActive(true);
        if (currentCharacter.GetAllMoves().Count > 0)
        {
            skillsButton.gameObject.SetActive(true);
        }
        */
        if (inventory.inventory.Count > 0)
        {
            itemButton.gameObject.SetActive(true);
        }
        if (currentGroup.enemyGroup.fleeable)
        {
            runButton.gameObject.SetActive(true);
        }
    }
}
