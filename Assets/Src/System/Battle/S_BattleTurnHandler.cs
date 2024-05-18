using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public CH_Func receiveCheckTurns;

    public Image[] pressTurnIcons;

    private void Awake()
    {
        foreach (var img in pressTurnIcons) {
            img.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        receiveTurnHandle.OnFunctionEvent += ProcessTurn;
        receiveCheckTurns.OnFunctionEvent += CheckTurns;
    }

    private void OnDisable()
    {
        receiveTurnHandle.OnFunctionEvent -= ProcessTurn;
        receiveCheckTurns.OnFunctionEvent -= CheckTurns;
    }


    public void ProcessTurn() {
        StartCoroutine(ProcessTurnIcon());
    }
    public IEnumerator ProcessTurnIcon() {
        switch ((TURN_FLAG)turnFlag.integer)
        {
            case TURN_FLAG.NORMAL:
                Debug.Log("Normal");
                if (turnIcons.integer > 0)
                {
                    pressTurnIcons[turnIcons.integer].gameObject.SetActive(false);
                    turnIcons.integer--;
                }
                else
                {
                    pressTurnIcons[pressedIcons.integer].gameObject.SetActive(false);
                    pressedIcons.integer--;
                }
                break;
            case TURN_FLAG.WEAK:
            case TURN_FLAG.CRITICAL:
                Debug.Log("WEAK!");
                if (turnIcons.integer > 0)
                {
                    turnIcons.integer--;
                    pressTurnIcons[pressedIcons.integer].color = Color.magenta;
                    pressedIcons.integer++;
                }
                else
                {
                    pressTurnIcons[pressedIcons.integer].gameObject.SetActive(false);
                    pressedIcons.integer--;
                }
                break;
            case TURN_FLAG.NULL:
            case TURN_FLAG.MISS:
                Debug.Log("Null");
                for (int i = 0; i < 2; i++)
                {
                    if (turnIcons.integer > 0)
                    {
                        pressTurnIcons[turnIcons.integer].gameObject.SetActive(false);
                        turnIcons.integer--;
                    }
                    else
                    {
                        pressTurnIcons[pressedIcons.integer].gameObject.SetActive(false);
                        pressedIcons.integer--;
                    }
                    if (netIcons.value == 0) {
                        break;
                    }
                }
                break;
        }
        if (netIcons.value <= 0)
        { 
            pressedIcons.integer = 0;
            turnIcons.integer = 0;
        }
        turnFlag.integer = 0;
        Debug.Log("Turn icons left: " + turnIcons.integer);
        Debug.Log("Pressed icons left: " + pressedIcons.integer);
        Debug.Log("Total icons left: " + netIcons.value);
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
                    pressTurnIcons[turnIcons.integer].gameObject.SetActive(true);
                    pressTurnIcons[turnIcons.integer].color = Color.white;
                }

            }
            else
            {
                Debug.Log("OK! Goodies' turn!");
                isPlayerturn.boolean = true;
                foreach (var player in players.battleCharList)
                {
                    turnIcons.integer++;
                    pressTurnIcons[turnIcons.integer].gameObject.SetActive(true);
                    pressTurnIcons[turnIcons.integer].color = Color.white;
                }
            }
            callRequeue.RaiseEvent();
        }
        callBattleEndState.RaiseEvent();
    }
}
