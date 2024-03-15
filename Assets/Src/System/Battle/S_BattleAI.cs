using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S_BattleAI : MonoBehaviour
{
    public S_BattleSystem battleSystem;
    public O_BattleCharacter currentCharacter;
    [SerializeField]
    private R_Move currentMoveRef;
    [SerializeField]
    private R_BattleCharacter selectedTargetRef;
    [SerializeField]
    private CH_Func ececuteBattleSystemFunction;
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

    public void SelectMove(S_Move move) {
        currentMoveRef.SetMove(move);
        O_BattleCharacter[] targets = battleSystem.DisplayTargets();
        foreach (var target in targets)
        {
            rankingsAI.Add(new S_AI_Ranking(
                    Random.Range(0.2f, 1f),
                    target,
                    move
                ));
        }
    }

    public void AISystem(O_BattleCharacter character) {
        currentCharacter = character;
        rankingsAI.Clear();
        rankingsAI = new List<S_AI_Ranking>();
        S_Move[] moves = battleSystem.DisplayMoves();
        foreach (var move in moves) {
            SelectMove(move);
        }
        S_AI_Ranking bestAction = rankingsAI.OrderByDescending(r => r.score).First();
        currentMoveRef.SetMove(bestAction.move);
        selectedTargetRef.SetCharacter(bestAction.target);
        ececuteBattleSystemFunction.RaiseEvent();
    }
}
