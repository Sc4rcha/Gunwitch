using GameInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public Image HealthBar;
    public Image ManaBar;
    public Button CraftingButton;
    public PlayerHUDPortrait Portrait;

    public void Setup()
    {
        Portrait.Setup();
    }

    // refresh player status
    public void Refresh(Actor stats)
    {
        HealthBar.fillAmount = (float)stats.HealthCurrent / (float)stats.Health;
        ManaBar.fillAmount = (float)stats.ManaCurrent / (float)stats.Mana;
    }

}
