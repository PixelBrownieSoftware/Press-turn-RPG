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
    private R_BattleCharacter targetProcessCharacterRef;
    [SerializeField]
    private CH_Func preformCH;
    [SerializeField]
    private CH_Func turnHandler; 
    [SerializeField]
    private CH_Func damageProcess;
    [SerializeField]
    private CH_Int damageNumberProcess;
    [SerializeField]
    private CH_Func healProcess;
    [SerializeField]
    private CH_Int healNumberProcess;
    [SerializeField]
    private CH_Func damageTurnFlagProcess;

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
    [SerializeField]
    private R_Text projectileType;

    [SerializeField]
    private CH_SoundPitch soundPlayer;
    [SerializeField]
    private S_ProjectileSpawner projectileSpawner;

    private void OnEnable()
    {
        preformCH.OnFunctionEvent += ExcectcuteTurnFunction;
    }

    private void OnDisable()
    {
        preformCH.OnFunctionEvent -= ExcectcuteTurnFunction;
    }


    public void ProcessMove() {
        if (selectedMoveRef.move.isHeal)
        {
            hitObjectPressTurnFlag.Set((int)TURN_FLAG.NORMAL);
            healProcess.RaiseEvent();
        }
        else
        {
            damageTurnFlagProcess.RaiseEvent();
            damageProcess.RaiseEvent();
        }
    }

    public IEnumerator PlayAttackAnimation(O_BattleCharacter user, O_BattleCharacter targ, S_ActionAnim[] animations)
    {

        Vector3 dir = new Vector2(0, 0);
        Vector3 originalPos = new Vector2(0, 0);
        Vector3 targPos = new Vector2(0, 0);
        if (targ != null)
            targPos = targ.position;
        if (user != null)
            originalPos = user.position;
        targetProcessCharacterRef.SetCharacter(targ);
        if (animations != null)
        {
            if (animations.Length > 0)
            {
                foreach (S_ActionAnim an in animations)
                {
                    float timer = 0;
                    switch (an.actionType)
                    {
                        case S_ActionAnim.ACTION_TYPE.CHAR_ANIMATION:
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
                                while (timer < user.getAnimHandlerState)
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                            }
                            break;

                        case S_ActionAnim.ACTION_TYPE.ANIMATION:
                            {
                                projectileType.Set(an.name);
                                O_ProjectileAnim projectile = projectileSpawner.projectilePool.Get();
                                projectile.transform.position = targ.position;
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
                                }
                                */
                                while (timer < projectile.GetAnimHandlerState())
                                {
                                    timer += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                                projectile.DespawnObject();
                            }
                            break;

                        case S_ActionAnim.ACTION_TYPE.WAIT:
                            yield return new WaitForSeconds(an.time);
                            break;

                        case S_ActionAnim.ACTION_TYPE.PLAY_SOUND:
                            soundPlayer.RaiseEvent(an.sound, an.soundPitchMin, an.soundPitchMax);
                            yield return new WaitForSeconds(an.time);
                            break;

                        case S_ActionAnim.ACTION_TYPE.CALCULATION:
                            ProcessMove();
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
        }
    }


    public IEnumerator PreformAction()
    {
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        O_BattleCharacter targetCharacter = selectedTargetCharacterRef.battleCharacter;
        bool playerTeam = false;
        Debug.Log(currentCharacter.name + " used " + selectedMoveRef.move.name + " on " + targetCharacter.name);
        int amountOfTimes = Random.Range(selectedMoveRef.move.minRepeat, selectedMoveRef.move.maxRepeat);
        Debug.Log("Repeat: " + amountOfTimes);
        yield return StartCoroutine(PlayAttackAnimation(currentCharacter, targetCharacter, selectedMoveRef.move.pre_animations));
        for (int repeat = 0; repeat < amountOfTimes; repeat++)
        {
            switch (selectedMoveRef.move.targetScope)
            {
                case S_Move.TARGET_SCOPE.SINGLE:
                    yield return StartCoroutine(PlayAttackAnimation(currentCharacter, targetCharacter, selectedMoveRef.move.animations));
                    break;

                case S_Move.TARGET_SCOPE.ALL:
                    playerTeam = players.Find(x => x == targetCharacter, targetCharacter);
                    if (playerTeam)
                    {
                        for (int i = 0; i < players.battleCharList.Count; i++)
                        {
                            O_BattleCharacter bc = players.battleCharList[i];
                            yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc, selectedMoveRef.move.animations));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < enemies.battleCharList.Count; i++)
                        {
                            O_BattleCharacter bc = enemies.battleCharList[i];
                            yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc, selectedMoveRef.move.animations));
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
                            yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc, selectedMoveRef.move.animations));
                        }
                    }
                    else
                    {
                        for (int i = leftmostPos; i < rightmostPos; i++)
                        {
                            O_BattleCharacter bc = enemies.battleCharList[i];
                            yield return StartCoroutine(PlayAttackAnimation(currentCharacter, bc, selectedMoveRef.move.animations));
                        }
                    }
                    break;
            }
        }
        if(currentCharacter != null)
            currentCharacter.playAnimation.Invoke("idle");
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator CheckForDamageStatusEffects()
    {
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
                    healNumberProcess.RaiseEvent(affect);
                    //yield return StartCoroutine(SpawnHitObject(affect, currentCharacter.position, hitObjDamageType));
                }
                else
                {
                    damageNumberProcess.RaiseEvent(affect);
                    //yield return StartCoroutine(SpawnHitObject(affect, currentCharacter.position, hitObjDamageType));
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