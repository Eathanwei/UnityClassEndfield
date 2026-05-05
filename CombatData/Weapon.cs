public enum WeaponTypeEnum
{
    Sword, Gun, ArtsUnit, Greatsword 
}
public enum WeaponAttributeEnum
{
    Strength, Agilility, Intellect, Will, Main
}
public enum WeaponSecondaryEnum
{
    Attack, Hp,
    PhysicalDMG, ArtsDMG, HeatDMG, ElectricDMG, CryoDMG, NatureDMG,
    CriticalRate, ArtsIntensity,
    TreatmentBonus, UltimateGain
}
public enum WeaponSkillEnum
{
    Assault, Suppression, Pursuit,
    Crusher, Inspiring, Combative,
    Brutality, Infliction, Medicant,
    Fracture, Detonate, Twilight,
    Flow, Efficacy
}

public class Weapon
{
    public struct WeaponStruct
    {
        public WeaponSO1 weaponSO;
        public int typeId, allId, level, exp;
        public float atk;
        public WeaponTypeEnum weaponType;
        public WeaponAttributeEnum weaponAttribute;
        public WeaponSecondaryEnum weaponSecondary;
        public WeaponSkillEnum weaponSkill;
        public int weaponAttributeLevel, weaponSecondaryLevel, weaponSkillLevel;
        public int weaponAttributeMaxLevel, weaponSecondaryMaxLevel, weaponSkillMaxLevel;
        public Essence essence;
    }
    private WeaponStruct _data;
    public WeaponStruct Data => _data;
    public Weapon(WeaponSO1 weaponSO)
    {
        SetWeapon(weaponSO);
    }
    public void SetWeapon(WeaponSO1 weaponSO)
    {
        _data.weaponSO = weaponSO;
        _data.atk = weaponSO.atk;
        _data.typeId = weaponSO.TypeId;
        _data.weaponType = weaponSO.WeaponType;
        _data.weaponAttribute = weaponSO.WeaponAttribute;
        _data.weaponSecondary = weaponSO.WeaponSecondary;
    }
}