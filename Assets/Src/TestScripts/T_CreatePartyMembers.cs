using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_CreatePartyMembers : MonoBehaviour
{

    public S_BattleCharacterSetter[] players;
    public S_BattleCharacterSetter[] enemies;

    public O_BattleCharacter[] CreateEnemyCharacters()
    {
        List<O_BattleCharacter> bc = new List<O_BattleCharacter>();
        foreach (var player in enemies)
        {
            O_BattleCharacter BC = new O_BattleCharacter();
            BC.baseCharacterData = player;
            BC.name = player.name;
            BC.characterHealth = new S_Stats_Util(30, 30, 20, 20);
            BC.characterStats = new S_Stats(
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
            BC.name = player.name;
            BC.characterHealth = new S_Stats_Util(30, 30, 20, 20);
            BC.characterStats = new S_Stats(
                Random.Range(1,5),
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
