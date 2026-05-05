using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class BebuffIcon : MonoBehaviour
{
    [SerializeField] Image bar;
    RawImage[] icons;
    public void SetIcon(int index, float amount)
    {
        if (bar == null) bar = GetComponent<Image>();
        if (icons == null) icons = GetComponentsInChildren<RawImage>(true);
        //Debug.Log("SetIcon: " + index + " amount" + amount);
        if (index == -1)
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
            return;
        }
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        for (int i = 0; i < icons.Length; i++)
        {
            if (i == index)
            {
                bar.fillAmount = amount;
                if (!icons[index].gameObject.activeSelf)
                    icons[index].gameObject.SetActive(true);
            }
            else if(icons[i].gameObject.activeSelf)
            {
                icons[i].gameObject.SetActive(false);
            }
        }
    }
}
