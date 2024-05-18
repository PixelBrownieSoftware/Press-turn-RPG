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
    private R_Int finalPressTurnFlag;
    [SerializeField]
    private R_Int hitObjectPressTurnFlag;
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


    public IEnumerator PlayAttackAnimation(O_BattleCharacter user, O_BattleCharacter targ)
    {

        Vector3 dir = new Vector2(0, 0);
        Vector3 originalPos = new Vector2(0, 0);
        Vector3 targPos = new Vector2(0, 0);
        if (targ != null)
            targPos = targ.position;
        if (user != null)
            originalPos = user.position;
        S_ActionAnim[] animations = selectedMoveRef.move.animations;

        if (animations.Length > 0)
        {
            foreach (S_ActionAnim an in animations)
            {
                //float timer = 0;
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
                        DamageCalculation(user, targ);
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
            DamageCalculation(user, targ);
        }
    }

    public IEnumerator SpawnHitObject(int damage, Vector2 pos, string hitObjType)
    {
        hitObjectType.Set(hitObjType);
        hitObjectDamage.Set(Mathf.Abs(damage));
        hitObjectPosition.Set(pos);
        hitObjectSpawner.hitObjectPool.Get();
        yield return new WaitForSeconds(0.6f);
    }

    public void DamageCalculation(O_BattleCharacter currentCharacter, O_BattleCharacter targetCharacter) {
        string hitObjDamageType = "";
        if (!selectedMoveRef.move.isHeal)
        {
            float elementWeakness = targetCharacter.GetElementWeakness(selectedMoveRef.move.element);
            List<float> modifiers = new List<float>();
            if (elementWeakness >= 2)
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.WEAK);
            else if (elementWeakness < 2 && elementWeakness > 0)
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.NORMAL);
            else if (elementWeakness == 0)
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.NULL);
            else if (elementWeakness < 0 && elementWeakness > -1)
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.REPEL);
            else if (elementWeakness <= -1)
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.ABSORB);

            if (elementWeakness > 0)
            {
                bool scoreCriticalHit = targetCharacter.IsCritical(selectedMoveRef.move);

                if (scoreCriticalHit)
                {
                    hitObjectPressTurnFlag.Set((int)TURN_FLAG.CRITICAL);
                    modifiers.Add(1.7f);
                }
            }
            int damage = S_Calculation.CalculateDamage(currentCharacter, targetCharacter, selectedMoveRef.move, modifiers, 0);

            bool willHit = S_Calculation.PredictStatChance(currentCharacter.dexterity, targetCharacter.agility, 0.65f);
            if (!willHit)
            {
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.MISS);
            }
            else
            {
                if (players.Contains(targetCharacter))
                {
                    hitObjDamageType = "damage_player";
                    hitObjectColour.Set(targetCharacter.baseCharacterData.characterColour);
                }
                else
                {
                    hitObjDamageType = "damage_enemy";
                    hitObjectColour.Set(Color.white);
                }
                targetCharacter.characterHealth.health += -damage;
                S_Element.S_StatusInflict[] inflictStatus = selectedMoveRef.move.element.statusInflict;
                if (inflictStatus != null)
                {
                    foreach (var statusInflict in inflictStatus)
                    {
                        if (statusInflict.add_remove)
                        {
                            float statusInflictChance = UnityEngine.Random.Range(0, 1f);
                            if (statusInflictChance > statusInflict.chance)
                            {
                                targetCharacter.SetStatus(statusInflict.statusEffect);
                            }
                        }
                        else
                        {
                            float statusRemoveChance = UnityEngine.Random.Range(0, 1f);
                            if (statusRemoveChance > statusInflict.chance)
                            {
                                targetCharacter.RemoveStatus(statusInflict.statusEffect);
                            }
                        }
                    }
                }
            }
            if (finalPressTurnFlag.integer < hitObjectPressTurnFlag.integer)
            {
                int changedFlag = hitObjectPressTurnFlag.integer;
                finalPressTurnFlag.integer = changedFlag;
            }

            Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + targetCharacter.name);
            StartCoroutine(DamageAction(damage, targetCharacter.position, hitObjDamageType));
        }
        else
        {
            int damage = selectedMoveRef.move.power;
            targetCharacter.characterHealth.health += damage;
            targetCharacter.characterHealth.health = Mathf.Clamp(targetCharacter.characterHealth.health, 0, targetCharacter.characterHealth.maxHealth);
            hitObjDamageType = "heal_hp";
            Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + targetCharacter.name);
            StartCoroutine(DamageAction(damage,targetCharacter.position, hitObjDamageType));
        }
    }

    private IEnumerator DamageAction(int damage, Vector2 position, string hitObjDamageType) {

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(SpawnHitObject(damage, position, hitObjDamageType));
    }


    public IEnumerator PreformAction()
    {
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        O_BattleCharacter targetCharacter = selectedTargetCharacterRef.battleCharacter;
        bool playerTeam = false;
        Debug.Log(currentCharacter.name + " used " + selectedMoveRef.move.name + " on " + targetCharacter.name);
        switch (selectedMoveRef.move.targetScope)
        {
            case S_Move.TARGET_SCOPE.SINGLE:
                yield return StartCoroutine(PlayAttackAnimation(currentCharacter, targetCharacter));
                break;

            case S_Move.TARGET_SCOPE.ALL:
                playerTeam = players.Find(x => x == targetCharacter, targetCharacter);
                if (playerTeam)
                {
                    for (int i = 0; i < players.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = players.battleCharList[i];
                        yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc));
                    }
                }
                else
                {
                    for (int i = 0; i < enemies.battleCharList.Count; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc));
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
                        yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc));
                    }
                }
                else
                {
                    for (int i = leftmostPos; i < rightmostPos; i++)
                    {
                        O_BattleCharacter bc = enemies.battleCharList[i];
                        yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc));
                    }
                }
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator CheckForDamageStatusEffects()
    {
        string hitObjDamageType = "";
        ///TODO: Make it so that there is a way that this regenerates/damages HP and SP
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        yield return new WaitForSeconds(0.1f);
        foreach (var status in currentCharacter.statusEffects)
        {
            int affect = status.status.statUtilAffect.health;
            if (affect != 0)
            {
                if (status.status.statUtilRegen)
                {
                    hitObjDamageType = "damage_enemy";
                    currentCharacter.characterHealth.health += affect;
                    yield return StartCoroutine(SpawnHitObject(affect, currentCharacter.position, hitObjDamageType));
                }
                else
                {
                    currentCharacter.characterHealth.health -= affect;

                    if (players.Contains(currentCharacter))
                    {
                        hitObjDamageType = "damage_player";
                        hitObjectColour.Set(currentCharacter.baseCharacterData.characterColour);
                    }
                    else
                    {
                        hitObjDamageType = "damage_enemy";
                        hitObjectColour.Set(Color.white);
                    }
                    yield return StartCoroutine(SpawnHitObject(affect, currentCharacter.position, hitObjDamageType));
                }
            }
        }
        currentCharacter.UpateStatusEffectDuration();
        currentCharacter.UpdateStatusEffectBuffs();

    }

    public void ExcectcuteTurnFunction()
    {
        StartCoroutine(ExcecuteTurn());
    }

    public IEnumerator ExcecuteTurn()
    {
        switch (selectedMoveRef.move.customFunction) {
            default:
                yield return StartCoroutine(PreformAction());
                break;
            case "pass":
                yield return new WaitForSeconds(0.1f);
                hitObjectPressTurnFlag.Set((int)TURN_FLAG.WEAK);
                finalPressTurnFlag.Set((int)TURN_FLAG.WEAK);
                Debug.Log("Turn pass!");
                break;
        }
        yield return StartCoroutine(CheckForDamageStatusEffects());

        turnHandler.RaiseEvent();
    }
}