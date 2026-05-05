public class Essence
{
    public struct EssenceStruct
    {
        public WeaponAttributeEnum essenceAttribute;
        public WeaponSecondaryEnum essenceSecondary;
        public WeaponSkillEnum essenceSkill;
        public int essenceAttributeLevel, essenceSecondaryLevel, essenceSkillLevel;
        public int essenceAttributeMaxLevel, essenceSecondaryMaxLevel, essenceSkillMaxLevel;
    }
    private EssenceStruct _data;
    public EssenceStruct Data => _data;
}
