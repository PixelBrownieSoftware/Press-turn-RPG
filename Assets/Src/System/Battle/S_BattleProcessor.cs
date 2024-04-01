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


    public IEnumerator PlayAttackAnimation(S_ActionAnim[] animations, O_BattleCharacter targ, O_BattleCharacter user)
    {

        Vector3 dir = new Vector2(0, 0);
        Vector3 originalPos = new Vector2(0, 0);
        Vector3 targPos = new Vector2(0, 0);
        if (targ != null)
            targPos = targ.position;
        if (user != null)
            originalPos = user.position;

        if (animations.Length > 0)
        {
            foreach (S_ActionAnim an in animations)
            {
                float timer = 0;
                switch (an.actionType)
                {
                    case S_ActionAnim.ACTION_TYPE.CHAR_ANIMATION:
                        /*
                        user.playAnimation.Invoke(an.name);

                        if (an.time > 0)
                        {
                            while (timer < an.time)
                            {
                                timer += Time.deltaTime;
                                yield return new WaitForSeconds(Time.deltaTime);
                            }
                        }
                        else
                        {
                            while (timer < user.GetAnimHandlerState())
                            {
                                timer += Time.deltaTime;
                                yield return new WaitForSeconds(Time.deltaTime);
                            }
                        }
                        */
                        break;

                    case S_ActionAnim.ACTION_TYPE.ANIMATION:
                        {
                            /*
                            Vector2 start = new Vector2(0, 0);
                            switch (an.start)
                            {
                                case S_ActionAnim.MOTION.SELF:
                                    start = user.position;
                                    break;
                                case S_ActionAnim.MOTION.TO_TARGET:
                                    if (targ != null)
                                        start = targ.position;
                                    break;
                            }
                            hitObjType.Set(an.name);
                            O_ProjectileAnim projectile = projectileSpawner.projectilePool.Get();
                            StartCoroutine(PlayProjectileAnimation(projectile, start, start));
                            if (an.time > 0)
                            {
                                while (timer < an.time)
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                            }
                            else
                            {
                                while (timer < projectile.GetAnimHandlerState())
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                            }
                            */
                        }
                        break;

                    case S_ActionAnim.ACTION_TYPE.WAIT:
                        yield return new WaitForSeconds(an.time);
                        break;



                    case S_ActionAnim.ACTION_TYPE.CALCULATION:
                        yield return StartCoroutine(DamageAction(user, targ));
                        break;

                        /*
                    case s_actionAnim.ACTION_TYPE.FADE_SCREEN:
                        fadeFunc.Fade(an.endColour);
                        yield return new WaitForSeconds(0.75f);
                        break;
                        */

                    case S_ActionAnim.ACTION_TYPE.PROJECTILE:
                        {
                            Vector2 start = new Vector2(0, 0);
                            Vector2 end = new Vector2(0, 0);

                            switch (an.start)
                            {
                                case S_ActionAnim.MOTION.SELF:
                                    start = user.position;
                                    break;
                                case S_ActionAnim.MOTION.TO_TARGET:
                                    start = targ.position;
                                    break;
                            }

                            switch (an.goal)
                            {
                                case S_ActionAnim.MOTION.SELF:
                                    end = user.position;
                                    break;
                                case S_ActionAnim.MOTION.TO_TARGET:
                                    end = targ.position;
                                    break;
                            }
                            /*
                            hitObjType.Set(an.name);
                            O_ProjectileAnim projectile = projectileSpawner.projectilePool.Get();
                            yield return StartCoroutine(PlayProjectileAnimation(projectile, start, end));
                            */
                        }
                        break;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            yield return StartCoroutine(DamageAction(user, targ));
        }
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
    public IEnumerator DamageAction(O_BattleCharacter currentCharacter, O_BattleCharacter targetCharacter) {

        int damage = 0;
        damage = S_Calculation.CalculateDamage(currentCharacter, targetCharacter, selectedMoveRef.move, null, 0);
        targetCharacter.Damage(damage);

        Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + targetCharacter.name);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(SpawnHitObject(damage, targetCharacter.position, targetCharacter));
    }

    public IEnumerator PreformAction()
    {
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        O_BattleCharacter targetCharacter = selectedTargetCharacterRef.battleCharacter;
        bool playerTeam = false;
        switch (selectedMoveRef.move.targetScope)
        {
            case S_Move.TARGET_SCOPE.SINGLE:
                break;

            case S_Move.TARGET_SCOPE.ALL:
                playerTeam = players.Find(x => x == targetCharacter, targetCharacter);
                if (playerTeam)
                {
                    for (int i = 0; i < players.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = players.battleCharList[i];
                        yield return StartCoroutine(DamageAction(currentCharacter, bc));


                    }
                }
                else
                {
                    for (int i = 0; i < enemies.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        yield return StartCoroutine(DamageAction(currentCharacter, bc));
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
                        yield return StartCoroutine(DamageAction(currentCharacter, bc));
                    }
                }
                else
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        yield return StartCoroutine(DamageAction(currentCharacter, bc));
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
