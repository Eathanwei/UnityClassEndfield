using UnityEngine;
using System.Collections;
public class MarbleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem Marble;
    [SerializeField] ParticleSystem MarbleGet;
    [SerializeField] EnemyCombatData enemyCombatData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        Marble.Stop(true);
        Marble.gameObject.SetActive(false);
        MarbleGet.gameObject.SetActive(true);
        MarbleGet.Play();
        StartCoroutine(PlayAndDisable());
    }
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
        Marble.gameObject.SetActive(true);
    }
    private IEnumerator PlayAndDisable()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        enemyCombatData.finalDMGReduce -= 0.1f;
    }
}
