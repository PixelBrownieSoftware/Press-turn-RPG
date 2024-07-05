using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BattleActorsManager : MonoBehaviour
{
    public O_BattleCharacterActor[] playerActors;
    public O_BattleCharacterActor[] enemyActors;
    public s_hpBoxGUI[] playerHPGUI;

    public void Awake()
    {
        foreach (var gui in playerHPGUI)
        {
            gui.gameObject.SetActive(false);
        }
        foreach (var actor in playerActors)
        {
            actor.gameObject.SetActive(false);
        }
        foreach (var actor in enemyActors)
        {
            actor.gameObject.SetActive(false);
        }
    }

    public void AssignCharacterToActor(int i, ref O_BattleCharacter character, bool isPlayer) {
        if (isPlayer)
        {
            playerActors[i].gameObject.SetActive(true);
            playerActors[i].animator.runtimeAnimatorController = character.baseCharacterData.animationController;
            playerActors[i].SetCharacter(ref character);
        }
        else
        {
            enemyActors[i].gameObject.SetActive(true);
            enemyActors[i].animator.runtimeAnimatorController = character.baseCharacterData.animationController;
            enemyActors[i].SetCharacter(ref character);
        }
        character.playAnimation.Invoke("idle");
    }
}
