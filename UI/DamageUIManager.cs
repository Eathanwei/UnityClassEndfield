using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DamageUIManager : MonoBehaviour
{
    CombatUnit combatUnit;
    [SerializeField] List<TextMeshProUGUI> TextMeshPros;
    Coroutine[] TextMeshProCoroutines;
    [SerializeField] GameObject hpBar;
    [SerializeField] GameObject mainCamera;
    private void OnEnable()
    {
        combatUnit = transform.parent.GetComponent<CombatUnit>();
        if (combatUnit != null)
        {
            combatUnit.OnDamageHpChanged += UpdateHpUI;
        }
        TextMeshProCoroutines = new Coroutine[TextMeshPros.Count];
    }
    private void OnDisable()
    {
        if (combatUnit != null)
        {
            combatUnit.OnDamageHpChanged -= UpdateHpUI;
        }
    }
    void UpdateHpUI(int type, float damage)
    {
        if (type == -2)
        {
            type = TextMeshPros.Count - 1;
            TextMeshPros[type].SetText("+{0:0}", damage);
        }
        else
        {
            TextMeshPros[type].SetText("{0:0}", damage);
        }
        TextMeshPros[type].gameObject.SetActive(true);
        Vector3 basePos = hpBar.transform.position + new Vector3(0, -0.5f, 0);

        Vector2 v2Random = Random.insideUnitCircle;

        Vector3 worldOffset = (mainCamera.transform.right * v2Random.x) + (mainCamera.transform.up * v2Random.y * 0.5f) - mainCamera.transform.forward * 2f;

        Vector3 spawnPos = basePos + worldOffset;
        TextMeshPros[type].transform.position = spawnPos;
        if (TextMeshProCoroutines[type] != null)
            StopCoroutine(TextMeshProCoroutines[type]);
        TextMeshProCoroutines[type] = StartCoroutine(TextDisable(type));
    }
    IEnumerator TextDisable(int type)
    {
        yield return new WaitForSeconds(1f);
        TextMeshPros[type].gameObject.SetActive(false);
    }
}
