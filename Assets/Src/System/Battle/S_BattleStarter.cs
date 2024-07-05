using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_BattleStarter : MonoBehaviour
{
    public CH_Func onSceneLoad;
    public R_BattleCharacterList partyMembers;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;
    public s_hpBoxGUI[] playerHPGUI;
    public S_BattleActorsManager battleactorManager;
    public R_BattleCharacter bcfactoryOutput;
    public CH_BattleGroupMember bcfactoryInput;
    public CH_Func callCheckTurns;
    public CH_Func queueCharacters;
    public R_BattleGroup enemyGroup;
    public R_Boolean isPlayer;
    public R_Int turnIcons;
    public R_Int pressedIcons;

    private void OnEnable()
    {
        onSceneLoad.OnFunctionEvent += InitialiseOnload;
    }

    private void OnDisable()
    {
        onSceneLoad.OnFunctionEvent -= InitialiseOnload;
    }

    void InitialiseOnload()
    {
        turnIcons.integer = 0;
        pressedIcons.integer = 0;
        isPlayer.boolean = false;
        {
            int i = 0;
            foreach (var character in partyMembers.battleCharList)
            {
                players.Add(character);
                O_BattleCharacter c = character;
                c.characterHealth.health = c.characterHealth.maxHealth;
                c.characterHealth.stamina = 2;
                battleactorManager.AssignCharacterToActor(i, ref c, true);
                playerHPGUI[i].gameObject.SetActive(true);
                playerHPGUI[i].bc = character;
                i++;
            }
        }
        {
            int i = 0;
            foreach (var character in enemyGroup.battleGroup.members_Enemy)
            {
                bcfactoryInput.RaiseEvent(character);
                O_BattleCharacter c = bcfactoryOutput.battleCharacter;
                enemies.Add(c);
                battleactorManager.AssignCharacterToActor(i, ref c, false);
                i++;
            }
        }
        //queueCharacters.RaiseEvent();
        callCheckTurns.RaiseEvent();
    }
}
