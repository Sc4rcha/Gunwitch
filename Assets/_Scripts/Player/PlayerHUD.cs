using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header ("References")]
    public Image PlayerPortrait;
    public Image HealthBar;
    public Image ManaBar;
    public Button CraftingButton;

    [Header ("Variables")]
    public Color FocusOffColor;


    public void Setup()
    {
    }

    public void Refresh(Actor stats) 
    {
        HealthBar.fillAmount = (float)stats.HealthCurrent / (float)stats.Health;
        ManaBar.fillAmount = (float)stats.ManaCurrent / (float)stats.Mana;
    }
    public void SetFocusPlayer(bool isFocusOn)
    {
        if (isFocusOn)
            PlayerPortrait.color = Color.white;
        else
            PlayerPortrait.color = FocusOffColor;
    }
}
