using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComboOrderUI : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject eBotton;
    [SerializeField] List<BebuffIcon> comboOrderUIList;
    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnComboOrderChanged += UpdateComboOrder;
        }
    }
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnComboOrderChanged -= UpdateComboOrder;
        }
    }
    private void UpdateComboOrder()
    {
        int j = 0;
        if (inputManager.ComboOrder.Count > 0)
        {
            if (!eBotton.activeSelf)
                eBotton.SetActive(true);
        }
        else
        {
            if (eBotton.activeSelf)
                eBotton.SetActive(false);
        }
        for (int i = 0; i < inputManager.ComboOrder.Count; i++, j++)
        {
            comboOrderUIList[j].SetIcon(inputManager.ComboOrder[i], inputManager.ComboOrderTime[i] / inputManager.ComboSkillTempTimeMax);
        }
        for (; j < comboOrderUIList.Count; j++)
        {
            comboOrderUIList[j].SetIcon(-1, 0);
        }
    }
}
