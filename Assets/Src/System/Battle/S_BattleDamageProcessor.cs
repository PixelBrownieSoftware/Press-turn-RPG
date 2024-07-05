using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BattleDamageProcessor : MonoBehaviour
{
    public R_BattleCharacterList players;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList goneEnemies;
    public R_BattleCharacterList gonePlayers;
    [SerializeField]
    private R_Move selectedMoveRef;
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
    private S_HitObjSpawner hitObjectSpawner;
    [SerializeField]
    private R_BattleCharacter currentCharacterRef;
    [SerializeField]
    private R_BattleCharacter targetProcessCharacterRef;   //This is the target that this processor is processing on, not nessacariliy the target that the player selected
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
    private CH_SoundPitch soundPlayer;
    [SerializeField]
    private R_SoundEffect damagePlayerSound;
    [SerializeField]
    private R_SoundEffect damageEnemySound;

    List<float> modifiers = new List<float>();
    float elementWeakness = 0;

    private void OnEnable()
    {
        healProcess.OnFunctionEvent += HealCharacter;
        damageProcess.OnFunctionEvent += DamageCharacter;
        healNumberProcess.OnFunctionEvent += HealCharacterAmount;
        damageNumberProcess.OnFunctionEvent += DamageCharacterAmount;
        damageTurnFlagProcess.OnFunctionEvent += PredictTurnFlag;
    }

    private void OnDisable()
    {
        healProcess.OnFunctionEvent -= HealCharacter;
        damageProcess.OnFunctionEvent -= DamageCharacter;
        healNumberProcess.OnFunctionEvent -= HealCharacterAmount;
        damageNumberProcess.OnFunctionEvent -= DamageCharacterAmount;
        damageTurnFlagProcess.OnFunctionEvent -= PredictTurnFlag;
    }


    public IEnumerator SpawnHitObject(int damage, Vector2 pos, string hitObjType)
    {
        hitObjectType.Set(hitObjType);
        hitObjectDamage.Set(Mathf.Abs(damage));
        hitObjectPosition.Set(pos);
        hitObjectSpawner.hitObjectPool.Get();
        yield return new WaitForSeconds(0.6f);
    }

    public void PredictTurnFlag()
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
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
        bool willHit = S_Calculation.PredictStatChance(currentCharacter.dexterity, targetCharacter.agility, 0.65f);
        if (!willHit)
        {
            hitObjectPressTurnFlag.Set((int)TURN_FLAG.MISS);
        }
        if (finalPressTurnFlag.integer < hitObjectPressTurnFlag.integer)
        {
            int changedFlag = hitObjectPressTurnFlag.integer;
            finalPressTurnFlag.integer = changedFlag;
        }
    }

    public void DamageCharacterAmount(int damage)
    {
        string hitObjDamageType = "";
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        if (players.Contains(targetCharacter))
        {
            soundPlayer.RaiseEvent(damagePlayerSound);
            hitObjDamageType = "damage_player";
            hitObjectColour.Set(targetCharacter.baseCharacterData.characterColour);
        }
        else
        {
            soundPlayer.RaiseEvent(damageEnemySound);
            hitObjDamageType = "damage_enemy";
            hitObjectColour.Set(Color.white);
        }
        targetCharacter.characterHealth.health -= damage;
        if (targetCharacter.onHurt != null) {
            targetCharacter.onHurt.Invoke();
        }
        targetCharacter.characterHealth.health = Mathf.Clamp(targetCharacter.characterHealth.health, 0, targetCharacter.characterHealth.maxHealth);
        StartCoroutine(DamageAction(damage, targetCharacter.position, hitObjDamageType));
    }
    public void DamageCharacter()
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        int damage = S_Calculation.CalculateDamage(currentCharacter, targetCharacter, selectedMoveRef.move, modifiers, 0);
        DamageCharacterAmount(damage);

    }
    public void HealCharacterAmount(int damage)
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        string hitObjDamageType = "";

        targetCharacter.characterHealth.health += damage;
        targetCharacter.characterHealth.health = Mathf.Clamp(targetCharacter.characterHealth.health, 0, targetCharacter.characterHealth.maxHealth);
        hitObjDamageType = "heal_hp";
        Debug.Log(currentCharacter.name + " dealt " + damage + " damage to " + targetCharacter.name);
        StartCoroutine(DamageAction(damage, targetCharacter.position, hitObjDamageType));
    }
    public void HealCharacter()
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;

        int damage = selectedMoveRef.move.power;
        HealCharacterAmount(damage);
    }
    public void DamageCalculation2()
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        O_BattleCharacter currentCharacter = currentCharacterRef.battleCharacter;
        string hitObjDamageType = "";
        if (!selectedMoveRef.move.isHeal)
        {
            int damage = S_Calculation.CalculateDamage(currentCharacter, targetCharacter, selectedMoveRef.move, modifiers, 0);

            targetCharacter.characterHealth.health += -damage;
            S_StatusInflict[] inflictStatus = selectedMoveRef.move.element.statusInflict;
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
            StartCoroutine(DamageAction(damage, targetCharacter.position, hitObjDamageType));
        }
    }

    private IEnumerator DamageAction(int damage, Vector2 position, string hitObjDamageType)
    {
        O_BattleCharacter targetCharacter = targetProcessCharacterRef.battleCharacter;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(SpawnHitObject(damage, position, hitObjDamageType));
        if (targetCharacter != null)
        {
            if (targetCharacter.onDefeat != null)
            {
                targetCharacter.onDefeat.Invoke();
            }
            if (!targetCharacter.revivable && targetCharacter.characterHealth.health <= 0) {
                if (players.Contains(targetCharacter)) {
                    players.battleCharList.Remove(targetCharacter);
                    gonePlayers.Add(targetCharacter);
                } else {
                    enemies.battleCharList.Remove(targetCharacter);
                    goneEnemies.Add(targetCharacter);
                }
            }
        }
    }

}
