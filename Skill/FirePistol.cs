using System.Collections;
using UnityEngine;
using static CombatUnit;

public class FirePistol : CharacterSkill
{
    Coroutine criticalRateCoroutine;
    private void Awake()
    {
        characterCombatLive = GetComponent<CharacterCombatLive>();
        npcBehavior = GetComponent<NPCBehavior>();
    }
    private void OnEnable()
    {
        CombatEventManager.Infliction += AbleCombo;
    }
    private void OnDisable()
    {
        CombatEventManager.Infliction -= AbleCombo;
    }
    override protected float BasicAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("fire basic" + attackNum);
        if (attackNum == 1)
            return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 0.37f);
        return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 0.56f);
    }
    override protected float HeavyAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("fire heavy");
        if (!npcBehavior.enabled)
        {
            CombatEventManager.BroadcastHeavy();
            characterCombatLive.BattleSkillAmountAdd(14.4f);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 0.75f);
        //144frame get 14.4sp
        //70%, 6sp/s
    }
    override protected float Combo(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("fire combo");
        enemy.AddInfliction(characterCombatLive, Heat);
        return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 0.9f);
    }
    override protected float Short(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("fire short");
        if (enemy.CombatUnitData.debuffLevels[Heat] > 0)
        {
            enemy.ConsumeDebuff(characterCombatLive, Heat);
            if(criticalRateCoroutine != null)
                StopCoroutine(criticalRateCoroutine);
            criticalRateCoroutine = StartCoroutine(CriticalRateCoroutine());
        }
        else
        {
            enemy.AddInfliction(characterCombatLive, Heat);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 2.13f);
    }
    override protected float Long(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("fire long");
        enemy.AddDebuff(characterCombatLive, Heat, 1);
        return enemy.CharacterCauseDamage(characterCombatLive, Heat, attackNum, 2.83f);
    }
    IEnumerator CriticalRateCoroutine()
    {
        characterCombatLive.SetCriticalRate(2);
        yield return new WaitForSeconds(10);
        characterCombatLive.SetCriticalRate(0.05f);
        criticalRateCoroutine = null;
    }
}
