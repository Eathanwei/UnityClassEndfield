public class CharacterCombatData
{
    public struct CharacterCombatDataStruct
    {
        public int characterId;
        public int level;
        public float strength, agilility, intellect, will;
        public float hp, hpRatio, baseAttack, baseAttackAdd, attack, defense;
        public float criticalRate, criticalDMG, artsIntensity;
        public float physicalResistence, artsResistence, heatResistance, electricResistance, cryoResistance, natureResistance;
        public float treatmentBonus, treatmentReeceivedBonus, comboSkillCooldownReduction, ultimateGainEfficiency;
        public float StaggerEfficiencyBonus, physicalDMGBonus, heatDMGBonus, electricDMGBonus, cryoDMGBonus, natureDMGBonus;
        public float StaggerDMGBonus, anySkillDMGBonus, basicAttackDMGBonus, battleSkillDMGBonus, comboSkillDMGBonus, ultimateSkillDMGBonus;
        public float finalDMGReduce, finalDMGBonus;
        public CharacterBasicData characterBasicData;
        public Weapon weapon;
        public Gear armor, gloves, kit1, kit2;
        public GearSetTypeEnum gearSetType;
        public Tactical tactical;
    }
    private CharacterCombatDataStruct _data;
    public CharacterCombatDataStruct Data => _data;
    public CharacterCombatData(CharacterCombatSO characterCombatSO, WeaponSO1 weaponSO)
    {
        _data.characterBasicData = new CharacterBasicData(characterCombatSO);
        _data.weapon = new Weapon(weaponSO);
        AllDataSetting();
    }
    void AllDataSetting()
    {
        CharacterBasicDataSetting();
        WeaponDataSetting();
        FinalDataSetting();
    }
    void CharacterBasicDataSetting()
    {
        _data.strength = Data.characterBasicData.Data.strength;
        _data.agilility = Data.characterBasicData.Data.agilility;
        _data.intellect = Data.characterBasicData.Data.intellect;
        _data.will = Data.characterBasicData.Data.will;

        _data.hp = Data.characterBasicData.Data.hp;
        _data.hpRatio = 1;
        _data.baseAttack = Data.characterBasicData.Data.attack;
        _data.baseAttackAdd = 0;
        _data.attack = Data.characterBasicData.Data.attack;
        _data.defense = 140;

        _data.criticalRate = 0.05f;
        _data.criticalDMG = 0.5f;
        _data.artsIntensity = 0;

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
    }
    void WeaponDataSetting()
    {
        _data.baseAttack += _data.weapon.Data.atk;
        switch (_data.weapon.Data.weaponAttribute)
        {
            case WeaponAttributeEnum.Strength:
                _data.strength += WeaponUnit.weaponAttributeValue;
                break;
            case WeaponAttributeEnum.Agilility:
                _data.agilility += WeaponUnit.weaponAttributeValue;
                break;
            case WeaponAttributeEnum.Intellect:
                _data.intellect += WeaponUnit.weaponAttributeValue;
                break;
            case WeaponAttributeEnum.Will:
                _data.will += WeaponUnit.weaponAttributeValue;
                break;
            case WeaponAttributeEnum.Main:
                switch (Data.characterBasicData.Data.mainType)
                {
                    case 0:
                        _data.strength += WeaponUnit.weaponAttributeValue;
                        break;
                    case 1:
                        _data.agilility += WeaponUnit.weaponAttributeValue;
                        break;
                    case 2:
                        _data.intellect += WeaponUnit.weaponAttributeValue;
                        break;
                    case 3:
                        _data.will += WeaponUnit.weaponAttributeValue;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        switch (_data.weapon.Data.weaponSecondary)
        {
            case WeaponSecondaryEnum.Attack:
                _data.baseAttackAdd += _data.baseAttack * WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.Attack] * 0.01f;
                break;
            case WeaponSecondaryEnum.Hp:
                _data.hpRatio += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.Hp] * 0.01f;
                break;
            case WeaponSecondaryEnum.PhysicalDMG:
                _data.physicalDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.PhysicalDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.ArtsDMG:
                _data.cryoDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ArtsDMG] * 0.01f;
                _data.electricDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ArtsDMG] * 0.01f;
                _data.natureDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ArtsDMG] * 0.01f;
                _data.heatDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ArtsDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.HeatDMG:
                _data.heatDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.HeatDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.ElectricDMG:
                _data.electricDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ElectricDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.CryoDMG:
                _data.cryoDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.CryoDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.NatureDMG:
                _data.natureDMGBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.NatureDMG] * 0.01f;
                break;
            case WeaponSecondaryEnum.CriticalRate:
                _data.criticalRate += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.CriticalRate] * 0.01f;
                break;
            case WeaponSecondaryEnum.ArtsIntensity:
                _data.artsIntensity += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.ArtsIntensity];
                break;
            case WeaponSecondaryEnum.TreatmentBonus:
                _data.treatmentBonus += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.TreatmentBonus] * 0.01f;
                break;
            case WeaponSecondaryEnum.UltimateGain:
                _data.ultimateGainEfficiency += WeaponUnit.weaponSecondaryValue[(int)WeaponSecondaryEnum.UltimateGain] * 0.01f;
                break;
            default:
                break;
        }
    }
    void FinalDataSetting()
    {
        float attackAddRatio = 1;
        switch (Data.characterBasicData.Data.mainType)
        {
            case 0:
                attackAddRatio += _data.strength * 0.005f;
                break;
            case 1:
                attackAddRatio += _data.agilility * 0.005f;
                break;
            case 2:
                attackAddRatio += _data.intellect * 0.005f;
                break;
            case 3:
                attackAddRatio += _data.will * 0.005f;
                break;
            default:
                break;
        }
        switch (Data.characterBasicData.Data.viceType)
        {
            case 0:
                attackAddRatio += _data.strength * 0.002f;
                break;
            case 1:
                attackAddRatio += _data.agilility * 0.002f;
                break;
            case 2:
                attackAddRatio += _data.intellect * 0.002f;
                break;
            case 3:
                attackAddRatio += _data.will * 0.002f;
                break;
            default:
                break;
        }
        _data.attack = (_data.baseAttack + _data.baseAttackAdd) * attackAddRatio;
        _data.hp += _data.strength * 5;
        _data.hp *= _data.hpRatio;
        _data.physicalResistence = 1f / (0.001f * _data.agilility + 1f);
        _data.artsResistence = 1f / (0.001f * _data.intellect + 1f);
        _data.cryoResistance = _data.artsResistence;
        _data.electricResistance = _data.artsResistence;
        _data.natureResistance = _data.artsResistence;
        _data.heatResistance = _data.artsResistence;
        _data.treatmentReeceivedBonus = 0.001f * _data.intellect;
    }
    public void WeaponSetting(WeaponSO1 weaponSO)
    {
        _data.weapon = new Weapon(weaponSO);
        AllDataSetting();
    }
}
