using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatUnit : MonoBehaviour
{
    public const int Heal = -2;
    public const int Arts = -1;
    public const int Physical = 0;
    public const int Cryo = 1;
    public const int Electric = 2;
    public const int Nature = 3;
    public const int Heat = 4;
    public struct CombatUnitStruct
    {
        public bool isDead;
        public float hp, hpMax, baseAttack, attack, defense;
        public List<float> resistences;
        public List<int> inflictions;
        public List<int> debuffLevels;
        public float physicalDebuffRatio, electricDebuffRatio, natureDebuffRatio;
        public List<float> debuffTimes;
        public float physicalSusceptibility, artsSusceptibility;
        public float natureDebuffResistenceDown;
    }
    protected CombatUnitStruct _combatUnitData;
    public CombatUnitStruct CombatUnitData => _combatUnitData;
    static List<List<float>> debuffTimeMaxes = new()
    {
        new List<float> { 0f, 12f, 18f, 24f, 30f},
        new List<float> { 0f, 6f, 7f, 8f, 9f},
        new List<float> { 0f, 12f, 18f, 24f, 30f},
        new List<float> { 0f, 15f, 15f, 15f, 15f},
        new List<float> { 0f, 10f, 10f, 10f, 10f}
    };
    static public List<float> debuffTimeTypeMaxes = new()
    {
        30f, 9f, 30f, 15f, 10f
    };
    static List<List<float>> debuffRatios = new()
    {
        new List<float> { 1f, 1.12f, 1.16f, 1.20f, 1.24f},
        new List<float> { 0f, 0.9f, 0.8f, 0.7f, 0.6f},
        new List<float> { 1f, 1.12f, 1.16f, 1.20f, 1.24f},
        new List<float> { 0f, 12f, 16f, 20f, 24f},
        new List<float> { 0f, 0.24f, 0.36f, 0.48f, 0.6f}
    };
    static List<float> natureDebuffStart = new()
    {
        0f, 3.6f, 4.8f, 6f, 7.2f
    };
    static List<float> natureDebuffPerSecond = new()
    {
        0f, 0.84f, 1.12f, 1.4f, 1.68f
    };
    Coroutine heatDebuffCoroutine;
    protected CharacterCombatLive selfCharacterCombatLive;
    protected EnemyCombatData selfEnemyCombatData;
    protected NPCBehavior selfNPCBehavior;

    public event Action OnHpChanged;
    public event Action<int, float> OnDamageHpChanged;
    public event Action OnDebuffChanged;
    public event Action OnInflictionChanged;
    protected CombatUnit()
    {
        _combatUnitData.isDead = false;
        _combatUnitData.resistences = new();
        _combatUnitData.inflictions = new();
        _combatUnitData.debuffLevels = new();
        _combatUnitData.debuffTimes = new();
        for (int i = 0; i < 5; i++)
        {
            _combatUnitData.inflictions.Add(0);
            _combatUnitData.debuffLevels.Add(0);
            _combatUnitData.debuffTimes.Add(0);
        }
        OnInflictionChanged?.Invoke();
        OnDebuffChanged?.Invoke();
        _combatUnitData.physicalDebuffRatio = 1;
        _combatUnitData.electricDebuffRatio = 1;
        _combatUnitData.natureDebuffRatio = 1;
        _combatUnitData.physicalSusceptibility = 1;
        _combatUnitData.artsSusceptibility = 1;
        _combatUnitData.natureDebuffResistenceDown = 0;
    }
    protected virtual void Update()
    {
        for (int i = 0; i < _combatUnitData.debuffLevels.Count; i++)
        {
            if (i == Nature)
            {
                continue;
            }
            if (_combatUnitData.debuffLevels[i] > 0)
            {
                _combatUnitData.debuffTimes[i] -= Time.deltaTime;
                if (_combatUnitData.debuffTimes[i] < 0)
                {
                    _combatUnitData.debuffLevels[i] = 0;
                    _combatUnitData.debuffTimes[i] = 0;
                }
                OnDebuffChanged?.Invoke();
            }
        }
        if (_combatUnitData.debuffLevels[Nature] > 0)
        {
            _combatUnitData.debuffTimes[Nature] -= Time.deltaTime;
            if(_combatUnitData.natureDebuffResistenceDown < debuffRatios[Nature][_combatUnitData.debuffLevels[Nature]] * _combatUnitData.natureDebuffRatio)
            {
                _combatUnitData.natureDebuffResistenceDown += natureDebuffPerSecond[_combatUnitData.debuffLevels[Nature]] * _combatUnitData.natureDebuffRatio;
                if (_combatUnitData.natureDebuffResistenceDown > debuffRatios[Nature][_combatUnitData.debuffLevels[Nature]] * _combatUnitData.natureDebuffRatio)
                {
                    _combatUnitData.natureDebuffResistenceDown = debuffRatios[Nature][_combatUnitData.debuffLevels[Nature]] * _combatUnitData.natureDebuffRatio;
                }
            }
            if (_combatUnitData.debuffTimes[Nature] < 0)
            {
                _combatUnitData.natureDebuffResistenceDown = 0;
                _combatUnitData.debuffLevels[Nature] = 0;
                _combatUnitData.natureDebuffRatio = 1;
            }
            OnDebuffChanged?.Invoke();
        }
    }
    public void Crush(CombatUnit character)
    {
        if (_combatUnitData.debuffLevels[Cryo] > 0)
        {
            if (character is CharacterCombatLive characterCombatLive)
            {
                CharacterCauseDebuffDamage(characterCombatLive, Physical, Cryo, 1.2f * (_combatUnitData.debuffLevels[Cryo] + 1));
            }
            else if (character is EnemyCombatData enemyCombatData)
            {
                MonsterCauseDamage(Physical, 1.2f * (_combatUnitData.debuffLevels[Cryo] + 1));
            }
            ConsumeDebuff(character, Cryo);
        }
        if(_combatUnitData.inflictions[Physical] == 0)
        {
            _combatUnitData.inflictions[Physical] = 1;
        }
        else
        {
            if (character is CharacterCombatLive characterCombatLive)
            {
                CharacterCauseDebuffDamage(characterCombatLive, Physical, Physical, 1.5f * (_combatUnitData.inflictions[Physical] + 1));
            }
            else if (character is EnemyCombatData enemyCombatData)
            {
                MonsterCauseDamage(Physical, 1.5f * (_combatUnitData.inflictions[Physical] + 1));
            }
            _combatUnitData.inflictions[Physical] = 0;
        }
        OnInflictionChanged?.Invoke();
    }
    public void AddInfliction(CombatUnit character, int type)
    {
        Debug.Log("add infliction: " + type);
        CombatEventManager.BroadcastInfliction();
        if (_combatUnitData.inflictions[type] == 0)
        {
            for (int i = 1; i < _combatUnitData.inflictions.Count; i++) 
            {
                if (_combatUnitData.inflictions[i] > 0)
                {
                    if (character is CharacterCombatLive characterCombatLive)
                    {
                        CharacterCauseDebuffDamage(characterCombatLive, i, i, 0.8f * (_combatUnitData.inflictions[i] + 1));
                    }
                    else if (character is EnemyCombatData enemyCombatData)
                    {
                        MonsterCauseDamage(i, 0.8f * (_combatUnitData.inflictions[i] + 1));
                    }
                    AddDebuff(character, type, _combatUnitData.inflictions[i]);
                    _combatUnitData.inflictions[i] = 0;
                    OnInflictionChanged?.Invoke();
                    return;
                }
            } 
        }
        if (character is CharacterCombatLive characterCombatLive1 && _combatUnitData.inflictions[type] > 0)
        {
            CharacterCauseDebuffDamage(characterCombatLive1, type, type, 1.6f);
        }
        else if (character is EnemyCombatData enemyCombatData)
        {
            MonsterCauseDamage(type, 1.6f);
        }
        if (_combatUnitData.inflictions[type] < 4)
        {
            _combatUnitData.inflictions[type]++;
            OnInflictionChanged?.Invoke();
        }
    }
    public void ConsumeDebuff(CombatUnit character, int type)
    {
        _combatUnitData.debuffLevels[type] = 0;
        _combatUnitData.debuffTimes[type] = 0;
        CombatEventManager.BroadcastConsumeDebuff();
        OnDebuffChanged?.Invoke();
    }
    public void AddDebuff(CombatUnit character, int type, int level)
    {
        if (debuffTimeMaxes[type][level] > _combatUnitData.debuffTimes[type])
        {
            _combatUnitData.debuffLevels[type] = level;
            _combatUnitData.debuffTimes[type] = debuffTimeMaxes[type][level];
            if(type == Physical)
            {
                if (character is CharacterCombatLive characterCombatLive)
                {
                    _combatUnitData.physicalDebuffRatio = CharacterArtsIntensityEnhanceRatio(characterCombatLive);
                }
            }
            else if (type == Cryo)
            {
                CombatEventManager.BroadcastIceDebuff();
            }
            else if (type == Electric)
            {
                if (character is CharacterCombatLive characterCombatLive)
                {
                    _combatUnitData.electricDebuffRatio = CharacterArtsIntensityEnhanceRatio(characterCombatLive);
                }
            }
            else if (type == Nature)
            {
                if (character is CharacterCombatLive characterCombatLive)
                {
                    _combatUnitData.natureDebuffRatio = CharacterArtsIntensityEnhanceRatio(characterCombatLive);
                }
                if (_combatUnitData.natureDebuffResistenceDown < natureDebuffStart[level])
                    _combatUnitData.natureDebuffResistenceDown = natureDebuffStart[level];
            }
            else if (type == Heat)
            {
                if (heatDebuffCoroutine != null)
                    StopCoroutine(heatDebuffCoroutine);
                float damage = 0;
                if (character is CharacterCombatLive characterCombatLive)
                {
                    damage = CharacterCauseDebuffDamage(characterCombatLive, Heat, Heat, debuffRatios[Heat][_combatUnitData.debuffLevels[Heat]]);
                }
                else if (character is EnemyCombatData enemyCombatData)
                {
                    damage = MonsterCauseDamage(Heat, debuffRatios[Heat][_combatUnitData.debuffLevels[Heat]]);
                }
                heatDebuffCoroutine = StartCoroutine(HeatDebuff(damage));
            }
        }
        OnDebuffChanged?.Invoke();
    }
    public void adjustSusceptibility(int type, float susceptibility)
    {
        if (type == Physical)
        {
            _combatUnitData.physicalSusceptibility = susceptibility;
        }
        else
        {
            _combatUnitData.artsSusceptibility = susceptibility;
        }
    }
    public float CharacterCauseDamage(CharacterCombatLive characterCombatLive, int type, int attackNum, float damage)
    {
        damage *= 1 - selfEnemyCombatData.finalDMGReduce;
        damage *= characterCombatLive._combatUnitData.attack;
        damage *= CharacterCriticalRatio(characterCombatLive);
        damage *= CharacterTypeRatio(characterCombatLive, type, attackNum);
        damage *= 1 + characterCombatLive.Data.finalDMGBonus;
        return GetDamage(type, damage);
    }
    public float CharacterCauseDebuffDamage(CharacterCombatLive characterCombatLive, int type, int physicalArtsType, float damage)
    {
        damage *= 1 - selfEnemyCombatData.finalDMGReduce;
        damage *= characterCombatLive._combatUnitData.attack;
        damage *= CharacterCriticalRatio(characterCombatLive);
        damage *= CharacterTypeRatio(characterCombatLive, type, 0);
        damage *= CharacterLevelRatio(characterCombatLive, physicalArtsType);
        damage *= CharacterArtsIntensityRatio(characterCombatLive);
        damage *= 1 + characterCombatLive.Data.finalDMGBonus;
        return GetDamage(type, damage);
    }
    public float MonsterCauseDamage(int type, float damage)
    {
        damage *= 1 - selfCharacterCombatLive.Data.finalDMGReduce;
        if (selfNPCBehavior.enabled)
        {
            damage *= 0.2f;
        }
        return GetDamage(type, damage);
    }
    public float GetDamage(int type, float damage)
    {
        damage *= 1 - Mathf.Max(0, _combatUnitData.resistences[type] - _combatUnitData.natureDebuffResistenceDown)  / 100f;
        damage *= (type == Physical) ? debuffRatios[Physical][_combatUnitData.debuffLevels[Physical]] * _combatUnitData.physicalDebuffRatio
            : debuffRatios[Electric][_combatUnitData.debuffLevels[Electric]] * _combatUnitData.electricDebuffRatio;
        damage *= (type == Physical) ? _combatUnitData.physicalSusceptibility : _combatUnitData.artsSusceptibility;
        damage *= 100 / (Mathf.Max(0, _combatUnitData.defense) + 100);
        _combatUnitData.hp -= damage;
        if (damage > 1500) 
            Debug.LogWarning(gameObject.name + " hp: " + _combatUnitData.hp + ", " + type + " damage: " + damage);
        if (_combatUnitData.hp <= 0 && !_combatUnitData.isDead)
        {
            _combatUnitData.isDead = true;
            _combatUnitData.hp = 0;
            if(selfCharacterCombatLive == null)
            {
                RockmanStateMachine  rockmanStateMachine = GetComponent<RockmanStateMachine>();
                DragonState dragonState = GetComponent<DragonState>();
                BossState bossState = GetComponent<BossState>();
                if (rockmanStateMachine != null)
                {
                    rockmanStateMachine.Death();
                }
                else if (dragonState != null)
                {
                    dragonState.Death();
                }
                else if (bossState != null)
                {
                    bossState.Death();
                }
            }
            else
            {
                selfCharacterCombatLive.Death();
            }
        }
        OnHpChanged?.Invoke();
        OnDamageHpChanged?.Invoke(type, damage);
        return damage;
    }
    IEnumerator HeatDebuff(float damage)
    {
        yield return new WaitForSeconds(1);
        while (_combatUnitData.debuffTimes[Heat] > 0)
        {
            GetDamage(Heat, damage);
            yield return new WaitForSeconds(1);
        }
        heatDebuffCoroutine = null;
    }
    float CharacterLevelRatio(CharacterCombatLive characterCombatLive, int type)
    {
        if (type == Physical)
        {
            return 1 + (characterCombatLive.Data.level - 1) / 392;
        }
        else
        {
            return 1 + (characterCombatLive.Data.level - 1) / 196;
        }
    }
    float CharacterArtsIntensityRatio(CharacterCombatLive characterCombatLive)
    {
        return 1 + characterCombatLive.Data.artsIntensity / 100;
    }
    float CharacterArtsIntensityEnhanceRatio(CharacterCombatLive characterCombatLive)
    {
        //Debug.LogWarning(characterCombatLive);
        return 1 + 2 * characterCombatLive.Data.artsIntensity / (characterCombatLive.Data.artsIntensity + 300);
    }
    float CharacterCriticalRatio(CharacterCombatLive characterCombatLive)
    {
        if (UnityEngine.Random.value < characterCombatLive.Data.criticalRate)
        {
            return 1 + characterCombatLive.Data.criticalDMG;
        }
        return 1;
    }
    float CharacterTypeRatio(CharacterCombatLive characterCombatLive, int type, int attackNum)
    {
        float bonus = 0;
        switch (type)
        {
            case Physical:
                bonus += characterCombatLive.Data.physicalDMGBonus;
                break;
            case Cryo:
                bonus += characterCombatLive.Data.cryoDMGBonus;
                break;
            case Electric:
                bonus += characterCombatLive.Data.natureDMGBonus;
                break;
            case Nature:
                bonus += characterCombatLive.Data.heatDMGBonus;
                break;
            case Heat:
                bonus += characterCombatLive.Data.heatDMGBonus;
                break;
            default:
                bonus += 0;
                break;
        }
        switch (attackNum)
        {
            case 1:
            case 2:
            case 3:
                bonus += characterCombatLive.Data.basicAttackDMGBonus;
                break;
            case 4:
                bonus += characterCombatLive.Data.comboSkillDMGBonus + characterCombatLive.Data.anySkillDMGBonus;
                break;
            case 5:
                bonus += characterCombatLive.Data.battleSkillDMGBonus + characterCombatLive.Data.anySkillDMGBonus;
                break;
            case 6:
                bonus += characterCombatLive.Data.ultimateSkillDMGBonus + characterCombatLive.Data.anySkillDMGBonus;
                break;
            default:
                bonus += 0;
                break;
        }
        return 1 + bonus;
    }
    public void HealTeam(float value)
    {
        //Debug.LogWarning("heal team: " + value);
        value *= 1 + selfCharacterCombatLive.Data.treatmentBonus;
        List<CharacterCombatLive> characterCombatLives = new(); 
        foreach (var character in InputManager.Instance.Characters)
        {
            characterCombatLives.Add(character.GetComponent<CharacterCombatLive>());
        }
        if (characterCombatLives[InputManager.MainControl]._combatUnitData.hp < characterCombatLives[InputManager.MainControl].CombatUnitData.hpMax - 1)
        {
            characterCombatLives[InputManager.MainControl].Healed(value * (1 + characterCombatLives[InputManager.MainControl].Data.treatmentReeceivedBonus));
        }
        else
        {
            int minHPIndex = 0;
            for (int i = 1; i < characterCombatLives.Count; i++)
            {
                if(characterCombatLives[minHPIndex]._combatUnitData.hp / characterCombatLives[minHPIndex]._combatUnitData.hpMax
                    > characterCombatLives[i]._combatUnitData.hp / characterCombatLives[i]._combatUnitData.hpMax)
                {
                    minHPIndex = i;
                }
            }
            characterCombatLives[minHPIndex].Healed(value * (1 + characterCombatLives[InputManager.MainControl].Data.treatmentReeceivedBonus));
        }
    }
    public void Healed(float value)
    {
        //Debug.LogWarning(gameObject.name + " healed: " + value);
        if (_combatUnitData.hp + value > _combatUnitData.hpMax)
        {
            _combatUnitData.hp = _combatUnitData.hpMax;
        }
        else
        {
            _combatUnitData.hp = _combatUnitData.hp + value;
        }
        OnHpChanged?.Invoke();
        OnDamageHpChanged?.Invoke(Heal, value);
    }
    protected void NotifyHpChanged()
    {
        OnHpChanged?.Invoke();
    }
}
