using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_CreatePartyMembers : MonoBehaviour
{

    public S_BattleCharacterSetter[] players;
    public S_BattleCharacterSetter[] enemies;
    public R_BattleCharacterList partyMembers;
    public S_BattleGroup playerGroup;

    public R_BattleCharacter characterFactoryOutput;

    public void CreatePlayables() {
        /*
        foreach (var character in playerGroup.members_Player)
        {
            //O_BattleCharacter c = battleCharacterFactory.CreateBattleCharacter(character);
            
            partyMembers.Add(characterFactoryOutput.battleCharacter);
        }
        */
    }

    public O_BattleCharacter[] CreateEnemyCharacters()
    {
        List<O_BattleCharacter> bc = new List<O_BattleCharacter>();
        foreach (var player in enemies)
        {
            O_BattleCharacter BC = new O_BattleCharacter();
            BC.baseCharacterData = player;
            BC.name = player.name + " " + Random.Range(1,30);
            BC.characterHealth = new S_Stats_Util(player.baseHealth.maxHealth, player.baseHealth.maxHealth, 20, 20);
            BC.characterStats = new S_Stats(
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5)
                );
            bc.Add(BC);
        }
        return bc.ToArray();
    }
    public O_BattleCharacter[] CreatePlayerCharacters() {
        List<O_BattleCharacter> bc = new List<O_BattleCharacter>();
        foreach (var player in players)
        {
            O_BattleCharacter BC = new O_BattleCharacter();
            BC.baseCharacterData = player;
            BC.name = player.name + " " + Random.Range(1, 30);
            BC.characterHealth = new S_Stats_Util(player.baseHealth.maxHealth, player.baseHealth.maxHealth, 20, 20);
            BC.characterStats = new S_Stats(
                Random.Range(1,5),
                Random.Range(1, 5), 
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5),
                Random.Range(1, 5)
                );
            bc.Add(BC);
        }
        return bc.ToArray();
    }
}
