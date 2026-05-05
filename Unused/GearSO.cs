using UnityEngine;

[CreateAssetMenu(fileName = "GearSO", menuName = "Scriptable Objects/GearSO")]
public class GearSO : ScriptableObject
{
    [SerializeField] GearTypeEnum gearType;
    [SerializeField] GearAttributeEnum gearAttribute1;
    [SerializeField] GearAttributeEnum gearAttribute2;
    [SerializeField] GearStatEnum gearStat;
    [SerializeField] GearSetTypeEnum gearSetType;
    public GearTypeEnum GearType  => gearType;
    public GearAttributeEnum GearAttribute1 => gearAttribute1;
    public GearAttributeEnum GearAttribute2  => gearAttribute2;
    public GearStatEnum GearStat => gearStat;
    public GearSetTypeEnum GearSetType  => gearSetType;
}
