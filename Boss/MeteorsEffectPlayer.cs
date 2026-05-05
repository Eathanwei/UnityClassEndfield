using UnityEngine;
using System.Collections;
using static CombatUnit;

public class MeteorsEffectPlayer : MonoBehaviour
{
    public void Play()
    {
        float x = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);

        // 7. 設定目標位置
        Vector3 targetPos = new Vector3(
            PlaceManager.Instance.MainControlCharacter.transform.position.x + x,
            transform.position.y,
            PlaceManager.Instance.MainControlCharacter.transform.position.z + z
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
        }
        yield return new WaitForSeconds(2);
        hitColliders = Physics.OverlapSphere(transform.position, 4.5f, LayerMask.GetMask("Character"));
        foreach (var hit in hitColliders)
        {
            hit.gameObject.GetComponent<CharacterCombatLive>().Hit(transform.position, Heat, 750);
        }
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
