using UnityEngine;
using static TacticalSO;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] int typeId;
    [SerializeField] WeaponTypeEnum weaponType;
    [SerializeField] WeaponAttributeEnum weaponAttribute;
    [SerializeField] WeaponSecondaryEnum weaponSecondary;
    [SerializeField] WeaponSkillEnum weaponSkill;
    public int TypeId => typeId;
    public WeaponTypeEnum WeaponType => weaponType;
    public WeaponAttributeEnum WeaponAttribute => weaponAttribute;
    public WeaponSecondaryEnum WeaponSecondary => weaponSecondary;
    public WeaponSkillEnum WeaponSkill => weaponSkill;
}
