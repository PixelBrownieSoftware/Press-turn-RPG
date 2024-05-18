using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BattleEndCheck : MonoBehaviour
{
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public CH_Func callQueue;
    public CH_Func checkBattleEnd;
    public CH_Text menuChange;
    public CH_MapTransfer mapTransfer;

    private void OnEnable()
    {
        checkBattleEnd.OnFunctionEvent += EndTurn;
    }

    private void OnDisable()
    {
        checkBattleEnd.OnFunctionEvent -= EndTurn;
    }

    public void EndTurn()
    {
        if (players.battleCharList.Find(x => x.characterHealth.health > 0) == null)
        {
            Debug.Log("Game over!");
            mapTransfer.RaiseEvent("Hubworld");
            return;
        }
        else if(enemies.battleCharList.Find(x => x.characterHealth.health > 0) == null)
        {
            Debug.Log("You win!");
            menuChange.RaiseEvent("ExperienceMenu");
            return;
        }
        Debug.Log("Lets go to the next guy!");
        callQueue.RaiseEvent();
    }
}
