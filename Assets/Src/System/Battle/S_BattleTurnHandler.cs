using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BattleTurnHandler : MonoBehaviour
{
    public R_Int turnIcons;
    public R_Int pressedIcons;
    public R_Int_ReadOnly netIcons;
    public R_Int turnFlag;
    public R_Boolean isPlayerturn;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public CH_Func callBattleEndState;
    public CH_Func callRequeue;
    public CH_Func receiveTurnHandle;

    private void OnEnable()
    {
        receiveTurnHandle.OnFunctionEvent += ProcessTurn;
    }

    private void OnDisable()
    {
        receiveTurnHandle.OnFunctionEvent -= ProcessTurn;
    }

    public void ProcessTurn() {
        StartCoroutine(ProcessTurnIcon());
    }
    public IEnumerator ProcessTurnIcon() {
        switch ((TURN_FLAG)turnFlag.integer)
        {
            case TURN_FLAG.NORMAL:
                if (turnIcons.integer > 0)
                {
                    turnIcons.integer--;
                }
                else
                {
                    pressedIcons.integer--;
                }
                break;
            case TURN_FLAG.WEAK:
                if (turnIcons.integer > 0)
                {
                    turnIcons.integer--;
                    pressedIcons.integer++;
                }
                else
                {
                    pressedIcons.integer--;
                }
                break;
            case TURN_FLAG.NULL:
                for (int i = 0; i < 2; i++)
                {
                    if (turnIcons.integer > 0)
                    {
                        turnIcons.integer--;
                    }
                    else
                    {
                        pressedIcons.integer--;
                    }
                }
                break;
        }
        if (netIcons.value < 0)
        { 
            pressedIcons.integer = 0;
            turnIcons.integer = 0;
        }    
        Debug.Log(netIcons.value);
        yield return new WaitForSeconds(0.2f);
        CheckTurns();
    }

    public void CheckTurns()
    {
        if (netIcons.value == 0)
        {
            if (isPlayerturn.boolean)
            {
                Debug.Log("OK! Baddies' turn!");
                isPlayerturn.boolean = false;
                foreach (var enemy in enemies.battleCharList)
                {
                    turnIcons.integer++;
                }

            }
            else
            {
                Debug.Log("OK! Goodies' turn!");
                isPlayerturn.boolean = true;
                foreach (var player in players.battleCharList)
                {
                    turnIcons.integer++;
                }
            }
            callRequeue.RaiseEvent();
        }
        callBattleEndState.RaiseEvent();
    }
}
