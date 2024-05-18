using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_BattleCharcterQueue : MonoBehaviour
{
    public Queue<O_BattleCharacter> battleCharacterQueue = new Queue<O_BattleCharacter>();
    public R_Boolean isPlayerturn;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public R_BattleCharacterList partyMembers;
    public R_BattleCharacterList currentQueue;
    public R_BattleCharacter currentCharacter;
    public R_BattleGroup enemyGroup;
    public R_BattleGroup playerGroup;
    public R_MoveList movesList;
    public CH_Func callAI;
    //public CH_Func callCheckTurns;
    public CH_Func receiveQueue;
    public CH_Func receiveNextCharacter;
    public CH_Text changeMenu;
    [SerializeField]
    private CH_Func ececuteBattleSystemFunction;
    public S_BattleActorsManager battleactorManager;
    public s_hpBoxGUI[] playerHPGUI;


    private void OnEnable()
    {
        receiveQueue.OnFunctionEvent += QueueCharacters;
        receiveNextCharacter.OnFunctionEvent += CharacterSelect;
    }

    private void OnDisable()
    {
        receiveQueue.OnFunctionEvent -= QueueCharacters;
        receiveNextCharacter.OnFunctionEvent -= CharacterSelect;
    }

    public void CharacterSelect()
    {
        currentCharacter.SetCharacter(battleCharacterQueue.Dequeue());
        battleCharacterQueue.Enqueue(currentCharacter.battleCharacter);
        Debug.Log("Checking " + currentCharacter.battleCharacter.name + "...");
        if (currentCharacter.battleCharacter.characterHealth.health <= 0)
        {
            CharacterSelect();
            Debug.Log(currentCharacter.battleCharacter.name + " is dead ..");
            return;
        }

        /*
        S_RPGBehaviourScript statusBehaviour = currentCharacter.battleCharacter.statusEffects.Find(x => x.status.changedBehaviour).status.changedBehaviour;
        if (statusBehaviour != null) { 
        }
        */
        movesList.AddMoves(currentCharacter.battleCharacter.getCurrentMoves);
        if (isPlayerturn.boolean) {
            //changeMenu.RaiseEvent("SkillsMenu"); MainMenu
            changeMenu.RaiseEvent("MainMenu"); 
        }
        else {
            Debug.Log("AI control!");
            callAI.RaiseEvent();
            ececuteBattleSystemFunction.RaiseEvent();
        }
    }


    public void QueueCharacters() {

        battleCharacterQueue.Clear();
        if (isPlayerturn.boolean)
        {
            foreach (var player in players.battleCharList)
            {
                battleCharacterQueue.Enqueue(player);
            }

        }
        else
        {
            foreach (var enemy in enemies.battleCharList)
            {
                battleCharacterQueue.Enqueue(enemy);
            }
        }
    }
}
