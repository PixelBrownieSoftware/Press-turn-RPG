using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BattleActorsManager : MonoBehaviour
{
    public O_BattleCharacterActor[] playerActors;
    public O_BattleCharacterActor[] enemyActors;

    public void AssignCharacterToActor(int i, ref O_BattleCharacter character, bool isPlayer) {
        if (isPlayer)
        {
            playerActors[i].battleCharacter = character;
        }
        else
        {
            enemyActors[i].battleCharacter = character;
        }
    }
}
