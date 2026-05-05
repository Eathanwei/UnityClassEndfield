using UnityEngine;

[CreateAssetMenu(fileName = "TacticalSO", menuName = "Scriptable Objects/TacticalSO")]
public class TacticalSO : ScriptableObject
{
    [SerializeField] EffectStruct effect;
    [SerializeField] TargetStruct target;
    [SerializeField] Condition1Struct condition1;
    [SerializeField] Condition2Struct condition2;
    public EffectStruct Effect => effect;
    public TargetStruct Target => target;
    public Condition1Struct Condition1 => condition1;
    public Condition2Struct Condition2 => condition2;

    [System.Serializable]
    public struct EffectStruct
    {
        [SerializeField] private float percentHP;
        [SerializeField] private float fixedHP;

        [SerializeField] private bool isPerSecond;
        [SerializeField] private float lastSeconds;
        [SerializeField] private float percentHpPerSecond;
        [SerializeField] private float fixedHpPerScecond;

        [SerializeField] private bool isClearArtsEffect;
        [SerializeField] private bool isRelive;
        [SerializeField] private float reliveHpPercnt;
        [SerializeField] private bool isUltimateGain;
        [SerializeField] private float ultimateGainPercent;

        public float PercentHP => percentHP;
        public float FixedHP => fixedHP;

        public bool IsPerSecond => isPerSecond;
        public float LastSeconds => lastSeconds;
        public float PercentHpPerSecond => percentHpPerSecond;
        public float FixedHpPerScecond => fixedHpPerScecond;

        public bool IsClearArtsEffect => isClearArtsEffect;
        public bool IsRelive => isRelive;
        public float ReliveHpPercnt => reliveHpPercnt;
        public bool IsUltimateGain => isUltimateGain;
        public float ltimateGainPercent => ultimateGainPercent;


    }


    [System.Serializable]
    public struct TargetStruct
    {
        [SerializeField] private bool anyCharacter;
        [SerializeField] private bool mainCharacterAndSelf;
        [SerializeField] private bool allCharacter;
        public bool AnyCharacter => anyCharacter;
        public bool MainCharacterAndSelf => mainCharacterAndSelf;
        public bool AllCharacter => allCharacter;
    }

    [System.Serializable]
    public struct Condition1Struct
    {
        [SerializeField] private bool isLowHp;
        [SerializeField] private float lowHpPercent;
        [SerializeField] private bool isLargeDamage;
        [SerializeField] private float largeDamagePercent;

        [SerializeField] private bool isGotArtsEffect;
        [SerializeField] private bool isDead;
        [SerializeField] private bool isLowUltimate;
        [SerializeField] private float lowUltimatePercent;

        public bool IsLowHp => isLowHp;
        public float LowHpPercent => lowHpPercent;
        public bool IsLargeDamage => isLargeDamage;
        public float LargeDamagePercent => largeDamagePercent;

        public bool IsGotArtsEffect => isGotArtsEffect;
        public bool IsDead => isDead;
        public bool IsLowUltimate => isLowUltimate;
        public float LowUltimatePercent => lowUltimatePercent;
    }

    [System.Serializable]
    public struct Condition2Struct
    {
        [SerializeField] private int maxUsePerCharacter;
        [SerializeField] private int maxUseMainCharacter;
        [SerializeField] private int initialAmount;
        [SerializeField] private bool isAmountRecover;
        [SerializeField] private float amountRecoverTime;

        public int MaxUsePerCharacter => maxUsePerCharacter;
        public int MaxUseMainCharacter => maxUseMainCharacter;
        public int InitialAmount => initialAmount;
        public bool IsAmountRecover => isAmountRecover;
        public float AmountRecoverTime => amountRecoverTime;
    }
}
