public enum GearTypeEnum
{
    Armor, Gloves, Kit
}
public enum GearAttributeEnum
{
    Strength, Agilility, Intellect, Will
}
public enum GearStatEnum
{
    Attack, Hp, CriticalRate,
    ArtsIntensity, TreatmentBonus, PhysicalDMG,
    UltimateGain, BasicAttackDMG, BattleSkillDMG,
    ComboSkillDMG, UltimateSkillDMG, ToStaggeredDMG,
    MainAttribute, ArtsDMG, CryoElectricDMG,
    AllSkillDMG, SecondaryAttribute, FinalDMGReduction,
    HeatNatureDMG
}
public enum GearSetTypeEnum
{
    TideSurge, LYNX, MordvoltInsulation,
    AburreysLegacy, MordvoltResistance, ArmoredMSGR,
    Ethertech, HotWork, Catastrophe,
    AICLight, RovingMSGR, Swordmancer,
    Type50Yinglung, PulserLabs, EternalXiranite,
    MiSecurity, AicHeavy, Bonekrusha,
    Frontiers, NonSet
}
public class Gear
{
    public struct GearStruct
    {
        public GearTypeEnum gearType;
        public GearAttributeEnum gearAttribute1;
        public GearAttributeEnum gearAttribute2;
        public GearStatEnum gearStat;
        public GearSetTypeEnum gearSetType;
    }
    private GearStruct _data;
    public GearStruct Data => _data;
}
