using UnityEngine;
using System.Collections;
using static CombatUnit;
public class DragonBreath : MonoBehaviour
{
    private ParticleSystem firePS;
    [SerializeField] private float breathRange = 6f;
    [SerializeField] private float breathRadius = 0.8f;
    [SerializeField] private float damageInterval = 0.25f;

    int characterMask;

    void Awake()
    {
        characterMask = LayerMask.GetMask("Character");
        firePS = GetComponentInChildren<ParticleSystem>(true);
    }
    private void OnEnable()
    {
        ExecuteBreath();
    }

    public void ExecuteBreath()
    {
        StopAllCoroutines();
        //gameObject.SetActive(true);
        StartCoroutine(BreathRoutine());
    }

    private IEnumerator BreathRoutine()
    {
        firePS.Play(true);
        float duration = 2.5f;
        float timer = 0f;

        while (timer < duration)
        {
            // 1. 計算感測範圍的起點與終點
            Vector3 startPos = transform.position; // 從龍頭稍微往前一點開始
            Vector3 endPos = transform.position + transform.forward * breathRange; // 延伸到最大距離

            // 2. 直線膠囊感測 (像是把一根長管子橫在前方)
            Collider[] hitColliders = Physics.OverlapCapsule(startPos, endPos, breathRadius, characterMask);
            foreach (var hit in hitColliders)
            {
                hit.gameObject.GetComponent<CharacterCombatLive>().Hit(transform.position, Heat, 500);
                Debug.LogWarning("2");
            }

            yield return new WaitForSeconds(damageInterval);
            timer += damageInterval;
        }

        firePS.Stop(true);
        //gameObject.SetActive(false);
    }

    // 在 Scene 視窗畫出感測範圍，方便你調數值
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Matrix4x4 oldMatrix = Gizmos.matrix;

        // 讓 Gizmos 跟隨物件旋轉
        Gizmos.matrix = transform.localToWorldMatrix;

        // 畫出一個示意長方體代表範圍
        Gizmos.DrawWireCube(new Vector3(0, 0, breathRange / 2f), new Vector3(breathRadius * 2, breathRadius * 2, breathRange));

        Gizmos.matrix = oldMatrix;
    }
}
