using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_BattleProcessor : MonoBehaviour
{
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList players;

    [SerializeField]
    private R_Move selectedMoveRef;
    [SerializeField]
    public R_BattleCharacterList targetCharactersRef;
    [SerializeField]
    private R_BattleCharacter selectedTargetCharacterRef;
    [SerializeField]
    private R_BattleCharacter currentCharacterRef;
    [SerializeField]
    private CH_Func preformCH;
    [SerializeField]
    private CH_Func turnHandler;
    [SerializeField]
    private S_HitObjSpawner hitObjectSpawner;

    [SerializeField]
    private R_Int hitObjectDamage;
    [SerializeField]
    private R_Colour hitObjectColour;
    [SerializeField]
    private R_Vector2 hitObjectPosition;
    [SerializeField]
    private R_Text hitObjectType;

    private void OnEnable()
    {
        preformCH.OnFunctionEvent += ExcectcuteTurnFunction;
    }

    private void OnDisable()
    {
        preformCH.OnFunctionEvent -= ExcectcuteTurnFunction;
    }

    public IEnumerator SpawnHitObject(int damage, Vector2 pos, O_BattleCharacter bc)
    {
        hitObjectDamage.Set(damage);
        if (players.Find(x => x == bc, bc))
        {
            hitObjectType.Set("damage_player");
            hitObjectColour.Set(bc.baseCharacterData.characterColour);
        }
        else
        {
            hitObjectType.Set("damage_enemy");
            hitObjectColour.Set(Color.white);
        }
        hitObjectPosition.Set(pos);
        hitObjectSpawner.hitObjectPool.Get();
        yield return new WaitForSeconds(0.6f);
    }

    public IEnumerator PreformAction()
    {
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        O_BattleCharacter targetCharacter = selectedTargetCharacterRef.battleCharacter;
        int damage = 0;
        bool playerTeam = false;
        switch (selectedMoveRef.move.targetScope)
        {
            case S_Move.TARGET_SCOPE.SINGLE:
                damage = S_Calculation.CalculateDamage(currentCharacter, targetCharacter, selectedMoveRef.move, null, 0);
                targetCharacter.Damage(damage);

                Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + targetCharacter.name);
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(SpawnHitObject(damage, targetCharacter.postion, targetCharacter));
                break;

            case S_Move.TARGET_SCOPE.ALL:
                playerTeam = players.Find(x => x == targetCharacter, targetCharacter);
                if (playerTeam)
                {
                    for (int i = 0; i < players.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = players.battleCharList[i];
                        damage = S_Calculation.CalculateDamage(currentCharacter, bc, selectedMoveRef.move, null, 0);
                        bc.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                        yield return StartCoroutine(SpawnHitObject(damage, bc.postion, bc));
                    }
                }
                else
                {
                    for (int i = 0; i < enemies.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        damage = S_Calculation.CalculateDamage(currentCharacter, bc, selectedMoveRef.move, null, 0);
                        bc.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                        yield return StartCoroutine(SpawnHitObject(damage, bc.postion, bc));
                    }
                }
                break;

            case S_Move.TARGET_SCOPE.AOE:
                playerTeam = players.Find(x => x == targetCharacter, targetCharacter);
                int position = playerTeam ?
                    players.battleCharList.IndexOf(targetCharacter) :
                   enemies.battleCharList.IndexOf(targetCharacter);
                int leftmostPos = position + 1;
                int rightmostPos = position - 1;
                rightmostPos = Mathf.Clamp(rightmostPos, 0, int.MaxValue);
                leftmostPos = Mathf.Clamp(leftmostPos, 0, int.MaxValue);
                if (playerTeam)
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        O_BattleCharacter bc = players.battleCharList[i];
                        damage = S_Calculation.CalculateDamage(currentCharacter, bc, selectedMoveRef.move, null, 0);
                        targetCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                        yield return StartCoroutine(SpawnHitObject(damage, bc.postion, bc));
                    }
                }
                else
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        damage = S_Calculation.CalculateDamage(currentCharacter, bc, selectedMoveRef.move, null, 0);
                        targetCharacter.Damage(damage);
                        yield return new WaitForSeconds(0.1f);
                        yield return StartCoroutine(SpawnHitObject(damage, bc.postion, bc));
                    }
                }
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void ExcectcuteTurnFunction()
    {
        StartCoroutine(ExcecuteTurn());
    }

    public IEnumerator ExcecuteTurn()
    {
        yield return StartCoroutine(PreformAction());
        turnHandler.RaiseEvent();
    }
}
