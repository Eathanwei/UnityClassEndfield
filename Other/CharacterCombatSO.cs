using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterCombatValue", menuName = "Scriptable Objects/CharacterCombatValue")]
public class CharacterCombatSO : ScriptableObject
{
    [SerializeField] BasicValue basic;
    [SerializeField] BasicAttackValue basicAttack;
    [SerializeField] BattleSkillValue battleSkill;
    [SerializeField] ComboSkillValue comboSkill;
    [SerializeField] UltimateSkillValue ultimateSkill;
    public BasicValue Basic => basic;
    public BasicAttackValue BasicAttack => basicAttack;
    public BattleSkillValue BattleSkill => battleSkill;
    public ComboSkillValue ComboSkill => comboSkill;
    public UltimateSkillValue UltimateSkill => ultimateSkill;
    [System.Serializable] public struct BasicValue
    {
        [SerializeField] private float strength;
        [SerializeField] private float agilility;
        [SerializeField] private float intellect;
        [SerializeField] private float will;
        [SerializeField] private int mainType;
        [SerializeField] private int viceType;
        [SerializeField] private float hp;
        [SerializeField] private float attack;
        public float Strength => strength;
        public float Agilility => agilility;
        public float Intellect => intellect;
        public float Will => will;
        public int MainType => mainType;
        public int ViceType => viceType;
        public float Hp => hp;
        public float Attack => attack;
    }
    [System.Serializable] public struct BasicAttackValue
    {
        [SerializeField] private int level;
        [SerializeField] private int batkSeqLen;
        [SerializeField] private List<int> batkSeqMultiplier;
        [SerializeField] private List<int> batkSeqTimeInterval;
        [SerializeField] private int finisherAtkMultiplier;
        [SerializeField] private int diveAtkMultiplier;
        public int Level => level;
        public int BatkSeqLen => batkSeqLen;
        public IReadOnlyList<int> BatkSeqMultiplier => batkSeqMultiplier;
        public IReadOnlyList<int> BatkSeqTimeInterval => batkSeqTimeInterval;
        public int FinisherAtkMultiplier => finisherAtkMultiplier;
        public int DiveAtkMultiplier => diveAtkMultiplier;
    }
    [System.Serializable] public struct BattleSkillValue
    {
        [SerializeField] private int level;
        public int Level => level;
        [SerializeField] private float spCost;
        public float SpCost => spCost;
    }
    [System.Serializable] public struct ComboSkillValue
    {
        [SerializeField] private int level;
        public int Level => level;
        [SerializeField] private float cooldown;
        public float Cooldown => cooldown;
    }
    [System.Serializable] public struct UltimateSkillValue
    {
        [SerializeField] private int level;
        public int Level => level;
        [SerializeField] private float ultimateEnergyCost;
        public float UltimateEnergyCost => ultimateEnergyCost;
        [SerializeField] private float cooldown;
        public float Cooldown => cooldown;
    }
}
