using GameInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static CombatPlayer;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public Image HealthBar;
    public Image ManaBar;
    public Button CraftingButton;
    public PlayerHUDPortrait Portrait;
    public PlayerPortraitShake Shake;

    private ManagerPlayer player;

    private Coroutine damageCoroutine;
    private WaitForSeconds damageMessageDelay;

    public void Setup(ManagerPlayer player)
    {
        this.player = player;

        Portrait.Setup();

        damageMessageDelay = new WaitForSeconds(0.75f);
        Shake.enabled = false;
    }

    // refresh player status
    public void Refresh()
    {
        
        HealthBar.fillAmount = (float)player.Info.HealthCurrent / (float)player.Info.Health;
        ManaBar.fillAmount = (float)player.Info.ManaCurrent / (float)player.Info.Mana;
    }


    public void Damage(string value) 
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageAnimation(value));
    }


    private IEnumerator DamageAnimation(string value) 
    {
        yield return damageMessageDelay;

        if (value != "MISS")
            Shake.ShakeStart(3);

        Portrait.HitNumber.ShowNumber(value, 3);
        Refresh();


        damageCoroutine = null;
    }
}
