using UnityEngine;
using System.Collections;
using static CombatUnit;
public class MeteorsEffect : MonoBehaviour
{
    public void Play(int i, int count)
    {
        Vector3 parentPos = transform.parent.position;
        float radius = 30f;
        float angleStep = 360f / count;
        float minAngle = i * angleStep;
        float maxAngle = (i + 1) * angleStep;

        float randomAngle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        float randomDistance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        float x = Mathf.Cos(randomAngle) * randomDistance;
        float z = Mathf.Sin(randomAngle) * randomDistance;

        // 7. 設定目標位置
        Vector3 targetPos = new Vector3(
            parentPos.x + x,
            parentPos.y,
            parentPos.z + z
        );
        transform.position = targetPos;
        gameObject.SetActive(true);
        StartCoroutine(PlayAndDisable());
    }

    private IEnumerator PlayAndDisable()
    {
        yield return new WaitForSeconds(4);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4.5f, LayerMask.GetMask("Character"));
        foreach (var hit in hitColliders)
        {
            hit.gameObject.GetComponent<CharacterCombatLive>().Hit(transform.position, Heat, 750);
            Debug.LogWarning("3");
        }
        yield return new WaitForSeconds(2);
        hitColliders = Physics.OverlapSphere(transform.position, 4.5f, LayerMask.GetMask("Character"));
        foreach (var hit in hitColliders)
        {
            hit.gameObject.GetComponent<CharacterCombatLive>().Hit(transform.position, Heat, 750);
            Debug.LogWarning("3");
        }
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
