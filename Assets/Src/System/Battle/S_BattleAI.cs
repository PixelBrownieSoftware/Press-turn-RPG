using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class S_BattleAI : MonoBehaviour
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
    private CH_Func ececuteBattleSystemFunction;
    [SerializeField]
    private CH_Func displayMoves;
    [SerializeField]
    private CH_Func displayTargets;
    [SerializeField]
    private CH_Func receiveAICall;
    List<S_AI_Ranking> rankingsAI = new List<S_AI_Ranking>();

    private struct S_AI_Ranking{
        public float score;
        public O_BattleCharacter target;
        public S_Move move;
        public S_AI_Ranking(float score, O_BattleCharacter target, S_Move move) {
            this.score = score;
            this.target = target;
            this.move = move;
        }
    }

    private void OnEnable()
    {
        receiveAICall.OnFunctionEvent += AISystem;
    }

    private void OnDisable()
    {
        receiveAICall.OnFunctionEvent -= AISystem;
    }
    public void SelectMove(S_Move move) {
        currentMoveRef.SetMove(move);
        displayTargets.RaiseEvent();
        O_BattleCharacter[] targets = selectedTargetsRef.battleCharList.ToArray();
        foreach (var target in targets)
        {
            rankingsAI.Add(new S_AI_Ranking(
                    Random.Range(0.2f, 1f),
                    target,
                    move
                ));
        }
    }

    public void AISystem() {
        rankingsAI.Clear();
        rankingsAI = new List<S_AI_Ranking>();
        displayMoves.RaiseEvent();
        foreach (var move in currentMoveListRef.moveListRef) {
            SelectMove(move);
        }
        S_AI_Ranking bestAction = rankingsAI.OrderByDescending(r => r.score).First();
        currentMoveRef.SetMove(bestAction.move);
        selectedTargetRef.SetCharacter(bestAction.target);
        ececuteBattleSystemFunction.RaiseEvent();
    }
}
