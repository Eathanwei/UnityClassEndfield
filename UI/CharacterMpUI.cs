using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterMpUI : MonoBehaviour
{
    [SerializeField] Image comboMpFillImage;
    [SerializeField] Image battleMpFillImage1;
    [SerializeField] Image battleMpFillImage2;
    [SerializeField] Image battleMpFillImage3;
    [SerializeField] Image finalMpFillImage;
    [SerializeField] Image mainHpFillImage;
    [SerializeField] TextMeshProUGUI mainHpTextMeshPro;
    [SerializeField] CharacterCombatLive characterCombatLive;
    [SerializeField] Color characterMpHighColor;
    [SerializeField] Color characterMpLowColor;
    [SerializeField] GameObject finalMpPicture;
    CombatUnit combatUnit;
    LocomotionBlend locomotionBlend;
    [SerializeField] InputManager inputManager;
    private void OnEnable()
    {
        combatUnit = characterCombatLive.GetComponent<CombatUnit>();
        locomotionBlend = characterCombatLive.GetComponent<LocomotionBlend>();
        if (characterCombatLive != null)
        {
            characterCombatLive.OnComboMpChanged += UpdateComboMpUI;
            characterCombatLive.OnBattleMpChanged += UpdateBattleMpUI;
            characterCombatLive.OnFinalMpChanged += UpdateFinalMpUI;
            combatUnit.OnHpChanged += UpdateHpUI;
        }
        if (inputManager != null)
        {
            inputManager.OnHpChanged += UpdateHpUI;
        }
        UpdateHpUI();
    }

    private void OnDisable()
    {
        if (characterCombatLive != null)
        {
            characterCombatLive.OnComboMpChanged -= UpdateComboMpUI;
            characterCombatLive.OnBattleMpChanged -= UpdateBattleMpUI;
            characterCombatLive.OnFinalMpChanged -= UpdateFinalMpUI;
            combatUnit.OnHpChanged -= UpdateHpUI;
        }
        if (inputManager != null)
        {
            inputManager.OnHpChanged -= UpdateHpUI;
        }
    }
    void UpdateComboMpUI(float comboMp, float comboMpMax)
    {
        comboMpFillImage.fillAmount = comboMp / comboMpMax;
        if (comboMpFillImage.fillAmount >= 1) 
        {
            comboMpFillImage.color = characterMpHighColor;
        }
        else
        {
            comboMpFillImage.color = characterMpLowColor;
        }
    }
    void UpdateBattleMpUI(float battleMp, float battleMpMax)
    {
        if (battleMp >= 300)
        {
            battleMpFillImage3.fillAmount = 1;
            battleMpFillImage3.color = characterMpHighColor;
        }
        else
        {
            battleMpFillImage3.fillAmount = Mathf.Max(0, (battleMp - 200) / 100);
            battleMpFillImage3.color = characterMpLowColor;
        }
        if (battleMp >= 200)
        {
            battleMpFillImage2.fillAmount = 1;
            battleMpFillImage2.color = characterMpHighColor;
        }
        else
        {
            battleMpFillImage2.fillAmount = Mathf.Max(0, (battleMp - 100) / 100);
            battleMpFillImage2.color = characterMpLowColor;
        }
        if (battleMp >= 100)
        {
            battleMpFillImage1.fillAmount = 1;
            battleMpFillImage1.color = characterMpHighColor;
        }
        else
        {
            battleMpFillImage1.fillAmount = battleMp / 100;
            battleMpFillImage1.color = characterMpLowColor;
        }
    }
    void UpdateFinalMpUI(float finalMp, float finalMpMax)
    {
        finalMpFillImage.fillAmount = finalMp / finalMpMax;
        if (finalMpFillImage.fillAmount >= 1)
        {
            if(!finalMpPicture.activeSelf)
                finalMpPicture.SetActive(true);
        }
        else
        {
            if (finalMpPicture.activeSelf)
                finalMpPicture.SetActive(false);
        }
    }
    void UpdateHpUI()
    {
        if (locomotionBlend.enabled)
        {
            mainHpFillImage.fillAmount = combatUnit.CombatUnitData.hp / combatUnit.CombatUnitData.hpMax;
            mainHpTextMeshPro.text = $"{(int)combatUnit.CombatUnitData.hp} / {(int)combatUnit.CombatUnitData.hpMax}";
        }
    }
}
