using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public struct Amount_NumFlag { 
    public int amount;
    public TURN_FLAG flag;
}
public struct Amount_NumStatus
{
    public int amount;
}

public enum TURN_FLAG
{
    NORMAL,
    WEAK,
    CRITICAL,
    MISS,
    NULL,
    REPEL,
    ABSORB
}

/// <summary>
/// TODO: 
/// retire the term S_BattleSystem and decouple it into 4 components:
/// 
/// 1. S_BattleCharcterQueue - Manages who's turn it is. Uses the boolean variable "IsPlayerTurn", it compares it with the scriptable object
/// version, if it's not the same - it will change that variable and then switch queue to the opposing team.
/// Tailoff - It decides whether or not to call the menu (player) or AI (enemy)
/// 
/// 2. S_BattleAI/Menu (ALREADY DONE) 
/// Tailoff - Calls the exectution of the processor based on the selected character, current character and current move.
/// 
/// 2.5. S_BattleOptions
/// The AI and menus use this to get skills and targets.
/// 
/// 3. S_BattleProcessor
/// Does the animations and nessacary calculations on damage.
/// Tailoff - Determines the turn flag based on the calculations i.e. Weakness explotation, missing attacks, blocked attacks
/// 
/// 4. S_BattleTurnHandler
/// Looks at the turn flag and does some maths on the press turns.
/// Tailoff - If the net turns are 0, it will switch the scriptable object of IsPlayerTurn which the characterqueue will read.
/// 
/// 5. S_BattleEndCheck
/// Looks at whether or not the player or enemy team are defeated.
/// Tailoff - It will do a game over or victory if one of the conditions are true, otherwise it will call the battlequeue
/// </summary>

public class S_BattleSystem : MonoBehaviour
{
    public O_BattleCharacter currentCharacter;
    public S_MoveDatabase universalMoves;
    public R_MoveList currentCharacterMoves;
    [SerializeField]
    private R_Move selectedMoveRef;
    [SerializeField]
    public R_BattleCharacterList targetCharactersRef;
    [SerializeField]
    private R_BattleCharacter selectedTargetCharacter;
    [SerializeField]
    private CH_Func excecuteSystemChannel;
    [SerializeField]
    private CH_Func excecuteDisplayMovesChannel;
    [SerializeField]
    private CH_Func excecuteDisplayTargetsChannel;


    public T_CreatePartyMembers testCreatorParty;


    private void Start()
    {
        /*
        playerRound = false;
        CheckTurns();
        CharacterSelect();
        */
    }
    /*
    public void DisplayMovesFunction()
    {
        List<S_Move> moves = new List<S_Move>();
        foreach (S_Move mv in universalMoves.moves)
        {
            moves.Add(mv);
        }
        foreach (S_Move mv in currentCharacter.baseCharacterData.moves)
        {
            moves.Add(mv);
        }
        foreach (S_Move mv in currentCharacter.extraSkills)
        {
            moves.Add(mv);
        }
        currentCharacterMoves.SetMoves(moves);
    }

    public S_Move[] DisplayMoves() {
        List<S_Move> moves = new List<S_Move>();
        foreach (S_Move mv in universalMoves.moves) {
            moves.Add(mv);
        }
        foreach (S_Move mv in currentCharacter.baseCharacterData.moves)
        {
            moves.Add(mv);
        }
        foreach (S_Move mv in currentCharacter.extraSkills)
        {
            moves.Add(mv);
        }
        return moves.ToArray();
    }

    public void DisplayTargetsFunction()
    {
        List<O_BattleCharacter> targets = new List<O_BattleCharacter>();
        List<O_BattleCharacter> users = players.battleCharList.Contains(currentCharacter) ? players.battleCharList : enemies.battleCharList;
        List<O_BattleCharacter> adversaries = players.battleCharList.Contains(currentCharacter) ? enemies.battleCharList : players.battleCharList;

        switch (selectedMoveRef.move.factionScope)
        {
            case S_Move.FACTION_SCOPE.ALLY: targets.AddRange(users); break;
            case S_Move.FACTION_SCOPE.FOES: targets.AddRange(adversaries); break;
            case S_Move.FACTION_SCOPE.ALL: targets.AddRange(users); targets.AddRange(adversaries); break;
        }
        if (selectedMoveRef.move.targetDead)
        {
        }
        targetCharactersRef.battleCharList = targets;
    }

    public O_BattleCharacter[] DisplayTargets() {
        List<O_BattleCharacter> targets = new List<O_BattleCharacter>();
        List<O_BattleCharacter> users = players.battleCharList.Contains(currentCharacter) ? players.battleCharList : enemies.battleCharList;
        List<O_BattleCharacter> adversaries = players.battleCharList.Contains(currentCharacter) ? enemies.battleCharList : players.battleCharList;

        switch (selectedMoveRef.move.factionScope) { 
        case S_Move.FACTION_SCOPE.ALLY: targets.AddRange(users); break;
            case S_Move.FACTION_SCOPE.FOES: targets.AddRange(adversaries); break;
            case S_Move.FACTION_SCOPE.ALL: targets.AddRange(users); targets.AddRange(adversaries); break;
        }
        if (selectedMoveRef.move.targetDead) {
        }
        //selectedTargetCharacter
        return targets.ToArray();
    }
    */



}
