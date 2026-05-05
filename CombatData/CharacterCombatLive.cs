using System;
using UnityEngine;

public class CharacterCombatLive : CombatUnit
{
    [SerializeField] int characterId;
    bool alive = true;
    public bool Alive => alive;

    [SerializeField] public GameObject weapon1;
    [SerializeField] public GameObject weapon2;
    static float battleSkillAmount = 200;
    static float battleSkillAmountMax = 300;
    float comboSkillCooldown;
    [SerializeField] float comboSkillCooldownMax = 0;
    [SerializeField] float ultimateSkillAmount = 0;
    static float ultimateSkillAmountMax = 70;
    static public float UltimateSkillAmountMax => ultimateSkillAmountMax;

    Animator animator;
    bool isSkillable = true;
    public bool skillNow = false;
    NPCBehavior nPCBehavior;
    public bool basicAttack = false;
    int attackCount = 0;
    int attackCountMax = 3;

    [SerializeField] float idleWeaponTimer = 0;
    public event Action<float, float> OnComboMpChanged;
    public event Action<float, float> OnBattleMpChanged;
    public event Action<float, float> OnFinalMpChanged;

    [SerializeField] CharacterCombatSO characterCombatSO;
    [SerializeField] WeaponSO1 weaponSO;
    public struct CharacterCombatLiveStruct
    {
        public CharacterCombatData characterCombatData;
        public int level;
        public float strength, agilility, intellect, will;
        public float criticalRate, criticalDMG, artsIntensity;
        public float treatmentBonus, treatmentReeceivedBonus, comboSkillCooldownReduction, ultimateGainEfficiency;
        public float StaggerEfficiencyBonus, physicalDMGBonus, heatDMGBonus, electricDMGBonus, cryoDMGBonus, natureDMGBonus;
        public float StaggerDMGBonus, anySkillDMGBonus, basicAttackDMGBonus, battleSkillDMGBonus, comboSkillDMGBonus, ultimateSkillDMGBonus;
        public float finalDMGReduce, finalDMGBonus;
        Consumable consumable;
        public float consumableLastTime;
    }
    private CharacterCombatLiveStruct _data;
    public CharacterCombatLiveStruct Data => _data;
    private void Awake()
    {
        _data.characterCombatData = new CharacterCombatData(characterCombatSO, weaponSO);
        nPCBehavior = GetComponent<NPCBehavior>();
        animator = GetComponent<Animator>();
        selfCharacterCombatLive = this;
        selfNPCBehavior = nPCBehavior;

        comboSkillCooldown = comboSkillCooldownMax;
        OnComboMpChanged?.Invoke(comboSkillCooldown, comboSkillCooldownMax);
        CharacterCombatDataSetting();
    }
    protected override void Update()
    {
        base.Update();

        if(comboSkillCooldown < comboSkillCooldownMax)
        {
            comboSkillCooldown += Time.deltaTime;
            if (comboSkillCooldown > comboSkillCooldownMax)
            {
                comboSkillCooldown = comboSkillCooldownMax;
            }
            OnComboMpChanged?.Invoke(comboSkillCooldown, comboSkillCooldownMax);
        }
        if (!nPCBehavior.enabled)
        {
            if (PlaceManager.Instance.CombatNum == -1)
            {
                if (battleSkillAmount < battleSkillAmountMax - 100)
                {
                    battleSkillAmount += 75f * Time.deltaTime;
                    if (battleSkillAmount > battleSkillAmountMax - 100)
                        battleSkillAmount = battleSkillAmountMax - 100;
                    OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
                }
                else if(battleSkillAmount > battleSkillAmountMax - 100)
                {
                    battleSkillAmount -= 75f * Time.deltaTime;
                    if (battleSkillAmount < battleSkillAmountMax - 100)
                        battleSkillAmount = battleSkillAmountMax - 100;
                    OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
                }
            }
            else
            {
                if (battleSkillAmount < battleSkillAmountMax)
                {
                    battleSkillAmount += 8f * Time.deltaTime;
                    if (battleSkillAmount > battleSkillAmountMax)
                        battleSkillAmount = battleSkillAmountMax;
                    OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("IdleWeapon") && PlaceManager.Instance.CombatNum == -1)
        {
            idleWeaponTimer += Time.deltaTime;
            if (idleWeaponTimer > UnityEngine.Random.Range(4f, 6f))
            {
                idleWeaponTimer = 0;
                animator.SetTrigger("outCombat");
                animator.SetTrigger("Exit");
            }
        }
        else
        {
            idleWeaponTimer = 0;
        }
    }
    public void WeaponActive(bool weaponActive)
    {
        if (weapon1 != null)
            weapon1.SetActive(weaponActive);
        if (weapon2 != null)
            weapon2.SetActive(weaponActive);
    }
    public void EnterComBat()
    {
        //EnterComBatCharacterCombatDataSettingTest();
        CharacterCombatDataSetting();
        //EnterComBatWeaponSetting();
        //EnterComBatGearSetting();
        //EnterComBatGearSetTypeSetting();
        //EnterComBatTacticalSetting();
        //EnterComBatConsumableSetting();
    }
    void EnterComBatCharacterCombatDataSettingTest()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            animator.SetTrigger("Relive");
        }
        battleSkillAmount = 200;
        OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
        comboSkillCooldown = comboSkillCooldownMax;
        OnComboMpChanged?.Invoke(comboSkillCooldown, comboSkillCooldownMax);
        _data.level = 60;
        _data.strength = 200;
        _data.agilility = 200;
        _data.intellect = 200;
        _data.will = 200;

        _combatUnitData.hp = 10000;
        _combatUnitData.hpMax = 10000;
        _combatUnitData.baseAttack = 400;
        _combatUnitData.attack = 1000;
        _combatUnitData.defense = 140;

        _data.criticalRate = 0.05f;
        _data.criticalDMG = 0.5f;
        _data.artsIntensity = 50;

        _combatUnitData.resistences.Add(1f / (0.001f * _data.agilility + 1f));
        _combatUnitData.resistences.Add(1f / (0.001f * _data.intellect + 1f));
        _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);
        _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);
        _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);

        _data.treatmentBonus = 0;
        _data.treatmentReeceivedBonus = 0;
        _data.comboSkillCooldownReduction = 0;
        _data.ultimateGainEfficiency = 0;

        _data.StaggerEfficiencyBonus = 0;
        _data.physicalDMGBonus = 0;
        _data.heatDMGBonus = 0;
        _data.electricDMGBonus = 0;
        _data.cryoDMGBonus = 0;
        _data.natureDMGBonus = 0;

        _data.StaggerDMGBonus = 0;
        _data.anySkillDMGBonus = 0;
        _data.basicAttackDMGBonus = 0;
        _data.battleSkillDMGBonus = 0;
        _data.comboSkillDMGBonus = 0;
        _data.ultimateSkillDMGBonus = 0;

        _data.finalDMGReduce = 0;
        _data.finalDMGBonus = 0;

        NotifyHpChanged();
    }
    void CharacterCombatDataSetting()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            animator.SetTrigger("Relive");
        }
        battleSkillAmount = 200;
        OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
        comboSkillCooldown = comboSkillCooldownMax;
        OnComboMpChanged?.Invoke(comboSkillCooldown, comboSkillCooldownMax);

        _data.level = 60;
        _data.strength = Data.characterCombatData.Data.strength;
        _data.agilility = Data.characterCombatData.Data.agilility;
        _data.intellect = Data.characterCombatData.Data.intellect;
        _data.will = Data.characterCombatData.Data.will;

        _combatUnitData.hp = Data.characterCombatData.Data.hp;
        _combatUnitData.hpMax = Data.characterCombatData.Data.hp;
        _combatUnitData.baseAttack = Data.characterCombatData.Data.baseAttack;
        _combatUnitData.attack = Data.characterCombatData.Data.attack;
        _combatUnitData.defense = Data.characterCombatData.Data.defense;
        Debug.Log(_combatUnitData.hp + " " + _combatUnitData.baseAttack + " " + _combatUnitData.attack + " " + _combatUnitData.defense + " ");
        _data.criticalRate = Data.characterCombatData.Data.criticalRate;
        _data.criticalDMG = Data.characterCombatData.Data.criticalDMG;
        _data.artsIntensity = Data.characterCombatData.Data.artsIntensity;

        if (_combatUnitData.resistences.Count > 0)
        {
            _combatUnitData.resistences[0] = 1f / (0.001f * _data.agilility + 1f);
            _combatUnitData.resistences[1] = 1f / (0.001f * _data.intellect + 1f);
            _combatUnitData.resistences[2] = _combatUnitData.resistences[1];
            _combatUnitData.resistences[3] = _combatUnitData.resistences[1];
            _combatUnitData.resistences[4] = _combatUnitData.resistences[1];
        }
        else
        {
            _combatUnitData.resistences.Add(1f / (0.001f * _data.agilility + 1f));
            _combatUnitData.resistences.Add(1f / (0.001f * _data.intellect + 1f));
            _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);
            _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);
            _combatUnitData.resistences.Add(_combatUnitData.resistences[1]);
        }

        _data.treatmentBonus = Data.characterCombatData.Data.treatmentBonus;
        _data.treatmentReeceivedBonus = Data.characterCombatData.Data.treatmentReeceivedBonus;
        _data.comboSkillCooldownReduction = Data.characterCombatData.Data.comboSkillCooldownReduction;
        _data.ultimateGainEfficiency = Data.characterCombatData.Data.ultimateGainEfficiency;

        _data.StaggerEfficiencyBonus = Data.characterCombatData.Data.StaggerEfficiencyBonus;
        _data.physicalDMGBonus = Data.characterCombatData.Data.physicalDMGBonus;
        _data.heatDMGBonus = Data.characterCombatData.Data.heatDMGBonus;
        _data.electricDMGBonus = Data.characterCombatData.Data.electricDMGBonus;
        _data.cryoDMGBonus = Data.characterCombatData.Data.cryoDMGBonus;
        _data.natureDMGBonus = Data.characterCombatData.Data.natureDMGBonus;

        _data.StaggerDMGBonus = Data.characterCombatData.Data.StaggerDMGBonus;
        _data.anySkillDMGBonus = Data.characterCombatData.Data.anySkillDMGBonus;
        _data.basicAttackDMGBonus = Data.characterCombatData.Data.basicAttackDMGBonus;
        _data.battleSkillDMGBonus = Data.characterCombatData.Data.battleSkillDMGBonus;
        _data.comboSkillDMGBonus = Data.characterCombatData.Data.comboSkillDMGBonus;
        _data.ultimateSkillDMGBonus = Data.characterCombatData.Data.ultimateSkillDMGBonus;

        _data.finalDMGReduce = Data.characterCombatData.Data.finalDMGReduce;
        _data.finalDMGBonus = Data.characterCombatData.Data.finalDMGBonus;
        NotifyHpChanged();
    }
    public void WeaponSetting(WeaponSO1 weaponSO)
    {
        Data.characterCombatData.WeaponSetting(weaponSO);
        CharacterCombatDataSetting();
    }
    void EnterComBatGearSetting()
    {

    }
    void EnterComBatGearSetTypeSetting()
    {

    }
    void EnterComBatTacticalSetting()
    {

    }
    void EnterComBatConsumableSetting()
    {

    }
    public void LeaveComBat()
    {
        CharacterCombatDataSetting();
    }
    public void PressMouseLeft()
    {
        basicAttack = true;
        animator.SetBool("basicAttack", true);
    }
    public void PressCancelMouseLeft()
    {
        basicAttack = false;
        animator.SetBool("basicAttack", false);
    }
    public bool PressCombo()
    {
        if (isSkillable)
        {
            if (nPCBehavior.enabled)
            {
                if (!nPCBehavior.PressSkill())
                    return false;
            }
            else if (PlaceManager.Instance.CombatNum != -1 && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentTarget == null
                && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentSkillTarget == null)
            {
                return false;
            }
            comboSkillCooldown = 0;
            OnComboMpChanged?.Invoke(comboSkillCooldown, comboSkillCooldownMax);
            if (PlaceManager.Instance.CombatNum != -1)
                UltimateSkillAmountAdd(10f);
            animator.SetTrigger("Combo");
            animator.SetTrigger("Exit");
            return true;
        }
        return false;
    }
    public void PressShortNumSkill()
    {
        animator.SetTrigger("Short");
        animator.SetTrigger("Exit");
        if (isSkillable && battleSkillAmount >= 100)
        {
            //Debug.Log("tap" + (characterId).ToString());
            if (nPCBehavior.enabled)
            {
                if (!nPCBehavior.PressSkill())
                    return;
            }
            else if (PlaceManager.Instance.CombatNum != -1 && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentTarget == null
                && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentSkillTarget == null)
            {
                return;
            }
            battleSkillAmount -= 100;
            OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
            if (PlaceManager.Instance.CombatNum != -1)
            {
                for (int i = 0; i < InputManager.Instance.Characters.Count; i++)
                {
                    InputManager.Instance.Characters[i].GetComponent<CharacterCombatLive>().UltimateSkillAmountAdd(6.5f);
                }
            }
            animator.SetTrigger("Short");
            animator.SetTrigger("Exit");
        }
    }
    public void PressLongNumSkill()
    {
        animator.SetTrigger("Long");
        animator.SetTrigger("Exit");
        if (isSkillable && ultimateSkillAmount >= ultimateSkillAmountMax - 0.01f)
        {
            //Debug.Log("hold" + (characterId).ToString());
            if (nPCBehavior.enabled)
            {
                if (!nPCBehavior.PressSkill())
                    return;
            }
            else if (PlaceManager.Instance.CombatNum != -1 && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentTarget == null
                && PlaceManager.Instance.MainControlCharacter.GetComponent<LocomotionBlend>().currentSkillTarget == null)
            {
                return;
            }
            if (PlaceManager.Instance.CombatNum != -1)
            {
                ultimateSkillAmount = 0;
                OnFinalMpChanged?.Invoke(ultimateSkillAmount, ultimateSkillAmountMax);
            }
            animator.SetTrigger("Long");
            animator.SetTrigger("Exit");
        }
    }
    public void IsSkillable(bool isSkillable)
    {
        this.isSkillable = isSkillable;
    }
    public void AttackCountAdd()
    {
        attackCount++;
        attackCount %= attackCountMax;
        //if (nPCBehavior.enabled)
        //{
        //    attackCount %= (attackCountMax - 1);
        //}
        //else
        //{
        //    attackCount %= attackCountMax;
        //}
        animator.SetInteger("attackCount", attackCount);
    }
    public void AttackCountZero()
    {
        attackCount = 0;
        animator.SetInteger("attackCount", attackCount);
    }
    public void Hit(Vector3 EnemyPos, int type, float damage)
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo next = animator.GetNextAnimatorStateInfo(0);
        if (next.IsTag("Evade") || current.IsTag("Evade"))
        {
            BattleSkillAmountAdd(7.5f);
        }
        else if (current.IsTag("Hit") || next.IsTag("Hit") || current.IsTag("ShortNum") || next.IsTag("ShortNum") || current.IsTag("LongNum") || next.IsTag("LongNum"))
        {
            //Debug.LogWarning("In Hit from " + enemyCombatData.gameObject.name);
            MonsterCauseDamage(type, damage);
        }
        else
        {
            // 1. 取得攻擊方向向量 (從自己指向敵人)
            // 只考慮 XZ 平面，排除高度干擾
            Vector3 attackerDir = (EnemyPos - transform.position);
            attackerDir.y = 0;
            attackerDir.Normalize();
            // 2. 將「世界方向」轉換為「角色局部方向」
            // 這會根據角色的 transform.forward 算出相對座標
            Vector3 localDir = transform.InverseTransformDirection(attackerDir);
            animator.SetFloat("HitLR", localDir.x);
            animator.SetFloat("HitFB", localDir.z);
            animator.SetTrigger("HitLarge");
            animator.SetTrigger("Exit");
            MonsterCauseDamage(type, damage);
        }
    }
    public void Hit(Vector3 EnemyPos)
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo next = animator.GetNextAnimatorStateInfo(0);
        if (next.IsTag("Evade") || current.IsTag("Evade"))
        {
        }
        else if (current.IsTag("Hit") || next.IsTag("Hit") || current.IsTag("ShortNum") || next.IsTag("ShortNum") || current.IsTag("LongNum") || next.IsTag("LongNum"))
        {
        }
        else
        {
            // 1. 取得攻擊方向向量 (從自己指向敵人)
            // 只考慮 XZ 平面，排除高度干擾
            Vector3 attackerDir = (EnemyPos - transform.position);
            attackerDir.y = 0;
            attackerDir.Normalize();
            // 2. 將「世界方向」轉換為「角色局部方向」
            // 這會根據角色的 transform.forward 算出相對座標
            Vector3 localDir = transform.InverseTransformDirection(attackerDir);
            animator.SetFloat("HitLR", localDir.x);
            animator.SetFloat("HitFB", localDir.z);
            animator.SetTrigger("HitLarge");
            animator.SetTrigger("Exit");
        }
    }
    public void KnockDown(int type, float damage)
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo next = animator.GetNextAnimatorStateInfo(0);
        if (next.IsTag("Evade") || current.IsTag("Evade"))
        {
            BattleSkillAmountAdd(7.5f);
        }
        else if (current.IsTag("Hit") || next.IsTag("Hit") || current.IsTag("LongNum") || next.IsTag("LongNum"))
        {
            MonsterCauseDamage(type, damage);
        }
        else
        {
            MonsterCauseDamage(type, damage);
            animator.SetTrigger("Knock");
            animator.SetTrigger("Exit");
        }
    }
    public void BattleSkillAmountAdd(float amountAdd)
    {
        battleSkillAmount += amountAdd;
        if (battleSkillAmount > battleSkillAmountMax)
        {
             battleSkillAmount = battleSkillAmountMax;
        }
        OnBattleMpChanged?.Invoke(battleSkillAmount, battleSkillAmountMax);
    }
    public void UltimateSkillAmountAdd(float amountAdd)
    {
        ultimateSkillAmount += amountAdd * (1 + Data.ultimateGainEfficiency);
        if (ultimateSkillAmount > ultimateSkillAmountMax)
        {
             ultimateSkillAmount = ultimateSkillAmountMax;
        }
        OnFinalMpChanged?.Invoke(ultimateSkillAmount, ultimateSkillAmountMax);
    }
    public void Death()
    {
        animator.SetTrigger("Death");
    }
    public void SetCriticalRate(float value)
    {
        _data.criticalRate = value;
    }
    public void AbleCombo()
    {
        if (comboSkillCooldown >= comboSkillCooldownMax)
        {
            InputManager.Instance.ComboAdd(characterId);
        }
    }
}
