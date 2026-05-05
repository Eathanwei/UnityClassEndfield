using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableSO", menuName = "Scriptable Objects/ConsumableSO")]
public class ConsumableSO : ScriptableObject
{

    [SerializeField] ConsumableStruct consumableStruct;

    [System.Serializable]
    public struct ConsumableStruct
    {
        [SerializeField] private float lastSeconds;
        public float LastSeconds => lastSeconds;

        [SerializeField] private bool isFixAttackBonus;
        [SerializeField] private float fixAttackBonus;
        [SerializeField] private bool isPercentAttackBonus;
        [SerializeField] private float percentAttackBonus;
        [SerializeField] private bool isPercentDefenseBonus;
        [SerializeField] private float percentDefenseBonus;
        public bool IsFixAttackBonus => isFixAttackBonus;
        public float FixAttackBonus => fixAttackBonus;
        public bool IsPercentAttackBonus => isPercentAttackBonus;
        public float PercentAttackBonus => percentAttackBonus;
        public bool IsPercentDefenseBonus => isPercentDefenseBonus;
        public float PercentDefenseBonus => percentDefenseBonus;

        [SerializeField] private bool isCriticalRate;
        [SerializeField] private float criticalRate;
        [SerializeField] private bool isFinalDMGBonus;
        [SerializeField] private float finalDMGBonus;
        [SerializeField] private bool isPhysicalDMGBonus;
        [SerializeField] private float physicalDMGBonus;
        [SerializeField] private bool isFinalDMGReduce;
        [SerializeField] private float finalDMGReduce;
        public bool IsCriticalRate => isCriticalRate;
        public float CriticalRate => criticalRate;
        public bool IsFinalDMGBonus => isFinalDMGBonus;
        public float FinalDMGBonus => finalDMGBonus;
        public bool IsPhysicalDMGBonus => isPhysicalDMGBonus;
        public float PhysicalDMGBonus => physicalDMGBonus;
        public bool IsFinalDMGReduce => isFinalDMGReduce;
        public float FinalDMGReduce => finalDMGReduce;

        [SerializeField] private bool isUltimateGainEfficiency;
        [SerializeField] private float ultimateGainEfficiency;
        [SerializeField] private bool isTreatmentReeceivedBonus;
        [SerializeField] private float treatmentReeceivedBonus;
        [SerializeField] private bool isAllAttributeBonus;
        [SerializeField] private float allAttributeBonus;
        public bool IsUltimateGainEfficiency => isUltimateGainEfficiency;
        public float UltimateGainEfficiency => ultimateGainEfficiency;
        public bool IsTreatmentReeceivedBonus => isTreatmentReeceivedBonus;
        public float TreatmentReeceivedBonus => treatmentReeceivedBonus;
        public bool IsAllAttributeBonus => isAllAttributeBonus;
        public float AllAttributeBonus => allAttributeBonus;
    }
}
