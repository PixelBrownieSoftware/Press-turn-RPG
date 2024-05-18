using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;

public class S_BattleBehaviour : MonoBehaviour
{
    public S_BattleSystem battleSystem;
    public R_BattleCharacter currentCharacter;
    [SerializeField]
    private R_Move currentMoveRef;
    [SerializeField]
    private R_MoveList currentMoveListRef;
    [SerializeField]
    private R_BattleCharacter selectedTargetRef;
    [SerializeField]
    private R_BattleCharacterList selectedTargetsRef;
    [SerializeField]
    private CH_Func displayMoves;
    [SerializeField]
    private CH_Func displayTargets;
    [SerializeField]
    private CH_Func receiveAICall;
    [SerializeField]
    List<RPGB_Actions> rpgActionsList = new List<RPGB_Actions>();

    [System.Serializable]
    [SerializeField]
    private class RPGB_Actions
    {
        public int rent;
        public S_Move move;
        public List<O_BattleCharacter> targets = new List<O_BattleCharacter>();
        public RPGB_Actions(int rent, O_BattleCharacter[] targets, S_Move move)
        {
            this.rent = rent;
            this.targets = targets.ToList();
            this.move = move;
        }
    }

    private void OnEnable()
    {
        receiveAICall.OnFunctionEvent += BehaviourSystem;
    }

    private void OnDisable()
    {
        receiveAICall.OnFunctionEvent -= BehaviourSystem;
    }
    public void SelectMove(S_Move move)
    {
        currentMoveRef.SetMove(move);
        displayTargets.RaiseEvent();
        O_BattleCharacter[] targets = selectedTargetsRef.battleCharList.ToArray();
        rpgActionsList.Add(new RPGB_Actions(
                0,
                targets,
                move
            ));
    }

    public void BehaviourSystem()
    {
        rpgActionsList.Clear();
        rpgActionsList = new List<RPGB_Actions>();
        displayMoves.RaiseEvent();
        foreach (var move in currentMoveListRef.moveListRef)
        {
            SelectMove(move);
        }
        S_RPGBehaviourScript sc = currentCharacter.battleCharacter.baseCharacterData.characterBehaviour;

        int bestSettingRent = -1;
        List<List<RPGB_Actions>> listOfBestActions = new List<List<RPGB_Actions>>();
        //goto fuckallthatbullshit; 
        foreach (var setting in sc.settings)
        {
            List<RPGB_Actions> tempList = rpgActionsList.ToList();
            Dictionary<Predicate<S_Move>, int> moveCond = new Dictionary<Predicate<S_Move>, int>();
            Dictionary<Predicate<O_BattleCharacter>, int> targetCond = new Dictionary<Predicate<O_BattleCharacter>, int>();
            foreach (var cond in setting.conditions)
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);
                if (chance > cond.rentChance)
                {
                    continue;
                }
                if (cond.isMove)
                {
                    switch (cond.conditionType)
                    {
                        case RPGB_Condition.CONDITION_TYPE.TYPE_HEALING:
                            moveCond.Add(x => x.isHeal, 4);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.TYPE_OFFENSIVE:
                            moveCond.Add(x => !x.isHeal, 3);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.COST_LOWER:
                            moveCond.Add(x => x.cost < currentCharacter.battleCharacter.characterHealth.stamina, 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.COST_LOWER_EQUAL:
                            moveCond.Add(x => x.cost <= currentCharacter.battleCharacter.characterHealth.stamina, 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.MOVE_OF_ELEMENT:
                            moveCond.Add(x => x.element == cond.elementConstrain, 1);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.POWER_LESS:
                            moveCond.Add(x => x.power < cond.amount, 3);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.POWER_EQUAL:
                            moveCond.Add(x => x.power == cond.amount, 3);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.POWER_LESS_EQUAL:
                            moveCond.Add(x => x.power <= cond.amount, 3);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.POWER_GREATER_EQUAL:
                            moveCond.Add(x => x.power >= cond.amount, 3);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.POWER_GREATER:
                            moveCond.Add(x => x.power > cond.amount, 3);
                            break;
                    }
                }
                else
                {
                    switch (cond.conditionType)
                    {
                        case RPGB_Condition.CONDITION_TYPE.HEALTH_GREATER:
                            targetCond.Add(x => x.characterHealth.health > x.characterHealth.maxHealth * cond.percentage, 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.HEALTH_EQUAL:
                            targetCond.Add(x => x.characterHealth.health == x.characterHealth.maxHealth * cond.percentage, 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.HEALTH_LOWER:
                            targetCond.Add(x => x.characterHealth.health < x.characterHealth.maxHealth * cond.percentage, 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.ELEMENTAL_AFFINITY_EQUAL:
                            targetCond.Add(x => cond.percentage == x.GetElementWeakness(cond.elementConstrain), 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.ELEMENTAL_AFFINITY_GREATER_EQUAL:
                            targetCond.Add(x => cond.percentage <= x.GetElementWeakness(cond.elementConstrain), 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.ELEMENTAL_AFFINITY_GREATER:
                            targetCond.Add(x => cond.percentage < x.GetElementWeakness(cond.elementConstrain), 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.ELEMENTAL_AFFINITY_LOWER:
                            targetCond.Add(x => cond.percentage > x.GetElementWeakness(cond.elementConstrain), 2);
                            break;
                        case RPGB_Condition.CONDITION_TYPE.ELEMENTAL_AFFINITY_LOWER_EQUAL:
                            targetCond.Add(x => cond.percentage >= x.GetElementWeakness(cond.elementConstrain), 2);
                            break;
                    }
                }
            }
            int bestRent = -1;
            //Filter the rpg actions to very specific moves and targets
            for (int i = 0; i < tempList.Count; i++)
            {
                //Action = Move -> Targets[]
                var action = tempList[i];
                bool moveFail = false;
                int rent = 0;
                foreach (var cond in moveCond)
                {
                    if (!cond.Key.Invoke(action.move))
                    {
                        //If the move cannot fufil the requirements, there isn't any point to finding targets
                        moveFail = true;
                        break;
                    }
                    rent += cond.Value;
                }
                if (moveFail)
                    continue;

                List<O_BattleCharacter> targsToRemove = new List<O_BattleCharacter>();
                foreach (var targ in action.targets)
                {
                    foreach (var cond in targetCond)
                    {
                        if (!cond.Key.Invoke(targ))
                        {
                            //If that target does not fufil the conditions for targets, remove them
                            targsToRemove.Add(targ);
                            break;
                        }
                        rent += cond.Value;
                    }
                }
                foreach (var targ in targsToRemove)
                {
                    action.targets.Remove(targ);
                }
                //What's the point of having a move when it has no targets?
                if (action.targets.Count == 0)
                {
                    tempList.Remove(action);
                }
                //If other options are better than this one, remove this one.
                if (bestRent > rent)
                {
                    tempList.Remove(action);
                    continue;
                }
                action.rent = rent;
            }
            //Remove any redundancies
            tempList.RemoveAll(x => x.rent < bestRent);
            if (bestSettingRent <= bestRent) {
                //listOfBestActions
                bestSettingRent = bestRent;
                listOfBestActions.Add(tempList);
            }
        }
        //fuckallthatbullshit:
        if (listOfBestActions.Count > 0)
        {
            rpgActionsList = listOfBestActions[UnityEngine.Random.Range(0, listOfBestActions.Count)];
        }
        RPGB_Actions bestAction = rpgActionsList[UnityEngine.Random.Range(0, rpgActionsList.Count)];
        currentMoveRef.SetMove(bestAction.move);
        if (bestAction.targets != null)
        {
            selectedTargetRef.SetCharacter(bestAction.targets[UnityEngine.Random.Range(0, bestAction.targets.Count)]);
        }
    }
}