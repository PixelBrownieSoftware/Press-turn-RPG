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
    public R_BattleCharacterList currentQueue;
    public R_BattleCharacter currentCharacter;
    public S_BattleAI AI;
    public CH_Func callAI;
    public CH_Func receiveQueue;
    public CH_Func receiveNextCharacter;
    public CH_Text changeMenu;
    public T_CreatePartyMembers testCreatorParty;
    public S_BattleActorsManager battleactorManager;

    private void Start()
    {

        {
            int i = 0;
            O_BattleCharacter[] characters = testCreatorParty.CreatePlayerCharacters();
            foreach (var character in characters)
            {
                players.Add(character);
                O_BattleCharacter c = character;
                battleactorManager.AssignCharacterToActor(i, ref c, true);
            }
        }
        {
            int i = 0;
            O_BattleCharacter[] characters = testCreatorParty.CreateEnemyCharacters();
            foreach (var character in characters)
            {
                enemies.Add(character);
                O_BattleCharacter c = character;
                battleactorManager.AssignCharacterToActor(i, ref c, false);
            }
        }
        QueueCharacters();
        CharacterSelect();
    }

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
        Debug.Log(currentCharacter.battleCharacter.name);
        if (currentCharacter.battleCharacter.characterHealth.health <= 0)
        {
            CharacterSelect();
            return;
        }
        //TODO: Do a check to see if the character is on the player side
        //For now, we'll just use an rudimentary AI
        if (isPlayerturn.boolean) { changeMenu.RaiseEvent("SkillsMenu"); } else { callAI.RaiseEvent(); }
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
