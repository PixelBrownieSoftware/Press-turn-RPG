using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_BattleOptions : MonoBehaviour
{
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public R_BattleCharacter currentCharacter;
    public R_MoveList universalMoves;
    public R_MoveList currentCharacterMoves;
    [SerializeField]
    private R_Move selectedMoveRef;
    [SerializeField]
    public R_BattleCharacterList targetCharactersRef;
    [SerializeField]
    private CH_Func displayMoveFuncitons;
    [SerializeField]
    private CH_Func displayTargetsFuncitons;

    private void OnEnable()
    {
        displayTargetsFuncitons.OnFunctionEvent += DisplayTargetsFunction;
        displayMoveFuncitons.OnFunctionEvent += DisplayMovesFunction;
    }

    private void OnDisable()
    {
        displayTargetsFuncitons.OnFunctionEvent -= DisplayTargetsFunction;
        displayMoveFuncitons.OnFunctionEvent -= DisplayMovesFunction;

    }

    public void DisplayMovesFunction()
    {
        currentCharacterMoves.SetMoves(DisplayMoves().ToList());
    }

    public void DisplayTargetsFunction()
    {
        targetCharactersRef.battleCharList = DisplayTargets().ToList();
    }

    public S_Move[] DisplayMoves()
    {
        List<S_Move> moves = new List<S_Move>();
        O_BattleCharacter ch = currentCharacter.battleCharacter;
        foreach (S_Move mv in universalMoves.moveListRef)
        {
            moves.Add(mv);
        }
        if (ch.baseCharacterData.moves != null)
        {
            foreach (S_Move mv in ch.baseCharacterData.moves)
            {
                moves.Add(mv);
            }
        }
        foreach (S_Move mv in ch.extraSkills)
        {
            moves.Add(mv);
        }
        return moves.ToArray();
    }


    public O_BattleCharacter[] DisplayTargets()
    {
        List<O_BattleCharacter> targets = new List<O_BattleCharacter>();
        List<O_BattleCharacter> users = players.battleCharList.Contains(currentCharacter.battleCharacter) ? players.battleCharList : enemies.battleCharList;
        List<O_BattleCharacter> adversaries = players.battleCharList.Contains(currentCharacter.battleCharacter) ? enemies.battleCharList : players.battleCharList;

        switch (selectedMoveRef.move.factionScope)
        {
            case S_Move.FACTION_SCOPE.ALLY: targets.AddRange(users); break;
            case S_Move.FACTION_SCOPE.FOES: targets.AddRange(adversaries); break;
            case S_Move.FACTION_SCOPE.ALL: targets.AddRange(users); targets.AddRange(adversaries); break;
        }
        if (selectedMoveRef.move.targetDead)
        {
        }
        //selectedTargetCharacter
        return targets.ToArray();
    }
}
