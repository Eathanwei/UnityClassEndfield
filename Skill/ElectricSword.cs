using UnityEngine;
using static CombatUnit;

public class ElectricSword : CharacterSkill
{
    private void Awake()
    {
        characterCombatLive = GetComponent<CharacterCombatLive>();
        npcBehavior = GetComponent<NPCBehavior>();
    }
    private void OnEnable()
    {
        CombatEventManager.ConsumeDebuff += AbleCombo;
    }
    private void OnDisable()
    {
        CombatEventManager.ConsumeDebuff -= AbleCombo;
    }
    override protected float BasicAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("electric basic" + attackNum);
        if (attackNum == 1)
            return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 0.36f);
        return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 0.54f);
    }
    override protected float HeavyAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("electric heavy");
        if (!npcBehavior.enabled)
        {
            CombatEventManager.BroadcastHeavy();
            characterCombatLive.BattleSkillAmountAdd(14f);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 0.72f);
        //140frame get 14sp
        //70%, 6sp/s
    }
    override protected float Combo(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("electric combo");
        characterCombatLive.BattleSkillAmountAdd(15);
        if (enemy.CombatUnitData.debuffLevels[Electric] > 0)
        {
            characterCombatLive.BattleSkillAmountAdd(15 + 5 * enemy.CombatUnitData.debuffLevels[Electric]);
            enemy.ConsumeDebuff(characterCombatLive, Electric);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 2.33f);
    }
    override protected float Short(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("electric short");
        enemy.AddInfliction(characterCombatLive, Electric);
        return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 2.67f);
    }
    override protected float Long(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("electric long");
        characterCombatLive.BattleSkillAmountAdd(60);
        enemy.AddDebuff(characterCombatLive, Electric, 1);
        return enemy.CharacterCauseDamage(characterCombatLive, Electric, attackNum, 6f); //90sp
    }
}
