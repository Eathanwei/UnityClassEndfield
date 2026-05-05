public class CharacterBasicData
{
    public struct CharacterBasicDataStruct
    {
        public CharacterCombatSO characterCombatValue;
        public int level, exp, elite;
        public int skirmisherLevel, talent1Level, talent2Level;
        public float strength, agilility, intellect, will;
        public int mainType, viceType;
        public float hp, attack;
        public int potential;
    }
    private CharacterBasicDataStruct _data;
    public CharacterBasicDataStruct Data => _data;

    public CharacterBasicData(CharacterCombatSO characterCombatSO)
    {
        _data.characterCombatValue = characterCombatSO;
        SetBasicValue();
    }
    void SetBasicValue()
    {
        _data.strength = _data.characterCombatValue.Basic.Strength;
        _data.agilility = _data.characterCombatValue.Basic.Agilility;
        _data.intellect = _data.characterCombatValue.Basic.Intellect;
        _data.will = _data.characterCombatValue.Basic.Will;
        _data.mainType = _data.characterCombatValue.Basic.MainType;
        _data.viceType = _data.characterCombatValue.Basic.ViceType;
        _data.hp = _data.characterCombatValue.Basic.Hp;
        _data.attack = _data.characterCombatValue.Basic.Attack;
    }
}
