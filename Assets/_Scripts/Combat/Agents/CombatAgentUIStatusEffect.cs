using UnityEngine;
using UnityEngine.UI;

public class CombatAgentUIStatusEffect : MonoBehaviour
{
    public Image StatusImage;

    public void Show(SOStatusEffect statusEffect) 
    {
        gameObject.SetActive(true);
        StatusImage.sprite = statusEffect.StatusSprite;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
