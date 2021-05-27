using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpManager : MonoBehaviour
{
    [Header("血條")]
    public Image bar;
    [Header("傷害數值")]
    public RectTransform rectDamage;

    public void UpdateHpbar(float hp, float hpMax)
    {
        bar.fillAmount = hp / hpMax;
    }

    public IEnumerator ShowDamage(float damage)
    {
        RectTransform rect = Instantiate(rectDamage, transform);
        rect.anchoredPosition = new Vector2(-200, -50);
        rect.GetComponent<Text>().text = damage.ToString();

        float y = rect.anchoredPosition.y;

        while (y < 100)
        {
            y += 10;
            rect.anchoredPosition = new Vector2(0, y);
            yield return new WaitForSeconds(0.06f);
        }

        Destroy(rect.gameObject);
    }
}
