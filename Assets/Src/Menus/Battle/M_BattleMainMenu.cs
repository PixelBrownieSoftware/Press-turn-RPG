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
    private R_Move selectedMove;
    [SerializeField]
    private R_Items inventory;
    [SerializeField]
    private R_BattleGroup currentGroup;
    [SerializeField]
    private R_Int targetMode; //0 for move targeting, 1 for analalyse targeting

    public R_BattleCharacterList targetList;
    public R_BattleCharacterList players;
    public R_BattleCharacterList opponents;

    public B_Function skillsButton;
    public B_Function guardButton;
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
    [SerializeField]
    private CH_Func excecuteDisplayTargetsChannel;
    [SerializeField]
    private CH_Func displayMoves;

    private void OnEnable()
    {
        goToSkills.OnFunctionEvent += GoToSkills;
        goToAnalyse.OnFunctionEvent += GoToAnalysis;
        goToPass.OnFunctionEvent += PassAction;
        //goToGuard.OnFunctionEvent += GuardAction;
        //runFromBattle.OnFunctionEvent += RunFromBattle;
    }

    private void OnDisable()
    {
        goToSkills.OnFunctionEvent -= GoToSkills;
        goToAnalyse.OnFunctionEvent -= GoToAnalysis;
        goToPass.OnFunctionEvent -= PassAction;
        //goToGuard.OnFunctionEvent -= GuardAction;
        //runFromBattle.OnFunctionEvent -= RunFromBattle;
    }

    public void RunFromBattle()
    {
        targetMode.Set(0);
        changeMenu.RaiseEvent("EMPTY");
    }

    public void GoToAnalysis() {
        targetMode.Set(1);
        selectedMove.SetMove(analyseMove);
        excecuteDisplayTargetsChannel.RaiseEvent();
        changeMenu.RaiseEvent("TargetMenu");
    }

    public void GoToSkills()
    {
        targetMode.Set(0);
        changeMenu.RaiseEvent("SkillsMenu");
    }
    public void GuardAction()
    {
        targetMode.Set(0);
        // targetCharacterRef.SetCharacter(currentCharacter);
        selectedMove.SetMove(guard);
        performMove.RaiseEvent();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("EMPTY");
    }
    public void PassAction()
    {
        targetMode.Set(0);
        //targetCharacterRef.SetCharacter(currentCharacter.);
        selectedMove.SetMove(pass);
        performMove.RaiseEvent();
        changeMenu.RaiseEvent("EMPTY");
    }

    public override void StartMenu()
    {
        targetMode.Set(0);
        currentCharacter = currentCharacterRef;
        skillsButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(false);
        base.StartMenu();
        guardButton.gameObject.SetActive(false);
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
        if (currentGroup.enemyGroup.fleeable)
        {
            runButton.gameObject.SetActive(true);
        }
        */
    }
}
