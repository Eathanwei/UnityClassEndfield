using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatUnitDataUI : MonoBehaviour
{
    [SerializeField] Image hpFillImage;
    [SerializeField] CombatUnit combatUnit;
    [SerializeField] List<BebuffIcon> debuffIcons;
    [SerializeField] List<BebuffIcon> inflictionIcons;
    [SerializeField] bool isCharacter;
    [SerializeField] Color characterHpHighColor;
    [SerializeField] Color characterHpLowColor;

    private void OnEnable()
    {
        if (combatUnit == null)
            combatUnit = transform.parent.GetComponent<CombatUnit>();
        if (combatUnit != null)
        {
            combatUnit.OnHpChanged += UpdateHpUI;
            combatUnit.OnDebuffChanged += UpdateDebuffUI;
            combatUnit.OnInflictionChanged += UpdateInflictionUI;
        }
    }
    private void OnDisable()
    {
        if (combatUnit != null)
        {
            combatUnit.OnHpChanged -= UpdateHpUI;
            combatUnit.OnDebuffChanged -= UpdateDebuffUI;
            combatUnit.OnInflictionChanged -= UpdateInflictionUI;
        }
    }
    void UpdateHpUI()
    {
        hpFillImage.fillAmount = combatUnit.CombatUnitData.hp / combatUnit.CombatUnitData.hpMax;
        if (isCharacter)
        {
            if (hpFillImage.fillAmount > 0.5)
            {
                hpFillImage.color = characterHpHighColor;
            }
            else
            {
                //hpFillImage.color = characterHpLowColor;
            }
        }
    }
    private void UpdateDebuffUI()
    {
        int j = 0;
        for (int i = 0; i < combatUnit.CombatUnitData.debuffLevels.Count; i++) 
        {
            if (combatUnit.CombatUnitData.debuffLevels[i] > 0) 
            {
                debuffIcons[j].SetIcon(i, combatUnit.CombatUnitData.debuffTimes[i] / CombatUnit.debuffTimeTypeMaxes[i]);
                j++;
            }
        }
        for (; j < debuffIcons.Count; j++)
        {
            debuffIcons[j].SetIcon(-1, 0);
        }
    }
    private void UpdateInflictionUI()
    {
        int j = 0;
        for (int i = combatUnit.CombatUnitData.inflictions.Count - 1; i >= 0; i--) 
        {
            //Debug.Log("infliction " + i + " amount " + combatUnit.CombatUnitData.inflictions[i]);
            if (combatUnit.CombatUnitData.inflictions[i] > 0)
            {
                inflictionIcons[j].SetIcon(i, (float)combatUnit.CombatUnitData.inflictions[i] / 4f);
                j++;
            }
        }
        for (; j < inflictionIcons.Count; j++)
        {
            inflictionIcons[j].SetIcon(-1, 0);
        }
    }
}
