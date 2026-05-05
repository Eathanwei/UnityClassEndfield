using UnityEngine;
using System.Collections.Generic;

public class CharacterSkill : MonoBehaviour
{
    protected CharacterCombatLive characterCombatLive;
    protected NPCBehavior npcBehavior;
    virtual protected float BasicAttack(int attackNum, CombatUnit enemy)
    {
        return 1;
    }
    virtual protected float HeavyAttack(int attackNum, CombatUnit enemy)
    {
        return 1;
    }
    virtual protected float Combo(int attackNum, CombatUnit enemy)
    {
        return 1;
    }
    virtual protected float Short(int attackNum, CombatUnit enemy)
    {
        return 1;
    }
    virtual protected float Long(int attackNum, CombatUnit enemy)
    {
        return 1;
    }
    public float AtackNum(int attackNum, CombatUnit enemy)
    {
        switch(attackNum)
        {
            case 1:
                return BasicAttack(attackNum, enemy);
            case 2:
                return BasicAttack(attackNum, enemy);
            case 3:
                return HeavyAttack(attackNum, enemy);
            case 4:
                return Combo(attackNum, enemy);
            case 5:
                return Short(attackNum, enemy);
            case 6:
                return Long(attackNum, enemy);
            default:
                return 0;
        }
    }
    protected void AbleCombo()
    {
        characterCombatLive.AbleCombo();
    }
}
