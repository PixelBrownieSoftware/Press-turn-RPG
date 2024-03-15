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
/// 3. S_BattleProcessor
/// Does the animations and nessacary calculations on damage.
/// Tailoff - Determines the turn flag based on the calculations i.e. Weakness explotation, missing attacks, blocked attacks
/// 
/// 4. S_BattleTurnHandler
/// Looks at the turn flag and does some maths on the press turns.
/// Tailoff - If the net turns are 0, it will switch the scriptable object of IsPlayerTurn which the characterqueue will read.
/// </summary>

public class S_BattleSystem : MonoBehaviour
{
    public O_BattleCharacter currentCharacter;
    public S_MoveDatabase universalMoves;
    public R_MoveList currentCharacterMoves;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public R_BattleCharacterList currentQueue;
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

    public Queue<O_BattleCharacter> battleCharacterQueue = new Queue<O_BattleCharacter>();

    public T_CreatePartyMembers testCreatorParty;
    public S_BattleAI AI;

    public bool playerRound = true;
    public R_Int turnIcons;
    public R_Int pressedIcons;
    public R_Int_ReadOnly netIcons;

    private void OnEnable()
    {
        excecuteSystemChannel.OnFunctionEvent += ExcectcuteTurnFunction;
        excecuteDisplayMovesChannel.OnFunctionEvent += DisplayMovesFunction;
        excecuteDisplayTargetsChannel.OnFunctionEvent += DisplayTargetsFunction;
    }

    private void OnDisable()
    {
        excecuteSystemChannel.OnFunctionEvent -= ExcectcuteTurnFunction;
        excecuteDisplayMovesChannel.OnFunctionEvent -= DisplayMovesFunction;
        excecuteDisplayTargetsChannel.OnFunctionEvent -= DisplayTargetsFunction;
    }
    private void Start()
    {
        {
            O_BattleCharacter[] characters = testCreatorParty.CreatePlayerCharacters();
            foreach (var  character in characters)
            {
                players.Add(character);
            }
        }
        {
            O_BattleCharacter[] characters = testCreatorParty.CreateEnemyCharacters();
            foreach (var character in characters)
            {
                enemies.Add(character);
            }
        }
        playerRound = false;
        CheckTurns();
        CharacterSelect();
    }

    public void CharacterSelect() {
        currentCharacter = battleCharacterQueue.Dequeue();
        battleCharacterQueue.Enqueue(currentCharacter);
        if (currentCharacter.characterHealth.health <= 0) {
            CharacterSelect();
            return;
        }
        //TODO: Do a check to see if the character is on the player side
        //For now, we'll just use an rudimentary AI
        AI.AISystem(currentCharacter);
    }
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

    public void CheckTurns() {
        if (netIcons.value == 0)
        {
            battleCharacterQueue.Clear();
            if (playerRound)
            {
                playerRound = false;
                foreach (var enemy in enemies.battleCharList)
                {
                    battleCharacterQueue.Enqueue(enemy);
                    turnIcons.integer++;
                }

            }
            else
            {
                playerRound = true;
                foreach (var player in players.battleCharList)
                {
                    battleCharacterQueue.Enqueue(player);
                    turnIcons.integer++;
                }
            }
        }
    }

    public void EndTurn() {
        if (players.battleCharList.Find(x => x.characterHealth.health > 0) == null)
        {
            Debug.Log("Game over!");
            return;
        }
        else if(enemies.battleCharList.Find(x => x.characterHealth.health > 0) == null)
        {
            Debug.Log("You win!");
            return;
        }
        //Have a function that checks the final outcome of the turn flag
        //For now, we'll just subtract it by 1 turn
        turnIcons.integer--;
        CheckTurns();
        CharacterSelect();
    }

    /// <summary>
    /// This is the attack animation as well as the stuff that calls the damage
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreformAction()
    {
        int damage = 0;
        bool playerTeam = false;
        switch (selectedMoveRef.move.targetScope)
        {
            case S_Move.TARGET_SCOPE.SINGLE:
                damage = S_Calculation.CalculateDamage(currentCharacter, selectedTargetCharacter.battleCharacter, selectedMoveRef.move, null, 0);
                selectedTargetCharacter.battleCharacter.Damage(damage);

                Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + selectedTargetCharacter.name);
                yield return new WaitForSeconds(0.1f);
                break;

            case S_Move.TARGET_SCOPE.ALL:
                playerTeam = players.Find(x => x == selectedTargetCharacter.battleCharacter, selectedTargetCharacter.battleCharacter);
                if (playerTeam)
                {
                    for (int i = 0; i < players.battleCharList.Count; i++)
                    {
                        damage = S_Calculation.CalculateDamage(currentCharacter, players.battleCharList[i], selectedMoveRef.move, null, 0);
                        selectedTargetCharacter.battleCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    for (int i = 0; i < enemies.battleCharList.Count; i++)
                    {
                        damage = S_Calculation.CalculateDamage(currentCharacter, enemies.battleCharList[i], selectedMoveRef.move, null, 0);
                        selectedTargetCharacter.battleCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                break;

            case S_Move.TARGET_SCOPE.AOE:
                playerTeam = players.Find(x => x == selectedTargetCharacter.battleCharacter, selectedTargetCharacter.battleCharacter);
                int position = playerTeam ? 
                    players.battleCharList.IndexOf(selectedTargetCharacter.battleCharacter) :
                   enemies.battleCharList.IndexOf(selectedTargetCharacter.battleCharacter);
                int leftmostPos = position + 1;
                int rightmostPos = position - 1;
                rightmostPos = Mathf.Clamp(rightmostPos, 0, int.MaxValue);
                leftmostPos = Mathf.Clamp(leftmostPos, 0, int.MaxValue);
                if (playerTeam)
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        damage = S_Calculation.CalculateDamage(currentCharacter, players.battleCharList[i], selectedMoveRef.move, null, 0);
                        selectedTargetCharacter.battleCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        damage = S_Calculation.CalculateDamage(currentCharacter, enemies.battleCharList[i], selectedMoveRef.move, null, 0);
                        selectedTargetCharacter.battleCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void ExcectcuteTurnFunction() {
        StartCoroutine(ExcecuteTurn());
    }

    public IEnumerator ExcecuteTurn() {
        yield return StartCoroutine(PreformAction());
        EndTurn();
    }
}
