using UnityEngine;
using static CombatUnit;
using System.Collections;

public class NatureMagic : CharacterSkill
{
    private void Awake()
    {
        characterCombatLive = GetComponent<CharacterCombatLive>();
        npcBehavior = GetComponent<NPCBehavior>();
    }
    private void OnEnable()
    {
        CombatEventManager.Heavy += AbleCombo;
    }
    private void OnDisable()
    {
        CombatEventManager.Heavy -= AbleCombo;
    }
    override protected float BasicAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("nature basic" + attackNum);
        if (attackNum == 1)
            return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 0.52f);
        return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 0.78f);
    }
    override protected float HeavyAttack(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("nature heavy");
        if (!npcBehavior.enabled)
        {
            CombatEventManager.BroadcastHeavy();
            characterCombatLive.BattleSkillAmountAdd(20.2f);
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 1.04f);
        //202frame get 20.2sp
        //70%, 6sp/s
    }
    override protected float Combo(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("nature combo");
        characterCombatLive.HealTeam(90f + characterCombatLive.Data.will * 0.75f);
        enemy.AddDebuff(GetComponent<CharacterCombatLive>(), Nature, 1);
        return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 2.1f);
    }
    override protected float Short(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("nature short");
        characterCombatLive.HealTeam(180f + characterCombatLive.Data.will * 1.5f);
        enemy.AddInfliction(characterCombatLive, Nature);
        return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 2.32f);
    }
    override protected float Long(int attackNum, CombatUnit enemy)
    {
        //Debug.LogWarning("nature long");
        characterCombatLive.HealTeam(270f + characterCombatLive.Data.will * 2.25f);
        StartCoroutine(AdjustSusceptibility(enemy));
        if (enemy.CombatUnitData.debuffLevels[Nature] > 0)
        {
            for (int i = 1; i < enemy.CombatUnitData.inflictions.Count; i++)
            {
                if (enemy.CombatUnitData.inflictions[i] > 0)
                {
                    enemy.AddInfliction(characterCombatLive, i);
                    break;
                }
            }
        }
        return enemy.CharacterCauseDamage(characterCombatLive, Nature, attackNum, 5f);
    }
    IEnumerator AdjustSusceptibility(CombatUnit enemy)
    {
        enemy.adjustSusceptibility(Arts, 1.22f);
        yield return new WaitForSeconds(30);
        enemy.adjustSusceptibility(Arts, 1f);
    }
}
