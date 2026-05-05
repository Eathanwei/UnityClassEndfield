using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    MarbleEffect[] marbleEffects;
    private void Awake()
    {
        marbleEffects = GetComponentsInChildren<MarbleEffect>(true);
    }
    public void PlayMarble()
    {
        for (int i = 0; i < marbleEffects.Length; i++)
        {
            marbleEffects[i].Play(i, marbleEffects.Length);
        }
    }
}
