using UnityEngine;
using static CombatUnit;

public class IceGreatSword : CharacterSkill
{
    private void Awake()
    {
        characterCombatLive = GetComponent<CharacterCombatLive>();
        npcBehavior = GetComponent<NPCBehavior>();
    }
    private void OnEnable()
    {
        CombatEventManager.IceDebuff += AbleCombo;
    }
    private void OnDisable()
    {
        CombatEventManager.IceDebuff -= AbleCombo;
    }
    override protected float BasicAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("ice basic" + attackNum);
        if (attackNum == 1) 
            return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 0.26f);
        return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 0.39f);
    }
    override protected float HeavyAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("ice heavy");
        if (!npcBehavior.enabled)
        {
            CombatEventManager.BroadcastHeavy();
            characterCombatLive.BattleSkillAmountAdd(10f);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 0.52f);
        //100frame get 10sp
        //70%, 6sp/s
    }
    override protected float Combo(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("ice combo");
        enemy.Crush(characterCombatLive);
        if (enemy.CombatUnitData.debuffLevels[Cryo] > 0)
        {
            return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 4.2f);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 2.4f);
    }
    override protected float Short(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("ice short");
        enemy.AddInfliction(characterCombatLive, Cryo);
        return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 2.34f);
    }
    override protected float Long(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("ice long");
        enemy.AddDebuff(characterCombatLive, Cryo, 1);
        return enemy.CharacterCauseDamage(characterCombatLive, Cryo, attackNum, 8.21f); //90sp
    }
}
