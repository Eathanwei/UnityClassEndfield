using UnityEngine;

public class MeteorsManager : MonoBehaviour
{
    MeteorsEffect[] meteorsEffects;
    [SerializeField] MeteorsEffectPlayer meteorsEffectPlayer;
    private void Awake()
    {
        meteorsEffects = GetComponentsInChildren<MeteorsEffect>(true);
    }
    public void PlayMeteor()
    {
        for (int i = 0; i < meteorsEffects.Length; i++)
        {
            meteorsEffects[i].Play(i, meteorsEffects.Length);
        }
        meteorsEffectPlayer.Play();
    }
}
